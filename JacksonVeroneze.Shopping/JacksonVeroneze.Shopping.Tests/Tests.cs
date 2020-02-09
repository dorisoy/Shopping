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
        public void TestOpenFilterByCategory()
        {
            app.Tap(x => x.Marked("ToolbarItemFilterByCategoryId"));

            Assert.IsTrue(app.Query(x => x.Text("Escolha a categoria para filtrar")).Any());
        }

        [Test]
        public void TestMakePurchaseAndNavigateToPaymentScreen()
        {
            app.EnterText(x => x.Marked("SearchBarSearchId"), "Galaxy");

            app.Tap(x => x.Marked("NoResourceEntry-22"));

            app.Tap(x => x.Marked("ButtonBuyId"));

            app.Tap(x => x.Marked("ButtonCheckoutId"));

            app.Query(x => x.Text("Checkout"));
        }


        [Test]
        public void BuyButtonShouldRemainDisabledAfterSearching()
        {
            app.EnterText("SearchBarSearchId", "Galaxy");

            bool isReadOnly = app.Query(c => c.Id("ButtonBuyId").Property("IsEnable").Value<bool>()).FirstOrDefault();

            Assert.AreEqual(true, isReadOnly);
        }
    }
}