using Bunit;
using EmployeeManagement;
using EmployeeManagement.Pages;
using EmployeeManagementLibrary.DB;
using EmployeeManagementLibrary.Models;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace TestProject1
{
    public class LoginTests : TestContext
    {
        [Fact]
        public void LoginPage_RendersCorrectly()
        {
            var mockSessionService = new Mock<ISessionService>();
            var mockUserData = new Mock<IUserData>();

            Services.AddSingleton(mockSessionService.Object);
            Services.AddSingleton(mockUserData.Object);

            var cut = RenderComponent<Login>();

            Assert.NotNull(cut.Find("input[placeholder='Email']"));
            Assert.NotNull(cut.Find("input[placeholder='Password']"));
            Assert.NotNull(cut.Find("input[type='checkbox']"));
            Assert.NotNull(cut.Find("button.loginButton"));
        }



        [Fact]
        public void LoginPage_InvalidCredentials_ShowsErrorMessage()
        {
            var mockSessionService = new Mock<ISessionService>();
            var mockUserData = new Mock<IUserData>();

            mockUserData.Setup(u => u.CheckDetails("admin@test.com", "password")).ReturnsAsync(true);
            mockUserData.Setup(u => u.CheckDetails(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

            Services.AddSingleton(mockSessionService.Object);
            Services.AddSingleton(mockUserData.Object);

            var cut = RenderComponent<Login>();

            cut.Find("input[placeholder='Email']").Change("wrong@test.com");
            cut.Find("input[placeholder='Password']").Change("wrongPassword");

            cut.Find("button.loginButton").Click();

            var errorMessage = cut.Find("div.errMessage").TextContent;
            Assert.Equal("Invalid email or password", errorMessage);
        }

        [Fact]
        public void LoginPage_SuccessfulLogin_NavigatesToHomePage()
        {
            var mockSessionService = new Mock<ISessionService>();
            var mockUserData = new Mock<IUserData>();

            mockUserData.Setup(u => u.CheckDetails("admin@test.com", "password")).ReturnsAsync(true);
            mockUserData.Setup(u => u.IsAdministrator("valid@example.com")).ReturnsAsync(false);

            mockSessionService.Setup(s => s.SetCurrentUser(It.IsAny<string>(), It.IsAny<bool>()));

            Services.AddSingleton(mockSessionService.Object);
            Services.AddSingleton(mockUserData.Object);

            var cut = RenderComponent<Login>();

            cut.Find("input[placeholder='Email']").Change("admin@test.com");
            cut.Find("input[placeholder='Password']").Change("password");
            cut.Find("input[type='checkbox']").Change(true);

            cut.Find("button.loginButton").Click();

            mockSessionService.Verify(s => s.SetCurrentUser("admin@test.com", false), Times.Once);
            mockSessionService.Verify(s => s.NavigateTo("/"), Times.Once);
        }
        [Fact]
        public void LoginPage_EmptyEmail_ShowsErrorMessage()
        {
            var mockSessionService = new Mock<ISessionService>();
            var mockUserData = new Mock<IUserData>();

            Services.AddSingleton(mockSessionService.Object);
            Services.AddSingleton(mockUserData.Object);

            var cut = RenderComponent<Login>();

            cut.Find("input[placeholder='Password']").Change("password123");

            cut.Find("button.loginButton").Click();

            var errorMessage = cut.Find("div.errMessage").TextContent;
            Assert.Equal("Please enter your email", errorMessage);
        }
        [Fact]
        public void LoginPage_EmptyPassword_ShowsErrorMessage()
        {
            var mockSessionService = new Mock<ISessionService>();
            var mockUserData = new Mock<IUserData>();

            Services.AddSingleton(mockSessionService.Object);
            Services.AddSingleton(mockUserData.Object);

            var cut = RenderComponent<Login>();

            cut.Find("input[placeholder='Email']").Change("username");

            cut.Find("button.loginButton").Click();

            var errorMessage = cut.Find("div.errMessage").TextContent;
            Assert.Equal("Please enter your password", errorMessage);
        }
    }




    public class DepartmentTests : TestContext
    {
        [Fact]
        public void AddDepartmentPage_Administrator()
        {
            var mockSessionService = new Mock<ISessionService>();
            var mockDepartmentData = new Mock<IDepartmentData>();

            mockSessionService.Setup(u => u.GetCurrentUser()).ReturnsAsync("admin@test.com");
            mockSessionService.Setup(u => u.GetUserRole()).ReturnsAsync(true);

            Services.AddSingleton(mockSessionService.Object);
            Services.AddSingleton(mockDepartmentData.Object);

            var cut = RenderComponent<DepartmentCreate>();
            Assert.Contains("DepartmentCreate", cut.Markup);
            Assert.Contains("Department Name:", cut.Markup);


        }
        [Fact]
        public void AddDepartmentPage_NonAdministrator()
        {
            var mockSessionService = new Mock<ISessionService>();
            var mockDepartmentData = new Mock<IDepartmentData>();

            mockSessionService.Setup(u => u.GetCurrentUser()).ReturnsAsync("admin@test.com");
            mockSessionService.Setup(u => u.GetUserRole()).ReturnsAsync(false);

            Services.AddSingleton(mockSessionService.Object);
            Services.AddSingleton(mockDepartmentData.Object);

            var cut = RenderComponent<DepartmentCreate>();
            mockSessionService.Verify(s => s.NavigateTo("/"), Times.Once);

        }
        [Fact]
        public void AddDepartmentPage_NotLoggedIn()
        {
            var mockSessionService = new Mock<ISessionService>();
            var mockDepartmentData = new Mock<IDepartmentData>();

            mockSessionService.Setup(u => u.GetCurrentUser()).ReturnsAsync((string)null);


            Services.AddSingleton(mockSessionService.Object);
            Services.AddSingleton(mockDepartmentData.Object);

            var cut = RenderComponent<DepartmentCreate>();
            mockSessionService.Verify(s => s.NavigateTo("/Login"), Times.Once);
        }

        [Fact]
        public void DeleteDepartment()
        {
            var mockSessionService = new Mock<ISessionService>();
            var mockDepartmentData = new Mock<IDepartmentData>();
            var mockUserData = new Mock<IUserData>();

            mockSessionService.Setup(u => u.GetCurrentUser()).ReturnsAsync("admin@test.com");
            mockSessionService.Setup(u => u.GetUserRole()).ReturnsAsync(true);


            var departments = new List<DepartmentModel>
            {
                new DepartmentModel { DepartmentId = 1, DepartmentName = "IT" },
                new DepartmentModel { DepartmentId = 2, DepartmentName = "TEST" },

            };
            mockDepartmentData.Setup(d => d.Departments()).ReturnsAsync(departments);

            mockDepartmentData.Setup(d => d.RemoveDepartment(It.IsAny<int>())).Callback<int>(id=>departments.RemoveAll(dId=> dId.DepartmentId == id)).Returns(Task.CompletedTask);



            Services.AddSingleton(mockSessionService.Object);
            Services.AddSingleton(mockDepartmentData.Object);
            Services.AddSingleton(mockUserData.Object);

            var cut = RenderComponent<ViewDepartments>();


            cut.FindAll("span.deleteDepartmentButton")[0].Click();
            mockDepartmentData.Verify(d => d.RemoveDepartment(1), Times.Once);



            cut.Render();
            Assert.DoesNotContain("IT", cut.Markup);
            Assert.Contains("TEST", cut.Markup);


        }

    }









    public class EmployeeTests : TestContext
    {
        [Fact]
        public void AddEmployeePage_Administrator()
        {
            var mockSessionService = new Mock<ISessionService>();
            var mockEmployeeData = new Mock<IEmployeeData>();
            var mockDepartmentData = new Mock<IDepartmentData>();

            mockSessionService.Setup(u => u.GetCurrentUser()).ReturnsAsync("admin@test.com");
            mockSessionService.Setup(u => u.GetUserRole()).ReturnsAsync(true);


            var departments = new List<DepartmentModel>
            {
                new DepartmentModel { DepartmentId = 1, DepartmentName = "IT" },
                new DepartmentModel { DepartmentId = 2, DepartmentName = "TEST" },

            };
            mockDepartmentData.Setup(d => d.Departments()).ReturnsAsync(departments);



            Services.AddSingleton(mockSessionService.Object);
            Services.AddSingleton(mockEmployeeData.Object);
            Services.AddSingleton(mockDepartmentData.Object);


            var cut = RenderComponent<EmployeeCreate>();
            cut.Find("input[placeholder='FirstName']").Change("fn");
            cut.Find("input[placeholder='LastName']").Change("ln");
            cut.Find("select#selectDepartment").Change("IT");
            cut.Find("input[type='number']").Change(5500);
            cut.Find("button").Click();

            mockEmployeeData.Verify(e => e.AddEmployee(It.Is<EmployeeModel>(e => e.FirstName == "fn" && e.LastName == "ln" && e.Department == "IT" && e.Salary == 5500)), Times.Once);
        }




        [Fact]
        public void AddEmployeePage_NonAdministrator()
        {
            var mockSessionService = new Mock<ISessionService>();
            var mockEmployeeData = new Mock<IEmployeeData>();
            var mockDepartmentData = new Mock<IDepartmentData>();

            mockSessionService.Setup(u => u.GetCurrentUser()).ReturnsAsync("admin@test.com");
            mockSessionService.Setup(u => u.GetUserRole()).ReturnsAsync(false);

            Services.AddSingleton(mockSessionService.Object);
            Services.AddSingleton(mockEmployeeData.Object);
            Services.AddSingleton(mockDepartmentData.Object);

            var cut = RenderComponent<EmployeeCreate>();
            mockSessionService.Verify(s => s.NavigateTo("/"), Times.Once);

        }
        [Fact]
        public void AddEmployeePage_NotLoggedIn()
        {
            var mockSessionService = new Mock<ISessionService>();
            var mockEmployeeData = new Mock<IEmployeeData>();
            var mockDepartmentData = new Mock<IDepartmentData>();

            mockSessionService.Setup(u => u.GetCurrentUser()).ReturnsAsync((string)null);


            Services.AddSingleton(mockSessionService.Object);
            Services.AddSingleton(mockEmployeeData.Object);
            Services.AddSingleton(mockDepartmentData.Object);

            var cut = RenderComponent<EmployeeCreate>();
            mockSessionService.Verify(s => s.NavigateTo("/Login"), Times.Once);
        }
        [Fact]
        public void AddEmployeePage_ShowErrorMessage()
        {
            var mockSessionService = new Mock<ISessionService>();
            var mockEmployeeData = new Mock<IEmployeeData>();
            var mockDepartmentData = new Mock<IDepartmentData>();

            mockSessionService.Setup(u => u.GetCurrentUser()).ReturnsAsync("admin@test.com");
            mockSessionService.Setup(u => u.GetUserRole()).ReturnsAsync(true);

            Services.AddSingleton(mockSessionService.Object);
            Services.AddSingleton(mockEmployeeData.Object);
            Services.AddSingleton(mockDepartmentData.Object);

            var cut = RenderComponent<EmployeeCreate>();

            cut.Find("button.addEmployeeButton").Click();

            var errorMessage = cut.Find("div.employeeErrMessage").TextContent;
            Assert.Equal("Please enter a first name", errorMessage);
        }

    }


}