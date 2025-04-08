using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.Configuration;
using Moosic.Controllers;
using Moosic.Models;
using Moosic.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moosic.Tests
{
    public class AccountControllerTest
    {
        //SETUP HELPER FOR DATABASE
        private MoosicDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<MoosicDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new MoosicDbContext(options);
        }


        [Fact]
        //HTTP POST FOR SIGN UP TEST = SUCCESs
        public async Task Sign_Up_Post_ValidUser_ShouldRedirectToLogin()

        //DOCUMENTATION : https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/testing?view=aspnetcore-9.0
        {
            //arrange - Get environment ready 
            var context = GetInMemoryDbContext();
            var mockHttpClient = new Mock<HttpClient>();

            // Mock IConfiguration
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["Spotify:ClientId"]).Returns("fakeClientId");
            mockConfig.Setup(c => c["Spotify:ClientSecret"]).Returns("fakeClientSecret");

            // Mock SpotifyAuthService (passing the mocked HttpClient and IConfiguration)
            var mockAuthService = new SpotifyAuthService(mockHttpClient.Object, mockConfig.Object);

            // Mock ApiService (passing in the mocked SpotifyAuthService)
            var apiService = new ApiService(mockHttpClient.Object, mockAuthService);
            var controller = new AccountController(context, apiService);

            var user = new User
            {
                userName = "testUser",
                email = "test@testexample.com",
                password = "password"
            };

        

            // Act
            var result = await controller.SignUp(user);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Login", redirectResult.ActionName);

            
        }

        //Valid Login
        [Fact]
        public async Task Login_Post_ValidUser_RedirectToDashBoard()
        {
            //arrange - Get environment ready 
            var context = GetInMemoryDbContext();
            var mockHttpClient = new Mock<HttpClient>();

            // Mock IConfiguration
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["Spotify:ClientId"]).Returns("fakeClientId");
            mockConfig.Setup(c => c["Spotify:ClientSecret"]).Returns("fakeClientSecret");

            // Mock SpotifyAuthService (passing the mocked HttpClient and IConfiguration)
            var mockAuthService = new SpotifyAuthService(mockHttpClient.Object, mockConfig.Object);

            // Mock ApiService (passing in the mocked SpotifyAuthService)
            var apiService = new ApiService(mockHttpClient.Object, mockAuthService);
            var controller = new AccountController(context, apiService);

            var user = new User
            {
                userName = "testUser",
                email = "test@testexample.com",
                password = "password"
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.Login(user.email, user.password);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Dashboard", redirectResult.ActionName); // Should redirect to the Dashboard
        }

        //Invalid Login 
        [Fact]
        public async Task Login_Post_InValidUser_ERROR()
        {
            //arrange - Get environment ready 
            var context = GetInMemoryDbContext();
            var mockHttpClient = new Mock<HttpClient>();

            // Mock IConfiguration
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["Spotify:ClientId"]).Returns("fakeClientId");
            mockConfig.Setup(c => c["Spotify:ClientSecret"]).Returns("fakeClientSecret");

            // Mock SpotifyAuthService (passing the mocked HttpClient and IConfiguration)
            var mockAuthService = new SpotifyAuthService(mockHttpClient.Object, mockConfig.Object);

            // Mock ApiService (passing in the mocked SpotifyAuthService)
            var apiService = new ApiService(mockHttpClient.Object, mockAuthService);
            var controller = new AccountController(context, apiService);

    

            // Act
            var result = await controller.Login("invalid@example.com", "invalid");

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Invalid email or passowrd", controller.ViewData["Error"]); // Should redirect to the Dashboard
        }

        //GET Library
        [Fact]
    public async Task Library_Get_ValidUser_Library()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var mockHttpClient = new Mock<HttpClient>();

            // Mock IConfiguration
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["Spotify:ClientId"]).Returns("fakeClientId");
            mockConfig.Setup(c => c["Spotify:ClientSecret"]).Returns("fakeClientSecret");

            // Mock SpotifyAuthService (passing the mocked HttpClient and IConfiguration)
            var mockAuthService = new SpotifyAuthService(mockHttpClient.Object, mockConfig.Object);

            // Mock ApiService (passing in the mocked SpotifyAuthService)
            var apiService = new ApiService(mockHttpClient.Object, mockAuthService);
            var controller = new AccountController(context, apiService);

            // Create and add a mock user to the database
            var user = new User
            {
                userName = "testUser",
                email = "test@testexample.com",
                password = "password"
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Set the session for the logged-in user (simulate login)
            controller.HttpContext.Session.SetInt32("UserId", user.UserId);

            // Add mock library items for the user
            var libraryItems = new List<LibraryItem>
    {
        new LibraryItem { UserId = user.UserId, Music = new Music { Title = "Song 1" } },
        new LibraryItem { UserId = user.UserId, Music = new Music { Title = "Song 2" } }
    };
            context.LibraryItems.AddRange(libraryItems);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.Library();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<LibraryItem>>(viewResult.Model); // Check if the model is of the correct type
            Assert.Equal(2, model.Count); // Ensure there are 2 library items
        }
    }
}
