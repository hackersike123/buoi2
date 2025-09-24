using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace bai1
{
    internal class UserRecord
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }

    internal static class AuthService
    {
        private static readonly string UsersFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "users.txt");

        public static List<UserRecord> LoadUsers()
        {
            try
            {
                if (!File.Exists(UsersFile))
                {
                    // create default admin
                    var defaultList = new List<UserRecord>
                    {
                        new UserRecord { Username = "admin", PasswordHash = HashPassword("123456") }
                    };
                    SaveUsers(defaultList);
                    return defaultList;
                }

                var lines = File.ReadAllLines(UsersFile, Encoding.UTF8);
                var list = new List<UserRecord>();
                foreach (var line in lines)
                {
                    var parts = line.Split(new[] { '|' }, 2);
                    if (parts.Length == 2)
                        list.Add(new UserRecord { Username = parts[0], PasswordHash = parts[1] });
                }
                return list;
            }
            catch
            {
                return new List<UserRecord>();
            }
        }

        public static void SaveUsers(List<UserRecord> users)
        {
            var sb = new StringBuilder();
            foreach (var u in users)
            {
                sb.AppendLine($"{u.Username}|{u.PasswordHash}");
            }
            File.WriteAllText(UsersFile, sb.ToString(), Encoding.UTF8);
        }

        public static bool Verify(string username, string password)
        {
            var users = LoadUsers();
            var u = users.FirstOrDefault(x => string.Equals(x.Username, username, StringComparison.OrdinalIgnoreCase));
            if (u == null) return false;
            return string.Equals(u.PasswordHash, HashPassword(password), StringComparison.Ordinal);
        }

        public static bool Register(string username, string password)
        {
            var users = LoadUsers();
            if (users.Any(x => string.Equals(x.Username, username, StringComparison.OrdinalIgnoreCase))) return false;
            users.Add(new UserRecord { Username = username, PasswordHash = HashPassword(password) });
            SaveUsers(users);
            return true;
        }

        public static string ResetPassword(string username)
        {
            var users = LoadUsers();
            var u = users.FirstOrDefault(x => string.Equals(x.Username, username, StringComparison.OrdinalIgnoreCase));
            if (u == null) return null;
            var tmp = GenerateTempPassword();
            u.PasswordHash = HashPassword(tmp);
            SaveUsers(users);
            return tmp;
        }

        // New: set user's password to a specific value
        public static bool SetPassword(string username, string newPassword)
        {
            var users = LoadUsers();
            var u = users.FirstOrDefault(x => string.Equals(x.Username, username, StringComparison.OrdinalIgnoreCase));
            if (u == null) return false;
            u.PasswordHash = HashPassword(newPassword);
            SaveUsers(users);
            return true;
        }

        private static string HashPassword(string pwd)
        {
            using (var sha = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(pwd ?? string.Empty);
                var hash = sha.ComputeHash(bytes);
                var sb = new StringBuilder();
                foreach (var b in hash) sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        private static string GenerateTempPassword()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var bytes = new byte[8];
                rng.GetBytes(bytes);
                return Convert.ToBase64String(bytes).TrimEnd('=');
            }
        }
    }
}
