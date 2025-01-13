using agenda.Application.Commands;
using agenda.Application.Validators;
using FluentValidation;
using Xunit;

namespace AgendaUnitTest.Validators;

public class UserValidatorTests
{
    private readonly UserValidator _validator;

    public UserValidatorTests()
    {
        _validator = new UserValidator();
    }

    [Theory]
    [InlineData("John Doe")] // Nome válido
    [InlineData("Jane")] // Nome válido com tamanho mínimo
    public void Validate_Name_ShouldBeValid(string name)
    {
        // Arrange
        var user = new UserCommand { Name = name };

        // Act
        var result = _validator.Validate(user);

        // Assert
        Assert.True(result.IsValid, "Expected name to be valid.");
    }

    [Theory]
    [InlineData("")] // Nome vazio
    [InlineData("Jo")] // Nome muito curto
    [InlineData(null)] // Nome nulo
    [InlineData("A name that exceeds fifty characters long in total")]
    public void Validate_Name_ShouldBeInvalid(string name)
    {
        // Arrange
        var user = new UserCommand { Name = name };

        // Act
        var result = _validator.Validate(user);

        // Assert
        Assert.False(result.IsValid, "Expected name to be invalid.");
        Assert.Contains(result.Errors, e => e.PropertyName == "Name");
    }

    [Theory]
    [InlineData("ValidPass1@")] // Senha válida
    [InlineData("Complex#123")] // Outra senha válida
    public void Validate_Password_ShouldBeValid(string password)
    {
        // Arrange
        var user = new UserCommand { Password = password };

        // Act
        var result = _validator.Validate(user);

        // Assert
        Assert.True(result.IsValid, "Expected password to be valid.");
    }

    [Theory]
    [InlineData("")] // Senha vazia
    [InlineData("short")] // Senha curta demais
    [InlineData("NoNumbers!")] // Falta número
    [InlineData("nonumbers123")] // Falta maiúscula
    [InlineData("NONUMBERS123")] // Falta minúscula
    [InlineData("NoSpecial123")] // Falta caractere especial
    public void Validate_Password_ShouldBeInvalid(string password)
    {
        // Arrange
        var user = new UserCommand { Password = password };

        // Act
        var result = _validator.Validate(user);

        // Assert
        Assert.False(result.IsValid, "Expected password to be invalid.");
        Assert.Contains(result.Errors, e => e.PropertyName == "Password");
    }

    [Theory]
    [InlineData("valid@example.com")] // Email válido
    [InlineData("user.name@domain.co")] // Outro email válido
    public void Validate_Email_ShouldBeValid(string email)
    {
        // Arrange
        var user = new UserCommand { Email = email };

        // Act
        var result = _validator.Validate(user);

        // Assert
        Assert.True(result.IsValid, "Expected email to be valid.");
    }

    [Theory]
    [InlineData("")]
    [InlineData("invalid-email")]
    [InlineData("user@domain")]
    [InlineData(null)]
    public void Validate_Email_ShouldBeInvalid(string email)
    {
        // Arrange
        var user = new UserCommand { Email = email };

        // Act
        var result = _validator.Validate(user);

        // Assert
        Assert.False(result.IsValid, "Expected email to be invalid.");
        Assert.Contains(result.Errors, e => e.PropertyName == "Email");
    }
}
