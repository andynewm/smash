using System;
using Moq;
using NUnit.Framework;
using System.Runtime.Caching;

namespace Smash.Tests
{
    [TestFixture]
    public class ProxyTests
    {
        [SetUp]
        public void SetUp()
        {
            // Make sure the cache used is empty

            foreach (var element in MemoryCache.Default)
            {
                MemoryCache.Default.Remove(element.Key);
            }
        }

        [Test]
        public void TestRepeatedCalls()
        {
            var mockTest = new Mock<ITest>();

            var proxy = mockTest.Object.Smash();

            mockTest
                .Setup(x => x.Double(10))
                .Returns(20);

            var firstResult = proxy.Double(10);
            var secondResult = proxy.Double(10);

            Assert.That(firstResult,
                Is.EqualTo(20).And.EqualTo(secondResult));

            mockTest.Verify(x => x.Double(10), Times.Once,
                "Actual implementation should only be called once.");
        }

        [Test]
        public void TestRepeatedCallsWithMultipleArguments()
        {
            var mockTest = new Mock<ITest>();

            var proxy = mockTest.Object.Smash();

            mockTest
                .Setup(x => x.Add(1, 2))
                .Returns(3);

            var firstResult = proxy.Add(1, 2);
            var secondResult = proxy.Add(1, 2);

            Assert.That(firstResult,
                Is.EqualTo(3).And.EqualTo(secondResult));

            mockTest.Verify(x => x.Add(1, 2), Times.Once,
                "Actual implementation should only be called once.");
        }

        [Test]
        public void TestDifferentArgumentsAreNotCachedTogether()
        {
            var mockTest = new Mock<ITest>();

            var proxy = mockTest.Object.Smash();

            mockTest
                .Setup(x => x.Double(10))
                .Returns(20);

            mockTest
                .Setup(x => x.Double(20))
                .Returns(40);

            var firstResult = proxy.Double(10);
            var secondResult = proxy.Double(20);

            Assert.That(firstResult, Is.EqualTo(20));
            Assert.That(secondResult, Is.EqualTo(40));
        }

        [Test]
        public void TestDifferentMethodsAreNotCachedTogether()
        {
            var mockTest = new Mock<ITest>();

            var proxy = mockTest.Object.Smash();

            mockTest
                .Setup(x => x.Double(10))
                .Returns(20);

            mockTest
                .Setup(x => x.Triple(10))
                .Returns(30);

            var firstResult = proxy.Double(10);
            var secondResult = proxy.Triple(10);

            Assert.That(firstResult, Is.EqualTo(20));
            Assert.That(secondResult, Is.EqualTo(30));
        }
    }

    public interface ITest
    {
        int Double(int n);
        int Triple(int n);
        int Add(int a, int b);
    }
}