namespace BlitMasks
{
    using System.Globalization;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack = 0, Size = sizeof(uint))]
    public struct BlitMask32 : IEquatable<BlitMask32>, IFormattable
    {
        public static Type GetUnderlyingType() => typeof(uint);

        internal uint _value;

        public BlitMask32() { _value = Constants.None32; }

        public BlitMask32(Enum enumValue) { _value = Convert.ToUInt32(enumValue); }

        public BlitMask32(uint value) { _value = value; }

        public BlitMask32(BlitMask64 mask) { unchecked { _value = (uint)mask._value; } }

        public BlitMask32(BlitMask32 mask) { _value = mask._value; }

        public BlitMask32(bool flagsToValue = false)
        {
            _value = flagsToValue ? Constants.Everything32 : Constants.None32;
        }

        public BlitMask32(params int[] flags)
        {
            _value = Constants.None32;

            var flagLenght = flags.Length;
            for ( int flag = 0; flag < flagLenght; flag++ )
            {
                _value |= 1U << flags[flag];
            }
        }

        public static implicit operator uint(BlitMask32 mask) => mask._value;
        public static implicit operator BlitMask32(uint value) => new(value);

        public static BlitMask32 operator +(BlitMask32 mask1, BlitMask32 mask2) => new(mask1._value | mask2._value);
        public static BlitMask32 operator |(BlitMask32 mask1, BlitMask32 mask2) => new(mask1._value | mask2._value);
        public static BlitMask32 operator -(BlitMask32 mask1, BlitMask32 mask2) => new(mask1._value & ~mask2._value);
        public static BlitMask32 operator ^(BlitMask32 mask1, BlitMask32 mask2) => new(mask1._value ^ mask2._value);
        public static BlitMask32 operator ~(BlitMask32 mask) => new(~mask._value);

        public static bool operator ==(BlitMask32 mask1, BlitMask32 mask2) => mask1._value == mask2._value;
        public static bool operator !=(BlitMask32 mask1, BlitMask32 mask2) => mask1._value != mask2._value;

        public override string ToString() => _value.ToString();

        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            if ( string.IsNullOrWhiteSpace(format) ) format = null;
            if ( formatProvider == null ) formatProvider = NumberFormatInfo.InvariantInfo;

            return _value.ToString(format, formatProvider);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_value);
        }

        public override bool Equals(object? obj)
        {
            return obj is BlitMask32 mask && Equals(mask);
        }

        public bool Equals(BlitMask32 other)
        {
            return _value == other._value;
        }
    }
}
