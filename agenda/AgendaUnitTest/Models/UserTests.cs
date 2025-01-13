namespace AgendaUnitTest.Models;

using agenda.Models;
using Xunit;

public class UserTests
{
    [Fact]
    public void User_ShouldHaveValidProperties()
    {
        // Arrange
        var user = new User
        {
            Name = "John Doe",
            Password = "password123",
            Email = "john.doe@example.com"
        };

        // Act
        // No action needed here, as we are simply checking the properties

        // Assert
        Assert.NotNull(user);
        Assert.Equal("John Doe", user.Name);
        Assert.Equal("password123", user.Password);
        Assert.Equal("john.doe@example.com", user.Email);
    }
}
