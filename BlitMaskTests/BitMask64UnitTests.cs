using BlitMasks;

namespace BlitMaskTests
{
    using BitMask = BlitMask64;
    [TestClass]
    public class BitMask64UnitTests
    {
        [TestMethod]
        public void GetFlag()
        {
            int testValue = 2;

            BitMask bitMask = new BitMask(testValue);

            int testValuePow2 = testValue * testValue;

            /// This presumes that cast from <see cref="BitMask"/> to <see cref="uint"/> functions correctly
            Assert.IsTrue(bitMask.GetFlag(testValue), $"{nameof(BlitMaskExtensions.GetFlag)} failed unit test.");
        }

        [TestMethod]
        public void HasAllFlags()
        {
            int[] testValues = { 0, 1 };

            BitMask bitMask = new BitMask(testValues);

            uint expectedValue = 3; // first bit signifies 0 or 1, second bit signifies 0 or 2, 1+2=3

            /// This presumes that cast from <see cref="BitMask"/> to <see cref="uint"/> functions correctly
            Assert.IsTrue(bitMask.HasAllFlags(expectedValue), $"{nameof(BlitMaskExtensions.GetFlag)} failed unit test.");
        }

        [TestMethod]
        public void HasAnyFlag()
        {
            BitMask bitMask = new BitMask(0, 1, 3);
            BitMask anyFlag = new BitMask(1);

            /// This presumes that cast from <see cref="BitMask"/> to <see cref="uint"/> functions correctly
            Assert.IsTrue(bitMask.HasAnyFlag(anyFlag), $"{nameof(BlitMaskExtensions.GetFlag)} failed unit test.");
        }

        [TestMethod]
        public void SetFlag()
        {
            BitMask bitMask = new BitMask();

            int testValue = 2;
            int testValuePow2 = testValue * testValue;

            bitMask.SetFlag(testValue);

            /// This presumes that cast from <see cref="BitMask"/> to <see cref="uint"/> functions correctly
            Assert.AreEqual((uint)testValuePow2, (uint)bitMask, $"{nameof(BlitMaskExtensions.SetFlag)} failed unit test.");
        }

        [TestMethod]
        public void SetFlags()
        {
            BitMask bitMask = new BitMask();


            bitMask.SetFlags(0, 1);

            int expectedValue = 3; // first bit signifies 0 or 1, second bit signifies 0 or 2, 1+2=3

            /// This presumes that cast from <see cref="BitMask"/> to <see cref="uint"/> functions correctly
            Assert.AreEqual((uint)expectedValue, (uint)bitMask, $"{nameof(BlitMaskExtensions.SetFlag)} failed unit test.");
        }

        [TestMethod]
        public void ClearFlag()
        {
            BitMask bitMask = new BitMask(1);

            bitMask.ClearFlags(1);

            int expectedValue = 0;

            /// This presumes that cast from <see cref="BitMask"/> to <see cref="uint"/> functions correctly
            Assert.AreEqual((uint)expectedValue, (uint)bitMask, $"{nameof(BlitMaskExtensions.SetFlag)} failed unit test.");
        }

        [TestMethod]
        public void ClearFlags()
        {
            BitMask bitMask = new BitMask(0, 1, 3);

            bitMask.ClearFlags(0, 1, 3);

            int expectedValue = 0;

            /// This presumes that cast from <see cref="BitMask"/> to <see cref="uint"/> functions correctly
            Assert.AreEqual((uint)expectedValue, (uint)bitMask, $"{nameof(BlitMaskExtensions.SetFlag)} failed unit test.");
        }

        [TestMethod]
        public void ToggleFlag()
        {
            BitMask bitMask = new BitMask(0, 1, 2);

            bitMask.ToggleFlag(1);

            int expectedValue = 5;

            /// This presumes that cast from <see cref="BitMask"/> to <see cref="uint"/> functions correctly
            Assert.AreEqual((uint)expectedValue, (uint)bitMask, $"{nameof(BlitMaskExtensions.SetFlag)} failed unit test.");
        }

        [TestMethod]
        public void ToggleFlags()
        {
            BitMask bitMask = new BitMask(0, 1, 5);

            bitMask.ToggleFlags(0, 1);

            int expected = 32;

            /// This presumes that cast from <see cref="BitMask"/> to <see cref="uint"/> functions correctly
            Assert.AreEqual((uint)expected, (uint)bitMask, $"{nameof(BlitMaskExtensions.SetFlag)} failed unit test.");
        }

        [TestMethod]
        public void SetAllFlags()
        {
            BitMask bitMask = new BitMask(0, 1, 5);

            bitMask.SetAllFlags(true);

            int expected1 = ~0;

            /// This presumes that cast from <see cref="BitMask"/> to <see cref="uint"/> functions correctly
            Assert.AreEqual((uint)expected1, (uint)bitMask, $"{nameof(BlitMaskExtensions.SetFlag)} failed unit test.");

            bitMask.SetAllFlags(false);

            int expected2 = 0;

            /// This presumes that cast from <see cref="BitMask"/> to <see cref="uint"/> functions correctly
            Assert.AreEqual((uint)expected2, (uint)bitMask, $"{nameof(BlitMaskExtensions.SetFlag)} failed unit test.");
        }

        [TestMethod]
        public void IsBlittable()
        {
            Assert.IsTrue(BlitMaskUnitTestsHelpers.IsBlittableType(typeof(BitMask)));
        }
    }
}
