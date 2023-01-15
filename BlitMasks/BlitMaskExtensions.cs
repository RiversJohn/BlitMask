namespace BlitMasks
{
    public static class BlitMaskExtensions
    {
        public static bool GetFlag(this BlitMask64 mask, int flag)
        {
            return (mask._value & (1ul << flag)) != 0;
        }
        public static bool GetFlag(this BlitMask32 mask, int flag)
        {
            return (mask._value & (1u << flag)) != 0;
        }

        public static void SetFlag(this ref BlitMask64 mask, int flag)
        {
            mask._value |= (1ul << flag);
        }
        public static void SetFlag(this ref BlitMask32 mask, int flag)
        {
            mask._value |= (1u << flag);
        }

        public static void ClearFlag(this ref BlitMask64 mask, int flag)
        {
            mask._value &= ~(1ul << flag);
        }
        public static void ClearFlag(this ref BlitMask32 mask, int flag)
        {
            mask._value &= ~(1u << flag);
        }

        public static void ToggleFlag(this ref BlitMask64 mask, int flag)
        {
            mask._value ^= (1ul << flag);
        }
        public static void ToggleFlag(this ref BlitMask32 mask, int flag)
        {
            mask._value ^= (1u << flag);
        }

        public static bool HasAnyFlag(this BlitMask64 mask, ulong flags)
        {
            return (mask._value & flags) != 0ul;
        }
        public static bool HasAnyFlag(this BlitMask32 mask, uint flags)
        {
            return (mask._value & flags) != 0u;
        }

        public static bool HasAllFlags(this BlitMask64 mask, ulong flags)
        {
            return (mask._value & flags) == flags;
        }
        public static bool HasAllFlags(this BlitMask32 mask, uint flags)
        {
            return (mask._value & flags) == flags;
        }

        public static void SetFlags(this ref BlitMask64 mask, params int[] flags)
        {
            mask |= new BlitMask64(flags);
        }
        public static void SetFlags(this ref BlitMask32 mask, params int[] flags)
        {
            mask |= new BlitMask32(flags);
        }

        public static void ClearFlags(this ref BlitMask64 mask, params int[] flags)
        {
            mask &= ~new BlitMask64(flags);
        }
        public static void ClearFlags(this ref BlitMask32 mask, params int[] flags)
        {
            mask &= ~new BlitMask32(flags);
        }

        public static void ToggleFlags(this ref BlitMask64 mask, params int[] flags)
        {
            mask ^= new BlitMask64(flags);
        }

        public static void ToggleFlags(this ref BlitMask32 mask, params int[] flags)
        {
            mask ^= new BlitMask32(flags);
        }

        public static void SetAllFlags(this ref BlitMask64 mask, bool setFlags)
        {
            mask._value = setFlags ? Constants.Everything64 : Constants.None64;
        }
        public static void SetAllFlags(this ref BlitMask32 mask, bool setFlags)
        {
            mask._value = setFlags ? Constants.Everything32 : Constants.None32;
        }
    }
}
