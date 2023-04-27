using BlitMask;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlitMaskTests
{
    [TestClass]
    public class BlitMaskUnitTestTemplate
    {
        [TestMethod]
        public void HasFlag()
        {
            int testValue = 2;

            BlitMaskTemplate BlitMaskTemplate = new BlitMaskTemplate(testValue);

            int testValuePow2 = testValue * testValue;

            /// This presumes that cast from <see cref="BlitMaskTemplate"/> to <see cref="uint"/> functions correctly
            Assert.IsTrue(BlitMaskTemplate.HasFlag(testValue), $"{nameof(BlitMaskExtensionsTemplate.HasFlag)} failed unit test.");
        }

        [TestMethod]
        public void HasAllFlags()
        {
            int[] testValues = { 0, 1 };

            BlitMaskTemplate BlitMaskTemplate = new BlitMaskTemplate(testValues);

            uint expectedValue = 3; // first bit signifies 0 or 1, second bit signifies 0 or 2, 1+2=3

            /// This presumes that cast from <see cref="BlitMaskTemplate"/> to <see cref="uint"/> functions correctly
            Assert.IsTrue(BlitMaskTemplate.HasAllFlags(expectedValue), $"{nameof(BlitMaskExtensionsTemplate.HasFlag)} failed unit test.");
        }

        [TestMethod]
        public void HasAnyFlag()
        {
            BlitMaskTemplate BlitMaskTemplate = new BlitMaskTemplate(0, 1, 3);
            BlitMaskTemplate anyFlag = new BlitMaskTemplate(1);
            BlitMaskTemplate incorrectFlag = new BlitMaskTemplate(2);

            /// This presumes that cast from <see cref="BlitMaskTemplate"/> to <see cref="uint"/> functions correctly
            Assert.IsTrue(BlitMaskTemplate.HasAnyFlag(anyFlag) && !BlitMaskTemplate.HasAnyFlag(incorrectFlag), $"{nameof(BlitMaskExtensionsTemplate.HasFlag)} failed unit test.");
        }

        [TestMethod]
        public void SetFlag()
        {
            BlitMaskTemplate BlitMaskTemplate = new BlitMaskTemplate();

            int testValue = 2;
            int testValuePow2 = testValue * testValue;

            BlitMaskTemplate.SetFlag(testValue);

            /// This presumes that cast from <see cref="BlitMaskTemplate"/> to <see cref="uint"/> functions correctly
            Assert.AreEqual((uint)testValuePow2, (uint)BlitMaskTemplate, $"{nameof(BlitMaskExtensionsTemplate.SetFlag)} failed unit test.");
        }

        [TestMethod]
        public void SetFlags()
        {
            BlitMaskTemplate BlitMaskTemplate = new BlitMaskTemplate();


            BlitMaskTemplate.SetFlags(0, 1);

            int expectedValue = 3; // first bit signifies 0 or 1, second bit signifies 0 or 2, 1+2=3

            /// This presumes that cast from <see cref="BlitMaskTemplate"/> to <see cref="uint"/> functions correctly
            Assert.AreEqual((uint)expectedValue, (uint)BlitMaskTemplate, $"{nameof(BlitMaskExtensionsTemplate.SetFlag)} failed unit test.");
        }

        [TestMethod]
        public void ClearFlag()
        {
            BlitMaskTemplate BlitMaskTemplate = new BlitMaskTemplate(1);

            BlitMaskTemplate.ClearFlags(1);

            int expectedValue = 0;

            /// This presumes that cast from <see cref="BlitMaskTemplate"/> to <see cref="uint"/> functions correctly
            Assert.AreEqual((uint)expectedValue, (uint)BlitMaskTemplate, $"{nameof(BlitMaskExtensionsTemplate.SetFlag)} failed unit test.");
        }

        [TestMethod]
        public void ClearFlags()
        {
            BlitMaskTemplate BlitMaskTemplate = new BlitMaskTemplate(0, 1, 3);

            BlitMaskTemplate.ClearFlags(0, 1, 3);

            int expectedValue = 0;

            /// This presumes that cast from <see cref="BlitMaskTemplate"/> to <see cref="uint"/> functions correctly
            Assert.AreEqual((uint)expectedValue, (uint)BlitMaskTemplate, $"{nameof(BlitMaskExtensionsTemplate.SetFlag)} failed unit test.");
        }

        [TestMethod]
        public void ToggleFlag()
        {
            BlitMaskTemplate BlitMaskTemplate = new BlitMaskTemplate(0, 1, 2);

            BlitMaskTemplate.ToggleFlag(1);

            int expectedValue = 5;

            /// This presumes that cast from <see cref="BlitMaskTemplate"/> to <see cref="uint"/> functions correctly
            Assert.AreEqual((uint)expectedValue, (uint)BlitMaskTemplate, $"{nameof(BlitMaskExtensionsTemplate.SetFlag)} failed unit test.");
        }

        [TestMethod]
        public void ToggleFlags()
        {
            BlitMaskTemplate BlitMaskTemplate = new BlitMaskTemplate(0, 1, 5);

            BlitMaskTemplate.ToggleFlags(0, 1);

            int expected = 32;

            /// This presumes that cast from <see cref="BlitMaskTemplate"/> to <see cref="uint"/> functions correctly
            Assert.AreEqual((uint)expected, (uint)BlitMaskTemplate, $"{nameof(BlitMaskExtensionsTemplate.SetFlag)} failed unit test.");
        }

        [TestMethod]
        public void SetAllFlags()
        {
            BlitMaskTemplate BlitMaskTemplate = new BlitMaskTemplate(0, 1, 5);

            BlitMaskTemplate.SetAllFlags(true);

            int expected1 = ~0;

            /// This presumes that cast from <see cref="BlitMaskTemplate"/> to <see cref="uint"/> functions correctly
            Assert.AreEqual((uint)expected1, (uint)BlitMaskTemplate, $"{nameof(BlitMaskExtensionsTemplate.SetFlag)} failed unit test.");

            BlitMaskTemplate.SetAllFlags(false);

            int expected2 = 0;

            /// This presumes that cast from <see cref="BlitMaskTemplate"/> to <see cref="uint"/> functions correctly
            Assert.AreEqual((uint)expected2, (uint)BlitMaskTemplate, $"{nameof(BlitMaskExtensionsTemplate.SetFlag)} failed unit test.");
        }

        [TestMethod]
        public void IsBlittable()
        {
            Assert.IsTrue(BlittableTypeTest.IsBlittableType(typeof(BlitMaskTemplate)));
        }
    }
}
