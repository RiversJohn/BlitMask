using System;
using System.Linq;
using System.Reflection;

namespace BlitMaskTests
{
    public static class BlittableTypeTest
    {
        /// <summary>
        /// Checks if a <see cref="Type"/> is Blittable, meaning the memory representation of it is the same in managed and unmanaged memory,<br/>
        /// avoiding the need for boxing or marshalling when a struct is passed from managed to unmanaged code or vice versa.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsBlittableType(Type type)
            => IsBlittablePrimitive(type)
            || IsBlittableArray(type)
            || IsBlittableStruct(type)
            || IsBlittableClass(type);

        /// <summary>
        /// Checks if a type is a Blittable primitive.
        /// </summary>
        /// <param name="type"></param>
        /// <returns><see langword="bool"/> indicating if the type is of Blittable Primitive <see cref="Type"/></returns>
        public static bool IsBlittablePrimitive(Type type)
            => type == typeof(byte) || type == typeof(sbyte)
            || type == typeof(short) || type == typeof(ushort)
            || type == typeof(int) || type == typeof(uint)
            || type == typeof(IntPtr) || type == typeof(UIntPtr)
            || type == typeof(long) || type == typeof(ulong)
            || type == typeof(float) || type == typeof(double);

        /// <summary>
        /// Check if the Array can be vectorized and that the element type of the array is a blittable primitive.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsBlittableArray(Type type)
            => type.IsArray
            && type.GetArrayRank() == 1
            && IsBlittablePrimitive(type.GetElementType());

        /// <summary>
        /// Check if a <see langword="struct"/> is Blittable by verifying all fields of the <see langword="struct"/> are blittable and the memory layout is sequential.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsBlittableStruct(Type type)
            => type.IsValueType
            && !type.IsPrimitive
            && type.IsLayoutSequential
            && type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).All(IsBlittableField);

        /// <summary>
        /// Check if a <see langword="class"/> is Blittable by verifying all fields of the <see langword="class"/> are blittable, is not a primitive or valuetype and the memory layout is sequential.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsBlittableClass(Type type)
            => !type.IsValueType
            && !type.IsPrimitive
            && type.IsLayoutSequential
            && type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).All(IsBlittableField);

        /// <summary>
        /// Check if a field is blittable by checking if the field type is either Blittable <see langword="struct"/> or primitive.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static bool IsBlittableField(FieldInfo field)
            => IsBlittablePrimitive(field.FieldType)
            || IsBlittableStruct(field.FieldType);
    }
}