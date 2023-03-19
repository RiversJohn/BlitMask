/* Copyright © 2023 - Juho Veli-Matti Joensuu
 * 
 * All Rights Reserved.
 */

using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace BlitMaskGenerators
{
    [StructLayout(LayoutKind.Sequential, Pack = 0, Size = sizeof(uint))]
    public struct BlitMaskTemplate : IEquatable<BlitMaskTemplate>, IFormattable
    {
        /// <returns><see cref="Type"/> of the underlying data backing field</returns>
        public static Type GetUnderlyingType() => typeof(uint);

        /// <summary>
        /// The underlying value of the <see cref="BlitMaskTemplate"/>
        /// </summary>
        internal uint _value;

        /// <summary>
        /// <see langword="default"/> value of the field is no flags set.
        /// </summary>
        public BlitMaskTemplate() { _value = BlitMaskConstantsTemplate.Zero; }

        /// <summary>
        /// Easy conversion from an enum value into a <see cref="BlitMaskTemplate"/> that has the same <see cref="Type"/><br/>
        /// Use <see cref="GetUnderlyingType"/> to see what the underlying <see cref="Type"/> is.
        /// </summary>
        /// <param name="enumValue"></param>
        public BlitMaskTemplate(Enum enumValue) { _value = Convert.ToUInt32(enumValue); }

        /// <summary>
        /// Create a <see cref="BlitMaskTemplate"/> from <see cref="uint"/>
        /// </summary>
        /// <param name="value"></param>
        public BlitMaskTemplate(uint value) { _value = value; }

        /// <summary>
        /// Copies the value of one <see cref="BlitMaskTemplate"/> to another one.
        /// </summary>
        /// <param name="mask"></param>
        public BlitMaskTemplate(BlitMaskTemplate mask) { _value = mask._value; }

        /// <summary>
        /// Set all flags to supplied <see langword="bool"/> value.
        /// </summary>
        /// <param name="flagsToValue">all flags will be set to this <see langword="bool"/></param>
        public BlitMaskTemplate(bool flagsToValue = false)
        {
            _value = flagsToValue ? BlitMaskConstantsTemplate.Complement : BlitMaskConstantsTemplate.Zero;
        }

        /// <summary>
        /// Create a mask with all supplied flags set to <see langword="true"/>, unset flags will be <see langword="false"/>
        /// </summary>
        /// <param name="flags">Flag positions to set true</param>
        public BlitMaskTemplate(params int[] flags)
        {
            _value = BlitMaskConstantsTemplate.Zero;

            var flagLength = flags.Length;
            for ( int flag = 0; flag < flagLength; flag++ )
            {
                _value |= BlitMaskConstantsTemplate.FirstBit << flags[flag];
            }
        }

        public static implicit operator uint(BlitMaskTemplate mask) => mask._value;
        public static implicit operator BlitMaskTemplate(uint value) => new(value);

        /// <summary>
        /// Combine two masks, where all bits set in either mask will be true.
        /// </summary>
        /// <param name="mask1"></param>
        /// <param name="mask2"></param>
        /// <returns></returns>
        public static BlitMaskTemplate operator +(BlitMaskTemplate mask1, BlitMaskTemplate mask2) => new(mask1._value | mask2._value);

        /// <summary>
        /// Combine two masks, where all bits set in either mask will be true.
        /// </summary>
        /// <param name="mask1"></param>
        /// <param name="mask2"></param>
        /// <returns></returns>
        public static BlitMaskTemplate operator |(BlitMaskTemplate mask1, BlitMaskTemplate mask2) => new(mask1._value | mask2._value);

        /// <summary>
        /// Remove all bits set in <paramref name="mask2"/> from <paramref name="mask1"/>
        /// </summary>
        /// <param name="mask1"></param>
        /// <param name="mask2"></param>
        /// <returns>New <paramref name="mask1"/> but without the flags present in <paramref name="mask2"/></returns>
        public static BlitMaskTemplate operator -(BlitMaskTemplate mask1, BlitMaskTemplate mask2) => new(mask1._value & ~mask2._value);

        /// <summary>
        /// Create a new mask that only has bits set that are present only in <paramref name="mask1"/> OR <paramref name="mask2"/> but not both.
        /// </summary>
        /// <param name="mask1"></param>
        /// <param name="mask2"></param>
        /// <returns>Exclusive OR <see cref="BlitMaskTemplate"/> with only the bits set that were in one mask but not the other.</returns>
        public static BlitMaskTemplate operator ^(BlitMaskTemplate mask1, BlitMaskTemplate mask2) => new(mask1._value ^ mask2._value);

        /// <summary>
        /// Inverts the bitmask, all bits that were set <see langword="true"/> will become <see langword="false"/> and vice versa.
        /// </summary>
        /// <param name="mask"></param>
        /// <returns>New <see cref="BlitMaskTemplate"/> with opposite bits set than in <paramref name="mask"/></returns>
        public static BlitMaskTemplate operator ~(BlitMaskTemplate mask) => new(~mask._value);

        /// <summary>
        /// Test if <paramref name="mask1"/> and <paramref name="mask2"/> have equal value.
        /// </summary>
        /// <param name="mask1"></param>
        /// <param name="mask2"></param>
        /// <returns><see langword="true"/> if the two masks have equal <see langword="value"/></returns>
        public static bool operator ==(BlitMaskTemplate mask1, BlitMaskTemplate mask2) => mask1._value == mask2._value;

        /// <summary>
        /// Test if <paramref name="mask1"/> and <paramref name="mask2"/> have a different value.
        /// </summary>
        /// <param name="mask1"></param>
        /// <param name="mask2"></param>
        /// <returns><see langword="true"/> if the two masks are not of equal <see langword="value"/></returns>
        public static bool operator !=(BlitMaskTemplate mask1, BlitMaskTemplate mask2) => mask1._value != mask2._value;

        public override int GetHashCode()
        {
            return _value.GetType().GetHashCode(); ;
        }

        public bool Equals(BlitMaskTemplate other)
        {
            return _value == other._value;
        }

        /// <summary>
        /// Underlying value to string.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => _value.ToString();

        /// <summary>
        /// Formattable overload for ToString
        /// </summary>
        /// <param name="format">Optional <see cref="String"/> formatting expression</param>
        /// <param name="formatProvider">Optional <see cref="IFormatProvider"/></param>
        /// <returns></returns>
        public string ToString(string? format = null, IFormatProvider? formatProvider = null)
        {
            if ( string.IsNullOrWhiteSpace(format) ) format = null;
            formatProvider ??= NumberFormatInfo.InvariantInfo;

            return _value.ToString(format, formatProvider);
        }

        public override bool Equals(object? obj)
        {
            return obj is BlitMaskTemplate mask && Equals(mask);
        }
    }
}
