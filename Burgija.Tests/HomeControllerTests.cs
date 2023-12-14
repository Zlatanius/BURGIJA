using Burgija.Controllers;
using Burgija.Models;
using CsvHelper;
using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Moq;
using Burgija.Data;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.IO;
using System.Xml.Linq;
using Humanizer;

namespace Burgija.Tests
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }

        public DbSet<ToolType> ToolType { get; set; } // Add DbSet for ToolType (assuming ToolType is your model entity)
                                                      // Other DbSet properties as needed for testing other entities
    }

    [TestClass]
    public class HomeControllerTests
    {
        private HomeController controller;
        private Mock<IApplicationDbContext> mockDbContext;
        private ApplicationDbContext mockContext;


        [TestInitialize]
        public void SetUp()
        {
            controller = new HomeController(null, null);
            mockDbContext = new Mock<IApplicationDbContext>();
        }

        static IEnumerable<object[]> toolTypes
        {
            get
            {
                return UcitajPodatkeCSV("alati.csv");
            }
        }

        [TestMethod]
        public void Test_MergeSort()
        {
            var before = ConvertToToolTypes(toolTypes);
            var check = before;
            before = HomeController.MergeSort(before);
            check.Sort((tool1, tool2) => tool1.Name.CompareTo(tool2.Name));
            CollectionAssert.AreEqual(check, before);
        }
        [TestMethod]
        public void Test_QuickSort()
        {
            var before = ConvertToToolTypes(toolTypes);
            var check = before;
            HomeController.QuickSort(before);
            check.Sort((tool1, tool2) => tool2.Price.CompareTo(tool1.Price));
            CollectionAssert.AreEqual(check, before);
        }
        [TestMethod]
        public void Test_SelectionSortDescending()
        {
            var before = ConvertToToolTypes(toolTypes);
            var check = before;
            HomeController.SelectionSortDescending(before);
            check.Sort((tool1, tool2) => tool2.Price.CompareTo(tool1.Price));
            CollectionAssert.AreEqual(check, before);
        }
        [TestMethod]
        public async Task Test_Index_returnAllAlati()
        {

            // Create a mock DbSet using the MockDbSet class
            var mockSet = MockDbSet.Create(ConvertToToolTypes(toolTypes));

            // Setup mock DbContext
            var mockDbContext = new Mock<IApplicationDbContext>();
            mockDbContext.Setup(c => c.ToolTypes).Returns(mockSet.Object);

            var mockUserManager = new Mock<UserManager<IdentityUser<int>>>(
             new Mock<IUserStore<IdentityUser<int>>>().Object,
              null, null, null, null, null, null, null, null);

            controller = new HomeController(mockDbContext.Object, mockUserManager.Object);

            // Act
            var result = await controller.Index(null, null, null, null) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            // Assert.IsType<List<ToolType>>(result.Model);
            Assert.AreEqual(4, (result.Model as List<ToolType>).Count);
        }
        [TestMethod]
        public async Task Test_Index_returnSearch()
        {

            // Create a mock DbSet using the MockDbSet class
            var mockSet = MockDbSet.Create(ConvertToToolTypes(toolTypes));

            // Setup mock DbContext
            var mockDbContext = new Mock<IApplicationDbContext>();
            mockDbContext.Setup(c => c.ToolTypes).Returns(mockSet.Object);

            // Create a mock mockUserManager using the Mock class
            var mockUserManager = new Mock<UserManager<IdentityUser<int>>>(
             new Mock<IUserStore<IdentityUser<int>>>().Object,
              null, null, null, null, null, null, null, null);

            controller = new HomeController(mockDbContext.Object, mockUserManager.Object);

            // Act
            var result = await controller.Index("Hammer", 5, 15, null) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            // Jedan je alat Hammer u listi
            Assert.AreEqual(1, (result.Model as List<ToolType>).Count);

            var check = result.Model as List<ToolType>;

            Assert.IsTrue(check[0].Price >= 5 && check[0].Price <= 15);
        }
        [TestMethod]
        public async Task Test_Index_returnPriceFilteredAlatiByLowestPrice()
        {

            // Create a mock DbSet using the MockDbSet class
            var mockSet = MockDbSet.Create(ConvertToToolTypes(toolTypes));

            // Setup mock DbContext
            var mockDbContext = new Mock<IApplicationDbContext>();
            mockDbContext.Setup(c => c.ToolTypes).Returns(mockSet.Object);

            // Create a mock mockUserManager using the Mock class
            var mockUserManager = new Mock<UserManager<IdentityUser<int>>>(
             new Mock<IUserStore<IdentityUser<int>>>().Object,
              null, null, null, null, null, null, null, null);

            controller = new HomeController(mockDbContext.Object, mockUserManager.Object);

            // Act
            var result = await controller.Index(null, 5, 10, "lowest price") as ViewResult;

            // Assert
            Assert.IsNotNull(result);

            // Dva alata imaju cijenu između 5-10
            Assert.AreEqual(2, (result.Model as List<ToolType>).Count);

            var check = result.Model as List<ToolType>;

            check.Sort((tool1, tool2) => tool2.Price.CompareTo(tool1.Price));

            CollectionAssert.AreEqual(check, result.Model as List<ToolType>);
        }

        [TestMethod]
        public async Task Test_Index_returnPriceFilteredAlatiByHighestPrice()
        {

            // Create a mock DbSet using the MockDbSet class
            var mockSet = MockDbSet.Create(ConvertToToolTypes(toolTypes));

            // Setup mock DbContext
            var mockDbContext = new Mock<IApplicationDbContext>();
            mockDbContext.Setup(c => c.ToolTypes).Returns(mockSet.Object);

            // Create a mock mockUserManager using the Mock class
            var mockUserManager = new Mock<UserManager<IdentityUser<int>>>(
             new Mock<IUserStore<IdentityUser<int>>>().Object,
              null, null, null, null, null, null, null, null);

            controller = new HomeController(mockDbContext.Object, mockUserManager.Object);

            // Act
            var result = await controller.Index(null, 12, 50, "highestPrice") as ViewResult;

            // Assert
            Assert.IsNotNull(result);

            // Dva alata imaju cijenu između 15-50
            Assert.AreEqual(2, (result.Model as List<ToolType>).Count);

            var check = result.Model as List<ToolType>;

            check.Sort((tool1, tool2) => tool1.Price.CompareTo(tool2.Price));

            CollectionAssert.AreEqual(check, result.Model as List<ToolType>);
        }

        [TestMethod]
        public async Task Test_Index_returnPriceFilteredAlatiByName()
        {

            // Create a mock DbSet using the MockDbSet class
            var mockSet = MockDbSet.Create(ConvertToToolTypes(toolTypes));

            // Setup mock DbContext
            var mockDbContext = new Mock<IApplicationDbContext>();
            mockDbContext.Setup(c => c.ToolTypes).Returns(mockSet.Object);

            // Create a mock mockUserManager using the Mock class
            var mockUserManager = new Mock<UserManager<IdentityUser<int>>>(
             new Mock<IUserStore<IdentityUser<int>>>().Object,
              null, null, null, null, null, null, null, null);

            controller = new HomeController(mockDbContext.Object, mockUserManager.Object);

            // Act
            var result = await controller.Index(null, 8, 15, "alphabetical") as ViewResult;

            // Assert
            Assert.IsNotNull(result);

            // Dva alata imaju cijenu između 8-15
            Assert.AreEqual(2, (result.Model as List<ToolType>).Count);

            var check = result.Model as List<ToolType>;

            check.Sort((tool1, tool2) => tool2.Name.CompareTo(tool1.Name));

            CollectionAssert.AreEqual(check, result.Model as List<ToolType>);
        }


        public static IEnumerable<object[]> UcitajPodatkeCSV(string path)
        {
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var rows = csv.GetRecords<dynamic>();
                foreach (var row in rows)
                {
                    var values = ((IDictionary<String, Object>)row).Values;
                    var elements = values.Select(elem => elem.ToString()).ToList();
                    yield return new object[] { elements[0], elements[1],elements[2],
                        elements[3],elements[4],elements[5] };

                }

            }
        }

        public List<ToolType> ConvertToToolTypes(IEnumerable<object[]> toolTypeData)
        {
            List<ToolType> toolTypes = new List<ToolType>();

            foreach (var row in toolTypeData)
            {
                // Assuming the columns in the IEnumerable<object[]> match the properties of ToolType
                var toolType = new ToolType
                {
                    Id = Convert.ToInt32(row[0]),
                    Name = row[1].ToString(),
                    Category = (Category)Enum.Parse(typeof(Category), row[2].ToString()),
                    Description = row[3].ToString(),
                    Price = Convert.ToDouble(row[4]),
                    Image = row[5].ToString()
                };

                toolTypes.Add(toolType);
            }

            return toolTypes;
        }

    }
}
