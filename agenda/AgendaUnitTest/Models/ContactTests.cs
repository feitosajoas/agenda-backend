using agenda.Models;
using Xunit;

namespace AgendaUnitTest.Models
{
    public class ContactTests
    {
        [Fact]
        public void Contact_ShouldHaveValidProperties()
        {
            // Arrange
            var ownerContactId = Guid.NewGuid();
            var contact = new Contact
            {
                Name = "John Doe",
                PhoneNumber = "123456789",
                Email = "john.doe@example.com",
                OwnerContactId = ownerContactId
            };

            // Act & Assert
            Assert.NotNull(contact);
            Assert.Equal("John Doe", contact.Name);
            Assert.Equal("123456789", contact.PhoneNumber);
            Assert.Equal("john.doe@example.com", contact.Email);
            Assert.NotEqual(Guid.Empty, contact.OwnerContactId);
            Assert.Equal(ownerContactId, contact.OwnerContactId);
        }

        [Theory]
        [InlineData("1234567890")] // Número de telefone válido
        [InlineData("+5511987654321")] // Número de telefone internacional
        [InlineData("(11)98765-4321")] // Número com formato brasileiro
        public void Contact_PhoneNumber_ShouldBeValid(string phoneNumber)
        {
            // Arrange
            var contact = new Contact { PhoneNumber = phoneNumber };

            // Act & Assert
            Assert.NotNull(contact.PhoneNumber);
            Assert.Equal(phoneNumber, contact.PhoneNumber);
        }

        [Theory]
        [InlineData("abc")] // Número de telefone inválido
        [InlineData("123-456-7890x")] // Número de telefone inválido
        [InlineData("")] // Número vazio
        [InlineData(null)] // Número nulo
        public void Contact_PhoneNumber_ShouldBeInvalid(string phoneNumber)
        {
            // Arrange
            var contact = new Contact { PhoneNumber = phoneNumber };

            // Act
            var isValid = IsPhoneNumberValid(contact.PhoneNumber);

            // Assert
            Assert.False(isValid, $"Expected phone number '{phoneNumber}' to be invalid.");
        }

        [Theory]
        [InlineData("john.doe@example.com")] // Email válido
        [InlineData("jane_doe@example.org")] // Email válido
        [InlineData("user+test@subdomain.domain.com")] // Email com subdomínio
        public void Contact_Email_ShouldBeValid(string email)
        {
            // Arrange
            var contact = new Contact { Email = email };

            // Act & Assert
            Assert.NotNull(contact.Email);
            Assert.True(IsEmailValid(contact.Email), $"Expected email '{email}' to be valid.");
        }

        [Theory]
        [InlineData("invalido")] // Email inválido
        [InlineData("john.doe@example")] // Email inválido
        [InlineData("")] // Email vazio
        [InlineData(null)] // Email nulo
        public void Contact_Email_ShouldBeInvalid(string email)
        {
            // Arrange
            var contact = new Contact { Email = email };

            // Act
            var isValid = IsEmailValid(contact.Email);

            // Assert
            Assert.False(isValid, $"Expected email '{email}' to be invalid.");
        }

        private bool IsPhoneNumberValid(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            var phoneRegex = new System.Text.RegularExpressions.Regex(@"^\+?\d{8,15}$");
            return phoneRegex.IsMatch(phoneNumber);
        }

        private bool IsEmailValid(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            var emailRegex = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            return emailRegex.IsMatch(email);
        }
    }
}
