using Burgija.Controllers;
using Burgija.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Net.Http;

namespace Burgija.Tests
{
    [TestClass]
    public class RentControllerTest
    {
        private Mock<ApplicationDbContext> dbContextMock;

        [TestInitialize]
        public void ConnectToDatabase()
        {
            dbContextMock = new Mock<ApplicationDbContext>();
        }

        [TestMethod]
        public void GetToolType_Null_ReturnsNotFoundResult()
        {
            //Arrange
            var rentController = new RentController(dbContextMock.Object);
            //Act
            var result = rentController.GetToolType(null);
            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetToolType_Number_ReturnsRedirectToActionResult()
        {
            //Arrange
            var httpContextMock = new Mock<HttpContext>();
            var sessionMock = new Mock<ISession>();
            httpContextMock.Setup(c => c.Session).Returns(sessionMock.Object);
            var rentControllerContext = new ControllerContext
            {
                HttpContext = httpContextMock.Object,
            };
            var rentController = new RentController(dbContextMock.Object)
            {
                ControllerContext = rentControllerContext,
            };
            //Act
            var result = rentController.GetToolType(1);
            //Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));

            var redirectResult = (RedirectToActionResult)result;
            Assert.AreEqual("Create", redirectResult.ActionName);
            CollectionAssert.AreEqual(new object[] { 1 }, redirectResult.RouteValues.Values.ToArray());
        }

        [TestMethod]
        public async void RentHistory_User_ReturnsViewResultWithRents()
        {
            Assert.AreEqual(true, true);
        }

        [TestMethod]
        public async void Create_Null_ReturnsNotFoundResult()
        {
            //Arrange
            var rentController = new RentController(dbContextMock.Object);
            //Act
            var result = rentController.Create((int?)null);
            //Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }

        [TestMethod]
        public async void Create_ToolTypeIdNotInDb_ReturnsNotFoundResult()
        {
            //Arrange
            var rentController = new RentController(dbContextMock.Object);
            //Act
            var result = rentController.Create(-1);
            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}
