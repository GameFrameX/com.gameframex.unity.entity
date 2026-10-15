using System;
using GameFrameX.Entity.Runtime;
using GameFrameX.Runtime;
using NUnit.Framework;

namespace GameFrameX.Entity.Tests
{
    internal class UnitTests
    {
        private DateTime dateTime, dateTime1;

        [SetUp]
        public void Setup()
        {
            dateTime = DateTime.Now;
            dateTime1 = DateTime.Now.AddHours(1);
        }

        [TearDown]
        public void TearDown()
        {
            GameFrameXOptions.Reset(); // 清除 Options 注册，避免跨用例静态泄漏
        }

        // Here is an example of a unit test for the IsUnixSameDay method
        [Test]
        public void TestIsUnixSameDay()
        {
            // Arrange
            // long timestamp1 = 1617842400; // April 7, 2021 12:00:00 AM UTC
            // long timestamp2 = 1617896400; // April 7, 2021 12:00:00 PM UTC

            // Act
        }


        [Test]
        public void Test1()
        {
            Assert.That(dateTime1.Year, Is.EqualTo(dateTime.Year));
            Assert.That(dateTime1.Month, Is.EqualTo(dateTime.Month));
            Assert.That(dateTime1.Day, Is.EqualTo(dateTime.Day));
        }

        [Test]
        public void EntityOptions_ConfigureAndTryGet_RoundTripsDiffs()
        {
            Func<EntityHelperBase> entityHelperFactory = () => null;
            Func<EntityGroupHelperBase> entityGroupHelperFactory = () => null;
            GameFrameXOptions.Reset();
            GameFrameXOptions.Configure(new EntityOptions { DefaultCapacity = 64, DefaultExpireTime = 120f, EnableShowEntityUpdateEvent = true, EnableShowEntityDependencyAssetEvent = true, EntityHelperFactory = entityHelperFactory, EntityGroupHelperFactory = entityGroupHelperFactory });

            Assert.IsTrue(GameFrameXOptions.TryGet<EntityOptions>(out var options));
            Assert.That(options.DefaultCapacity, Is.EqualTo(64));
            Assert.That(options.DefaultExpireTime, Is.EqualTo(120f));
            Assert.That(options.EnableShowEntityUpdateEvent, Is.True);
            Assert.That(options.EnableShowEntityDependencyAssetEvent, Is.True);
            Assert.That(options.EntityHelperFactory, Is.SameAs(entityHelperFactory));
            Assert.That(options.EntityGroupHelperFactory, Is.SameAs(entityGroupHelperFactory));

            GameFrameXOptions.Reset();
            var defaults = new EntityOptions();
            Assert.That(defaults.DefaultCapacity, Is.EqualTo(512));
            Assert.That(defaults.DefaultExpireTime, Is.EqualTo(3600f));
            Assert.That(defaults.EnableShowEntityUpdateEvent, Is.False);
            Assert.That(defaults.EnableShowEntityDependencyAssetEvent, Is.False);
            var entityHelper = defaults.EntityHelperFactory();
            var entityGroupHelper = defaults.EntityGroupHelperFactory();
            Assert.That(entityHelper, Is.InstanceOf<DefaultEntityHelper>());
            Assert.That(entityGroupHelper, Is.InstanceOf<DefaultEntityGroupHelper>());
            UnityEngine.Object.DestroyImmediate(entityHelper);
            UnityEngine.Object.DestroyImmediate(entityGroupHelper);
        }
    }
}