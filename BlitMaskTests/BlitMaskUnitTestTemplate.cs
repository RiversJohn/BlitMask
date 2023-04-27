using BlitMask;

namespace BlitMaskTests
{
    [TestClass]
    public class BlitMaskUnitTestTemplate
    {
        [TestMethod]
        public void HasFlag()
        {
            int testValue = 2;

            BlitMask64 BlitMask64 = new BlitMask64(testValue);

            int testValuePow2 = testValue * testValue;

            /// This presumes that cast from <see cref="BlitMask64"/> to <see cref="uint"/> functions correctly
            Assert.IsTrue(BlitMask64.HasFlag(testValue), $"{nameof(BlitMaskExtensions64.HasFlag)} failed unit test.");
        }

        [TestMethod]
        public void HasAllFlags()
        {
            int[] testValues = { 0, 1 };

            BlitMask64 BlitMask64 = new BlitMask64(testValues);

            uint expectedValue = 3; // first bit signifies 0 or 1, second bit signifies 0 or 2, 1+2=3

            /// This presumes that cast from <see cref="BlitMask64"/> to <see cref="uint"/> functions correctly
            Assert.IsTrue(BlitMask64.HasAllFlags(expectedValue), $"{nameof(BlitMaskExtensions64.HasFlag)} failed unit test.");
        }

        [TestMethod]
        public void HasAnyFlag()
        {
            BlitMask64 BlitMask64 = new BlitMask64(0, 1, 3);
            BlitMask64 anyFlag = new BlitMask64(1);
            BlitMask64 incorrectFlag = new BlitMask64(2);

            /// This presumes that cast from <see cref="BlitMask64"/> to <see cref="uint"/> functions correctly
            Assert.IsTrue(BlitMask64.HasAnyFlag(anyFlag) && !BlitMask64.HasAnyFlag(incorrectFlag), $"{nameof(BlitMaskExtensions64.HasFlag)} failed unit test.");
        }

        [TestMethod]
        public void SetFlag()
        {
            BlitMask64 BlitMask64 = new BlitMask64();

            int testValue = 2;
            int testValuePow2 = testValue * testValue;

            BlitMask64.SetFlag(testValue);

            /// This presumes that cast from <see cref="BlitMask64"/> to <see cref="uint"/> functions correctly
            Assert.AreEqual((uint)testValuePow2, (uint)BlitMask64, $"{nameof(BlitMaskExtensions64.SetFlag)} failed unit test.");
        }

        [TestMethod]
        public void SetFlags()
        {
            BlitMask64 BlitMask64 = new BlitMask64();


            BlitMask64.SetFlags(0, 1);

            int expectedValue = 3; // first bit signifies 0 or 1, second bit signifies 0 or 2, 1+2=3

            /// This presumes that cast from <see cref="BlitMask64"/> to <see cref="uint"/> functions correctly
            Assert.AreEqual((uint)expectedValue, (uint)BlitMask64, $"{nameof(BlitMaskExtensions64.SetFlag)} failed unit test.");
        }

        [TestMethod]
        public void ClearFlag()
        {
            BlitMask64 BlitMask64 = new BlitMask64(1);

            BlitMask64.ClearFlags(1);

            int expectedValue = 0;

            /// This presumes that cast from <see cref="BlitMask64"/> to <see cref="uint"/> functions correctly
            Assert.AreEqual((uint)expectedValue, (uint)BlitMask64, $"{nameof(BlitMaskExtensions64.SetFlag)} failed unit test.");
        }

        [TestMethod]
        public void ClearFlags()
        {
            BlitMask64 BlitMask64 = new BlitMask64(0, 1, 3);

            BlitMask64.ClearFlags(0, 1, 3);

            int expectedValue = 0;

            /// This presumes that cast from <see cref="BlitMask64"/> to <see cref="uint"/> functions correctly
            Assert.AreEqual((uint)expectedValue, (uint)BlitMask64, $"{nameof(BlitMaskExtensions64.SetFlag)} failed unit test.");
        }

        [TestMethod]
        public void ToggleFlag()
        {
            BlitMask64 BlitMask64 = new BlitMask64(0, 1, 2);

            BlitMask64.ToggleFlag(1);

            int expectedValue = 5;

            /// This presumes that cast from <see cref="BlitMask64"/> to <see cref="uint"/> functions correctly
            Assert.AreEqual((uint)expectedValue, (uint)BlitMask64, $"{nameof(BlitMaskExtensions64.SetFlag)} failed unit test.");
        }

        [TestMethod]
        public void ToggleFlags()
        {
            BlitMask64 BlitMask64 = new BlitMask64(0, 1, 5);

            BlitMask64.ToggleFlags(0, 1);

            int expected = 32;

            /// This presumes that cast from <see cref="BlitMask64"/> to <see cref="uint"/> functions correctly
            Assert.AreEqual((uint)expected, (uint)BlitMask64, $"{nameof(BlitMaskExtensions64.SetFlag)} failed unit test.");
        }

        [TestMethod]
        public void SetAllFlags()
        {
            BlitMask64 BlitMask64 = new BlitMask64(0, 1, 5);

            BlitMask64.SetAllFlags(true);

            int expected1 = ~0;

            /// This presumes that cast from <see cref="BlitMask64"/> to <see cref="uint"/> functions correctly
            Assert.AreEqual((uint)expected1, (uint)BlitMask64, $"{nameof(BlitMaskExtensions64.SetFlag)} failed unit test.");

            BlitMask64.SetAllFlags(false);

            int expected2 = 0;

            /// This presumes that cast from <see cref="BlitMask64"/> to <see cref="uint"/> functions correctly
            Assert.AreEqual((uint)expected2, (uint)BlitMask64, $"{nameof(BlitMaskExtensions64.SetFlag)} failed unit test.");
        }

        [TestMethod]
        public void IsBlittable()
        {
            Assert.IsTrue(BlittableTypeTest.IsBlittableType(typeof(BlitMask64)));
        }
    }
}
