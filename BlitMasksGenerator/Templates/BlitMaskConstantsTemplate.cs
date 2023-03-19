/* Copyright © 2023 - Juho Veli-Matti Joensuu
 * 
 * All Rights Reserved.
 */

namespace BlitMaskGenerators
{
    /// <summary>
    /// Provides <see langword="const"/> internal _value fields <see langword="value"/> to avoid typo mistakes for BlitMask classes
    /// </summary>
    public static class BlitMaskConstantsTemplate
    {
        /// <summary>
        /// <see cref="BlitMaskTemplate._value"/> <see langword="const"/> for no flags / zero value.
        /// </summary>
        public const uint Zero = 0x00000000;
        /// <summary>
        /// <see cref="BlitMaskTemplate._value"/> <see langword="const"/> for all flags <see langword="true"/> / <see cref="uint.MaxValue"/>
        /// </summary>
        public const uint Complement = 0xFFFFFFFF;
        /// <summary>
        /// <see cref="BlitMaskTemplate._value"/> <see langword="const"/> for number one, for easy type setting.
        /// </summary>
        public const uint FirstBit = 0x00000001;
    }
}
