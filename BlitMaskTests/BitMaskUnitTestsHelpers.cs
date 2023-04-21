using System.Reflection;

namespace BlitMaskTests
{
    internal static class BlittableTestHelper
    {
        internal static bool IsBlittableType(Type type)
    => IsBlittablePrimitive(type)
    || IsBlittableArray(type)
    || IsBlittableStruct(type)
    || IsBlittableClass(type);
        static bool IsBlittablePrimitive(Type type)
            => type == typeof(byte)
            || type == typeof(sbyte)
            || type == typeof(short)
            || type == typeof(ushort)
            || type == typeof(int)
            || type == typeof(uint)
            || type == typeof(long)
            || type == typeof(ulong)
            || type == typeof(IntPtr)
            || type == typeof(UIntPtr)
            || type == typeof(float)
            || type == typeof(double)
            ;
        static bool IsBlittableArray(Type type)
            => type.IsArray
            && type.GetArrayRank() == 1
            && IsBlittablePrimitive(type.GetElementType())
            ;
        static bool IsBlittableStruct(Type type)
            => type.IsValueType
            && !type.IsPrimitive
            && type.IsLayoutSequential
            && type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).All(IsBlittableField);
        static bool IsBlittableClass(Type type)
            => !type.IsValueType
            && !type.IsPrimitive
            && type.IsLayoutSequential
            && type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).All(IsBlittableField);
        static bool IsBlittableField(FieldInfo field)
            => IsBlittablePrimitive(field.FieldType)
            || IsBlittableStruct(field.FieldType);
    }
}