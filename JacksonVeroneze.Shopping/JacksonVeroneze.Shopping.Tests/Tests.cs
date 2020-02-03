using System;
using System.IO;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace JacksonVeroneze.Shopping.Tests
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class Tests
    {
        IApp app;
        Platform platform;

        public Tests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }

        [Test]
        public void WelcomeTextIsDisplayed()
        {
            app.EnterText("SearchBarSearchId", "Galaxy");

            int total = app.Query(x => x.Id("ListViewListDataId").Child()).Length;

            Assert.IsNotNull(total);
        }

        [Test]
        public void WelcomeTextIsDisplayed1()
        {
            app.EnterText("SearchBarSearchId", "Galaxy");

            bool isReadOnly = app.Query(c => c.Id("ButtonBuyId").Property("IsEnable").Value<bool>()).FirstOrDefault();

            Assert.AreEqual(false, isReadOnly);
        }

        [Test]
        public void WelcomeImageButtonIncrementQuantityId()
        {
            app.EnterText("SearchBarSearchId", "Galaxy A5 2016");

            app.ScrollDown();
            app.PressEnter();

            app.Tap(x => x.Marked("ImageButtonIncrementQuantityId"));
            app.Tap(x => x.Marked("ImageButtonIncrementQuantityId"));

            app.ScrollDown();
            app.PressEnter();

            Thread.Sleep(200);

            bool isReadOnly = app.Query(c => c.Id("ButtonBuyId").Property("IsEnable").Value<bool>()).FirstOrDefault();

            Assert.AreEqual(true, isReadOnly);
        }
    }
}