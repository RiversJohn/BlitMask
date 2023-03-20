/* Copyright © 2023 - Juho Veli-Matti Joensuu
 * 
 * See the attached LICENSE.md for licensing details (MIT licensing)
 */

namespace BlitMask
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

        public const uint One = 1U;
    }
}
