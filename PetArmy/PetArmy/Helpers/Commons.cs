using System;
using System.Text.RegularExpressions;

namespace PetArmy.Helpers
{
    public static class Commons
    {
        public const string DefaultPhoneNumber = "+506 0000 0000";
        public const string FirebaseProjectId = "538291567160-era9d1f0o9qutsssrf6fcqmi45n9abfe.apps.googleusercontent.com";
        public static readonly string AdminCreationRequestFunction = "adminCreationRequest";
        public static readonly string AdminCreationApprovalFunction = "adminCreationApproval";
        public static readonly string AdminCreationRequestsFunction = "adminCreationRequests";
        const Int16 minimum_password_length = 6;
        const string emailRegex = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
        @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";
        const string phoneRegex = @"\+\d{3} \d{4} \d{4}";

        public static bool IsValidEmail(string email) {
            return (Regex.IsMatch(email, emailRegex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)));
        }

        public static bool IsValidPassword(string password) {
            return password.Length >= minimum_password_length;
        }

        public static bool IsValidPhone(string phone)
        {
            if(phone == null || phone == string.Empty || phone == DefaultPhoneNumber)
                return false;
            return Regex.IsMatch(phone, phoneRegex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        }
    }
}
