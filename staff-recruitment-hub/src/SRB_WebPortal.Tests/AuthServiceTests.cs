using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;

using SRB_ViewModel;
using SRB_ViewModel.Entities;

using SRB_WebPortal.Configs;
using SRB_WebPortal.Controllers.apis.auth;
using SRB_WebPortal.Models;
using SRB_WebPortal.Services;

public class AuthServiceTests
{
   private readonly Mock<IRedisService> _redisMock;
   private readonly Mock<IJwtService> _jwtMock;
   private readonly Mock<IHashingService> _hashingMock;
   private readonly Mock<IAuthRepository> _authRepoMock;
   private readonly Mock<IHttpContextAccessor> _httpMock;

   private readonly DatabaseContext _dbContext;
   private readonly IOptions<JwtOptions> _jwtOptions;

   private readonly TokenFactory _tokenFactory;

   private readonly IAuthService _authService;

   public AuthServiceTests()
   {
      // Mock Services
      _redisMock = new Mock<IRedisService>();
      _jwtMock = new Mock<IJwtService>();
      _hashingMock = new Mock<IHashingService>();
      _authRepoMock = new Mock<IAuthRepository>();
      _httpMock = new Mock<IHttpContextAccessor>();

      // InMemory DB
      var options = new DbContextOptionsBuilder<DatabaseContext>()
         .UseInMemoryDatabase(databaseName: "TestDatabase")
         .Options;

      _dbContext = new DatabaseContext(options);

      // JwtOptions
      _jwtOptions = Options.Create(new JwtOptions
      {
         SecretKey = "8443c04c79e05d6650242a9e7013013e8737efc0e713613a36c31c06cd6f4b22",
         Issuer = "Zeion",
         Audience = "ZeionDeveloper",
         EXP_ACCESS_TOKEN = "1m",
         EXP_REFRESH_TOKEN = "10m"
      });

      // TokenFactory
      _tokenFactory = new TokenFactory(_hashingMock.Object);

      _authService = new AuthService(
         _redisMock.Object,
         _jwtMock.Object,
         _dbContext,
         _hashingMock.Object,
         _authRepoMock.Object,
         _httpMock.Object,
         _jwtOptions,
         _tokenFactory
      );
   }

   [Fact]
   public async Task LoginTest()
   {
      var mockModel = new LoginModel
      {
         // Username = "zeion@develop.com",
         // Password = "ZeionDeveloper2005@",
         Username = "anhjkr",
         Password = "Anhhung1@",
         RememberMe = false
      };

      var device = new DeviceInfo
      {
         OS = "Mac",
         Browser = "Chrome",
         Version = "1.0",
         Device = "Desktop",
         IPAdress = "127.0.0.1"
      };

      // var mockHash = model.Password;
      // _hashingMock.Setup(x => x.HashValue(It.IsAny<string>())).Returns(model.Password);

      // var user = new User
      // {
      //    UserID = _tokenFactory.CreateSessionId(),
      //    Username = model.Username,
      //    HashPassword = mockHash,
      //    RoleID = 1,
      //    Status = UserStatus.active,
      //    CreatedAt = new DateTime(),
      //    UpdatedAt = new DateTime()
      // };

      // _authRepoMock.Setup(x => x.GetUserByUsername(It.IsAny<string>())).ReturnsAsync(user);

      // _hashingMock
      //    .Setup(x => x.VerifyHashValue(It.IsAny<string>(), It.IsAny<string>()))
      //    .Returns(true);

      // var tokenPayload = new TokenPayload
      // {
      //    User = new UserPayload
      //    {
      //       UserID = user.UserID,
      //       RoleSlug = "RoleTest",
      //       Status = user.Status.ToString()
      //    },
      //    SessionID = _tokenFactory.CreateSessionId(),
      //    RefreshToken = _tokenFactory.CreateSessionId(),
      //    CreatedAt = new DateTime()
      // };

      // _jwtMock
      //    .Setup(x => x.GenerateToken(It.IsAny<TokenPayload>()))
      //    .Returns("accessToken");

      // var result = await _authService.Login(mockModel, device);

      // if (!result.IsSuccess)
      // {
      //    throw new Xunit.Sdk.XunitException($"Login Failed: {result.Message}");
      // }

      // result.Should().NotBeNull();
      // result.IsSuccess.Should().BeTrue();
   }
}
