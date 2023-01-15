namespace BlitMasks
{
    using System.Globalization;
    using System.Numerics;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack = 0, Size = sizeof(ulong))]
    public struct BlitMask64 : IEquatable<BlitMask64>, IFormattable
    {
        public static Type GetUnderlyingType() => typeof(ulong);

        internal ulong _value;

        public BlitMask64() { _value = Constants.None64; }

        public BlitMask64(Enum enumValue) { _value = Convert.ToUInt64(enumValue); }

        public BlitMask64(ulong value) { _value = value; }

        public BlitMask64(BlitMask32 mask) { unchecked { _value = mask._value; } }

        public BlitMask64(BlitMask64 mask) { _value = mask._value; }

        public BlitMask64(bool flagsToValue = false)
        {
            _value = flagsToValue ? Constants.Everything64 : Constants.None64;
        }

        public BlitMask64(params int[] flags)
        {
            _value = Constants.None64;

            var flagLenght = flags.Length;
            for ( int flag = 0; flag < flagLenght; flag++ )
            {
                _value |= 1UL << flags[flag];
            }
        }

        public static implicit operator ulong(BlitMask64 mask) => mask._value;
        public static implicit operator BlitMask64(ulong value) => new(value);

        public static BlitMask64 operator +(BlitMask64 mask1, BlitMask64 mask2) => new(mask1._value | mask2._value);
        public static BlitMask64 operator |(BlitMask64 mask1, BlitMask64 mask2) => new(mask1._value | mask2._value);
        public static BlitMask64 operator -(BlitMask64 mask1, BlitMask64 mask2) => new(mask1._value & ~mask2._value);
        public static BlitMask64 operator ^(BlitMask64 mask1, BlitMask64 mask2) => new(mask1._value ^ mask2._value);
        public static BlitMask64 operator ~(BlitMask64 mask) => new(~mask._value);

        public static bool operator ==(BlitMask64 mask1, BlitMask64 mask2) => mask1._value == mask2._value;
        public static bool operator !=(BlitMask64 mask1, BlitMask64 mask2) => mask1._value != mask2._value;

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
            return obj is BlitMask64 mask && Equals(mask);
        }

        public bool Equals(BlitMask64 other)
        {
            return _value == other._value;
        }
    }
}
