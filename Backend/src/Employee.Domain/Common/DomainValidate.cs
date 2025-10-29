using System;

namespace EmployeeManagerDomain.Common
{
    public static class DomainValidate
    {
        public static string Name(string value, string paramName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", paramName);
            }

            var trimmed = value.Trim();
            if (trimmed.Length < 2 || trimmed.Length > 100)
            {
                throw new ArgumentOutOfRangeException(paramName, "Name must be between 2 and 100 characters.");
            }

            return trimmed;
        }

        public static string Email(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be null or whitespace.", nameof(email));
            }

            var trimmed = email.Trim();
            try
            {
                var mail = new System.Net.Mail.MailAddress(trimmed);
                return mail.Address.ToLowerInvariant();
            }
            catch (FormatException)
            {
                throw new ArgumentException("Email format is invalid.", nameof(email));
            }
        }

        public static string Position(string position)
        {
            if (string.IsNullOrWhiteSpace(position))
            {
                throw new ArgumentException("Position cannot be null or whitespace.", nameof(position));
            }

            var trimmed = position.Trim();
            if (trimmed.Length < 2 || trimmed.Length > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(position), "Position must be between 2 and 100 characters.");
            }

            return trimmed;
        }
    }
}


