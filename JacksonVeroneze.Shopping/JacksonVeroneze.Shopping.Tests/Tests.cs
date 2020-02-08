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

        static readonly Func<AppQuery, AppQuery> Button = c => c.Marked("ButtonBuyId").Text("Hello, Xamarin.Forms!");
        static readonly Func<AppQuery, AppQuery> SearchBar = c => c.Marked("SearchBarSearchId");
        static readonly Func<AppQuery, AppQuery> ListView = c => c.Marked("ListViewListDataId").Text("Was clicked");

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
            app.EnterText(x => x.Marked("SearchBarSearchId"), "Galaxy");
            app.Tap(x => x.Marked("ButtonBuyId"));

            int total = app.Query(x => x.Id("ListViewListDataId").Child()).Length;

            Assert.IsNotNull(total);


            // app.Repl();
            //app.EnterText("SearchBarSearchId", "Galaxy");

            //int total = app.Query(x => x.Id("ListViewListDataId").Child()).Length;

            //Assert.IsNotNull(total);
        }

        [Test]
        public void BuyButtonShouldRemainDisabledAfterSearching()
        {
            app.EnterText("SearchBarSearchId", "Galaxy");

            bool isReadOnly = app.Query(c => c.Id("ButtonBuyId").Property("IsEnable").Value<bool>()).FirstOrDefault();

            Assert.AreEqual(true, isReadOnly);
        }

        [Test]
        public void WelcomeImageButtonIncrementQuantityId()
        {
            //app.Query(x => x.Marked("SearchBarSearchId")).First().Text

            app.Repl();
            //app.EnterText("SearchBarSearchId", "Galaxy A5 2016");

            //app.ScrollDown();
            //app.PressEnter();

            //app.Tap(x => x.Marked("ImageButtonIncrementQuantityId"));
            //app.Tap(x => x.Marked("ImageButtonIncrementQuantityId"));

            //app.ScrollDown();
            //app.PressEnter();

            //Thread.Sleep(200);

            //bool isReadOnly = app.Query(c => c.Id("ButtonBuyId").Property("IsEnable").Value<bool>()).FirstOrDefault();

            //Assert.AreEqual(true, isReadOnly);
        }
    }
}