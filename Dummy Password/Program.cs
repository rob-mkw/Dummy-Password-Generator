using System;
using System.Security.Cryptography;

class Program
{
    static void Main()
    {
        const int minLength = 12;
        const int maxLength = 50;

        const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string lower = "abcdefghijklmnopqrstuvwxyz";
        const string digits = "0123456789";
        const string symbols = "!@#$%^&*()_+-=[]{}|;:,.<>?";
        string allChars = upper + lower + digits + symbols;

        while (true)
        {
            Console.Write($"Enter password length ({minLength}-{maxLength}), or type 'exit' to quit: ");
            string? input = Console.ReadLine();

            if (string.Equals(input, "exit", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("\n\n\n##### Exiting program #####");
                break;
            }

            if (!int.TryParse(input, out int length) || length < minLength || length > maxLength)
            {
                Console.WriteLine("Invalid input. Please enter a number between 12 and 50.\n");
                continue;
            }

            string password = GenerateSecurePassword(length, upper, lower, digits, symbols, allChars);
            Console.WriteLine("Generated Password: " + password + "\n");
        }
    }

    static string GenerateSecurePassword(int length, string upper, string lower, string digits, string symbols, string allChars)
    {
        var passwordChars = new char[length];
        int currentIndex = 0;

        // Ensure each rule is met
        passwordChars[currentIndex++] = GetRandomChar(upper);
        passwordChars[currentIndex++] = GetRandomChar(lower);
        passwordChars[currentIndex++] = GetRandomChar(digits);
        passwordChars[currentIndex++] = GetRandomChar(symbols);

        // Fill remaining with any allowed characters
        for (; currentIndex < length; currentIndex++)
        {
            passwordChars[currentIndex] = GetRandomChar(allChars);
        }

        Shuffle(passwordChars);
        return new string(passwordChars);
    }

    static char GetRandomChar(string charSet)
    {
        byte[] buffer = new byte[1];
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            do
            {
                rng.GetBytes(buffer);
            } while (buffer[0] >= (byte.MaxValue - (byte.MaxValue % charSet.Length)));

            return charSet[buffer[0] % charSet.Length];
        }
    }

    static void Shuffle(char[] array)
    {
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            for (int i = array.Length - 1; i > 0; i--)
            {
                byte[] buffer = new byte[1];
                do
                {
                    rng.GetBytes(buffer);
                } while (buffer[0] >= (byte.MaxValue - (byte.MaxValue % (i + 1))));

                int j = buffer[0] % (i + 1);
                (array[i], array[j]) = (array[j], array[i]);
            }
        }
    }
}
