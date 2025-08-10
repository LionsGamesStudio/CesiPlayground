using System;
using System.Security.Cryptography;
using System.Text;

namespace CesiPlayground.Shared.Security
{
    /// <summary>
    /// A utility class for handling password hashing and verification.
    /// Uses SHA256 with a salt.
    /// </summary>
    public static class PasswordHasher
    {
        private const int SaltSize = 32;

        /// <summary>
        /// Hashes a password with a newly generated salt.
        /// </summary>
        /// <param name="password">The plain-text password to hash.</param>
        /// <returns>The hashed password string (salt + hash), Base64 encoded.</returns>
        public static string HashPassword(string password)
        {
            // 1. Generate a salt
            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // 2. Hash the password with the salt
            byte[] hash = ComputeHash(password, salt);

            // 3. Combine salt and hash for storage
            byte[] hashBytes = new byte[SaltSize + hash.Length];
            Buffer.BlockCopy(salt, 0, hashBytes, 0, SaltSize);
            Buffer.BlockCopy(hash, 0, hashBytes, SaltSize, hash.Length);

            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// Verifies a plain-text password against a stored hash.
        /// This logic should ideally live ONLY ON THE SERVER.
        /// </summary>
        /// <param name="password">The plain-text password to verify.</param>
        /// <param name="hashedPasswordWithSalt">The stored hash from the database.</param>
        /// <returns>True if the password is correct.</returns>
        public static bool VerifyPassword(string password, string hashedPasswordWithSalt)
        {
            // 1. Extract salt and hash from the stored string
            byte[] hashBytes = Convert.FromBase64String(hashedPasswordWithSalt);
            byte[] salt = new byte[SaltSize];
            Buffer.BlockCopy(hashBytes, 0, salt, 0, SaltSize);
            
            // 2. Compute the hash of the input password with the extracted salt
            byte[] computedHash = ComputeHash(password, salt);
            
            // 3. Compare the computed hash with the stored hash
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (hashBytes[i + SaltSize] != computedHash[i])
                {
                    return false; // Mismatch found
                }
            }
            return true; // Hashes match
        }

        private static byte[] ComputeHash(string password, byte[] salt)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] combinedBytes = new byte[passwordBytes.Length + salt.Length];
                Buffer.BlockCopy(passwordBytes, 0, combinedBytes, 0, passwordBytes.Length);
                Buffer.BlockCopy(salt, 0, combinedBytes, passwordBytes.Length, salt.Length);
                return sha256.ComputeHash(combinedBytes);
            }
        }
    }
}