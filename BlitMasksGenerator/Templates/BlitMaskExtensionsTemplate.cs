/* Copyright © 2023 - Juho Veli-Matti Joensuu
 * 
 * All Rights Reserved.
 */

namespace BlitMaskGenerators
{
    /// <summary>
    /// Provides extension methods to <see cref="BlitMaskTemplate"/> <see cref="Type"/>'s
    /// </summary>
    public static class BlitMaskExtensionsTemplate
    {
        /// <summary>
        /// Check if a given <paramref name="flag"/> is <see langword="true"/> in <paramref name="mask"/>
        /// </summary>
        /// <param name="mask"><see cref="BlitMaskTemplate"/> to test</param>
        /// <param name="flag">Flag position in the mask</param>
        /// <returns></returns>
        public static bool HasFlag(this BlitMaskTemplate mask, int flag)
        {
            return (mask._value & (BlitMaskConstantsTemplate.FirstBit << flag)) != BlitMaskConstantsTemplate.Zero;
        }

        /// <summary>
        /// Set a given <paramref name="flag"/> <see langword="true"/> in <paramref name="mask"/>
        /// </summary>
        /// <param name="mask"><see cref="BlitMaskTemplate"/> to modify</param>
        /// <param name="flag">Flag position in the mask</param>
        /// <returns></returns>
        public static void SetFlag(this ref BlitMaskTemplate mask, int flag)
        {
            mask._value |= (BlitMaskConstantsTemplate.FirstBit << flag);
        }

        /// <summary>
        /// Clear a given <paramref name="flag"/> to <see langword="false"/> in <paramref name="mask"/>
        /// </summary>
        /// <param name="mask"><see cref="BlitMaskTemplate"/> to modify</param>
        /// <param name="flag">Flag position in the mask</param>
        /// <returns></returns>
        public static void ClearFlag(this ref BlitMaskTemplate mask, int flag)
        {
            mask._value &= ~(BlitMaskConstantsTemplate.FirstBit << flag);
        }

        /// <summary>
        /// Flip a given <paramref name="flag"/> to opposite <see langword="bool"/> in <paramref name="mask"/>
        /// </summary>
        /// <param name="mask"><see cref="BlitMaskTemplate"/> to modify</param>
        /// <param name="flag">Flag position in the mask</param>
        /// <returns></returns>
        public static void ToggleFlag(this ref BlitMaskTemplate mask, int flag)
        {
            mask._value ^= (BlitMaskConstantsTemplate.FirstBit << flag);
        }

        /// <summary>
        /// Check if ANY <paramref name="flags"/> in <paramref name="mask"/> is <see langword="true"/>
        /// </summary>
        /// <param name="mask"><see cref="BlitMaskTemplate"/> to test against</param>
        /// <param name="flags">Flags to test</param>
        /// <returns></returns>
        public static bool HasAnyFlag(this BlitMaskTemplate mask, uint flags)
        {
            return (mask._value & flags) != BlitMaskConstantsTemplate.Zero;
        }

        /// <summary>
        /// Check if ALL <paramref name="flags"/> in <paramref name="mask"/> is <see langword="true"/>
        /// </summary>
        /// <param name="mask"><see cref="BlitMask64Original"/> to test against</param>
        /// <param name="flags">Flags to test</param>
        /// <returns></returns>
        public static bool HasAllFlags(this BlitMaskTemplate mask, uint flags)
        {
            return (mask._value & flags) == flags;
        }

        /// <summary>
        /// Set all <paramref name="flags"/> to <see langword="true"/> in <paramref name="mask"/>
        /// </summary>
        /// <param name="mask"><see cref="BlitMaskTemplate"/> to modify</param>
        /// <param name="flags">Flag positions in the mask</param>
        /// <returns></returns>
        public static void SetFlags(this ref BlitMaskTemplate mask, params int[] flags)
        {
            mask |= new BlitMaskTemplate(flags);
        }

        /// <summary>
        /// Clear all <paramref name="flags"/> to <see langword="false"/> in <paramref name="mask"/>
        /// </summary>
        /// <param name="mask"><see cref="BlitMaskTemplate"/> to modify</param>
        /// <param name="flags">Flag positions in the mask</param>
        /// <returns></returns>
        public static void ClearFlags(this ref BlitMaskTemplate mask, params int[] flags)
        {
            mask &= ~new BlitMaskTemplate(flags);
        }

        /// <summary>
        /// Flip all <paramref name="flags"/> to opposite <see langword="bool"/> in <paramref name="mask"/>
        /// </summary>
        /// <param name="mask"><see cref="BlitMaskTemplate"/> to modify</param>
        /// <param name="flags">Flag positions in the mask</param>
        /// <returns></returns>
        public static void ToggleFlags(this ref BlitMaskTemplate mask, params int[] flags)
        {
            mask ^= new BlitMaskTemplate(flags);
        }

        /// <summary>
        /// Set all flags to a <see langword="bool"/> <paramref name="setFlags"/> in <paramref name="mask"/>
        /// </summary>
        /// <param name="mask"><see cref="BlitMaskTemplate"/> to modify</param>
        /// <param name="setFlags"><see langword="bool"/> <see langword="value"/> to set all flags in the mask to.</param>
        /// <returns></returns>
        public static void SetAllFlags(this ref BlitMaskTemplate mask, bool setFlags)
        {
            mask._value = setFlags ? BlitMaskConstantsTemplate.Complement : BlitMaskConstantsTemplate.Zero;
        }
    }
}
