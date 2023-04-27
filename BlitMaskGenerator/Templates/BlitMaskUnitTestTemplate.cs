using BlitMask;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.InteropServices;

namespace BlitMaskTests
{
    [TestClass]
    public class BlitMaskUnitTestTemplate
    {
        private static readonly int bitSize = Marshal.SizeOf(BlitMaskTemplate.GetUnderlyingType) * 8;

        [TestMethod]
        public void HasFlag()
        {
            int correctFlag = 2;
            int incorrectFlag = 4;

            BlitMaskTemplate testMask = new BlitMaskTemplate(correctFlag);

            /// This presumes that cast from <see cref="BlitMaskTemplate"/> to <see cref="uint"/> functions correctly
            Assert.IsTrue(testMask.HasFlag(correctFlag) is true && testMask.HasFlag(incorrectFlag) is false, $"{nameof(BlitMaskExtensionsTemplate.HasFlag)} failed unit test.");
        }

        [TestMethod]
        public void HasAllFlags()
        {
            int[] testValues = { 0, 3, bitSize };

            BlitMaskTemplate testMask = new BlitMaskTemplate(testValues);

            uint testAgainst = BlitMaskConstantsTemplate.Zero;
            testAgainst |= BlitMaskConstantsTemplate.One << 0;
            testAgainst |= BlitMaskConstantsTemplate.One << 3;
            testAgainst |= BlitMaskConstantsTemplate.One << bitSize;

            /// This presumes that cast from <see cref="BlitMaskTemplate"/> to <see cref="uint"/> functions correctly
            Assert.IsTrue(testMask.HasAllFlags(testAgainst), $"{nameof(BlitMaskExtensionsTemplate.HasFlag)} failed unit test.");
        }

        [TestMethod]
        public void HasAnyFlag()
        {
            BlitMaskTemplate testMask = new BlitMaskTemplate(0, 1, 3);
            BlitMaskTemplate anyFlag = new BlitMaskTemplate(1);

            /// This presumes that cast from <see cref="BlitMaskTemplate"/> to <see cref="uint"/> functions correctly
            Assert.IsTrue(testMask.HasAnyFlag(anyFlag), $"{nameof(BlitMaskExtensionsTemplate.HasFlag)} failed unit test.");
        }

        [TestMethod]
        public void SetFlag()
        {
            BlitMaskTemplate testMask = new BlitMaskTemplate();

            testMask.SetFlag(bitSize);

            /// This presumes that cast from <see cref="BlitMaskTemplate"/> to <see cref="uint"/> functions correctly
            Assert.IsTrue(testMask.HasFlag(bitSize), $"{nameof(BlitMaskExtensionsTemplate.SetFlag)} failed unit test.");
        }

        [TestMethod]
        public void SetFlags()
        {
            BlitMaskTemplate testMask = new BlitMaskTemplate();

            testMask.SetFlags(0, 1);

            /// This presumes that cast from <see cref="BlitMaskTemplate"/> to <see cref="uint"/> functions correctly
            Assert.IsTrue(testMask.HasFlag(0) && testMask.HasFlag(1), $"{nameof(BlitMaskExtensionsTemplate.SetFlag)} failed unit test.");
        }

        [TestMethod]
        public void ClearFlag()
        {
            BlitMaskTemplate testMask = new BlitMaskTemplate(1);

            testMask.ClearFlags(1);

            int expectedValue = 0;

            /// This presumes that cast from <see cref="BlitMaskTemplate"/> to <see cref="uint"/> functions correctly
            Assert.AreEqual((uint)expectedValue, (uint)testMask, $"{nameof(BlitMaskExtensionsTemplate.SetFlag)} failed unit test.");
        }

        [TestMethod]
        public void ClearFlags()
        {
            BlitMaskTemplate testMask = new BlitMaskTemplate(0, 1, 3);

            testMask.ClearFlags(0, 1, 3);

            int expectedValue = 0;

            /// This presumes that cast from <see cref="BlitMaskTemplate"/> to <see cref="uint"/> functions correctly
            Assert.AreEqual((uint)expectedValue, (uint)testMask, $"{nameof(BlitMaskExtensionsTemplate.SetFlag)} failed unit test.");
        }

        [TestMethod]
        public void ToggleFlag()
        {
            BlitMaskTemplate testMask = new BlitMaskTemplate(0, 1, 2);

            testMask.ToggleFlag(1);

            int expectedValue = 5;

            /// This presumes that cast from <see cref="BlitMaskTemplate"/> to <see cref="uint"/> functions correctly
            Assert.AreEqual((uint)expectedValue, (uint)testMask, $"{nameof(BlitMaskExtensionsTemplate.SetFlag)} failed unit test.");
        }

        [TestMethod]
        public void ToggleFlags()
        {
            BlitMaskTemplate testMask = new BlitMaskTemplate(0, 1, 5);

            testMask.ToggleFlags(0, 1);

            int expected = 32;

            /// This presumes that cast from <see cref="BlitMaskTemplate"/> to <see cref="uint"/> functions correctly
            Assert.AreEqual((uint)expected, (uint)testMask, $"{nameof(BlitMaskExtensionsTemplate.SetFlag)} failed unit test.");
        }

        [TestMethod]
        public void SetAllFlags()
        {
            BlitMaskTemplate testMask = new BlitMaskTemplate(0, 1, 5);

            testMask.SetAllFlags(true);

            int expected1 = ~0;

            /// This presumes that cast from <see cref="BlitMaskTemplate"/> to <see cref="uint"/> functions correctly
            Assert.AreEqual((uint)expected1, (uint)testMask, $"{nameof(BlitMaskExtensionsTemplate.SetFlag)} failed unit test.");

            testMask.SetAllFlags(false);

            int expected2 = 0;

            /// This presumes that cast from <see cref="BlitMaskTemplate"/> to <see cref="uint"/> functions correctly
            Assert.AreEqual((uint)expected2, (uint)testMask, $"{nameof(BlitMaskExtensionsTemplate.SetFlag)} failed unit test.");
        }

        [TestMethod]
        public void IsBlittable()
        {
            Assert.IsTrue(BlittableTypeTest.IsBlittableType(typeof(BlitMaskTemplate)));
        }
    }
}
