using JacksonVeroneze.Shopping.Domain.Interface.Services;
using JacksonVeroneze.Shopping.Domain.Results;
using JacksonVeroneze.Shopping.Infra.CrossCutting.Network;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JacksonVeroneze.Shopping.Services.Tests
{
    [TestClass()]
    public class CategoryServiceTest
    {
        private IMock<MakeRequest> _makeRequest;
        private ICategoryService _categoryService;

        [TestInitialize()]
        public void Initialize()
        {
            _makeRequest = new Mock<MakeRequest>();
            _categoryService = new CategoryService(_makeRequest.Object);
        }

        [TestCleanup()]
        public void Cleanup()
        {
            _makeRequest = null;
            _categoryService = null;
        }

        [TestMethod()]
        public async Task ContainsDataInListTest()
        {
            IList<CategoryResult> listItens = await _categoryService.FindAllAsync();

            Assert.IsTrue(listItens.Any());
        }

        [TestMethod()]
        public async Task TotalOfElementsMustBeFiveTest()
        {
            IList<CategoryResult> listItens = await _categoryService.FindAllAsync();

            Assert.AreEqual(5, listItens.Count());
        }

        [TestMethod()]
        public async Task FirstIdMustContainValueOneTest()
        {
            IList<CategoryResult> listItens = await _categoryService.FindAllAsync();

            Assert.AreEqual(1, listItens.First().Id);
        }
        
        [TestMethod()]
        public async Task SecondElementMustHaveTheNameCellPhonesTest()
        {
            IList<CategoryResult> listItens = await _categoryService.FindAllAsync();

            CategoryResult categoryResult = listItens.ElementAt(1);

            Assert.AreEqual("Celulares", categoryResult.Name);
        }
    }
}
