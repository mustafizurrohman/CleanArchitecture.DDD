﻿using System.Net.Mail;
using System.Text.RegularExpressions;

namespace CleanArchitecture.DDD.Core.ExtensionMethods;

/// <summary>
/// Extension methods for string
/// </summary>
public static class StringExtensions
{

    /// <summary>
    /// Returns true of the provided string represents an email. Otherwise false.
    /// Does not actually verify the email.
    /// </summary>
    /// <param name="email">The email.</param>
    /// <returns><c>true</c> if [is valid email] [the specified email]; otherwise, <c>false</c>.</returns>
    public static bool IsValidEmail(this string email)
    {
        try
        {
            var address = new MailAddress(email);
            return address.Address == email;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Returns true of the provided string represents an email. Otherwise false.
    /// Does not actually verify the email.
    /// </summary>
    /// <param name="password">The password.</param>
    /// <param name="minLength">The Minimum Length</param>
    /// <returns><c>true</c> if [is valid password] [the specified password]; otherwise, <c>false</c>.</returns>
    public static bool IsValidPassword(this string password, int minLength = 8)
    {
        if (password.Length < minLength || string.IsNullOrWhiteSpace(password))
            return false;

        if (!password.Any(char.IsUpper))
            return false;


        if (!password.Any(char.IsLower))
            return false;

        if (!password.Any(char.IsDigit))
            return false;

        // ReSharper disable once ConvertIfStatementToReturnStatement
        if (password.All(char.IsLetterOrDigit))
            return false;

        return true;

    }

    /// <summary>
    /// Returns true of the provided string represents an email. Otherwise false.
    /// Does not actually verify the email.
    /// </summary>
    /// <param name="password">The password.</param>
    /// <returns><c>true</c> if [contains special characters] [the specified password]; otherwise, <c>false</c>.</returns>
    public static bool ContainsSpecialCharacters(this string password)
    {
        return password.Any(c => !char.IsLetterOrDigit(c));
    }

    /// <summary>
    /// Randomizes a string
    /// </summary>
    /// <param name="input">The input string.</param>
    /// <returns>System.String.</returns>
    public static string Randomize(this string input)
    {
        return new string(input.ToCharArray().OrderBy(_ => Guid.NewGuid()).ToArray());
    }

    /// <summary>
    /// Remove duplicate characters from a string
    /// </summary>
    /// <param name="input">The input string.</param>
    /// <returns>System.String.</returns>
    public static string RemoveDuplicates(this string input)
    {
        return new string(input.ToCharArray().Distinct().ToArray());
    }

    /// <summary>
    /// Removes the consecutive spaces.
    /// </summary>
    /// <param name="input">The input string.</param>
    /// <returns>System.String.</returns>
    public static string RemoveConsecutiveSpaces(this string input)
    {
        return Regex.Replace(input.Trim(), @"\s+", " ");
    }

    /// <summary>
    /// Counts the number of words in a string
    /// </summary>
    /// <param name="sourceString">The source string.</param>
    /// <returns>System.Int32.</returns>
    public static int CountWords(this string sourceString)
    {
        return sourceString
            .Trim()
            .Split(" ")
            .Count(substring => !string.IsNullOrWhiteSpace(substring));
    }

    /// <summary>
    /// Capitalizes the specified source string.
    /// </summary>
    /// <param name="sourceString">The source string.</param>
    /// <returns>System.String.</returns>
    // ReSharper disable once MemberCanBePrivate.Global
    public static string CapitalizeWord(this string sourceString)
    {
        if (string.IsNullOrEmpty(sourceString))
            return string.Empty;

        return char.ToUpper(sourceString[0]) + sourceString.Substring(1);
    }

    /// <summary>
    /// Capitalizes the each word of sentence.
    /// </summary>
    /// <param name="sourceString">The source string.</param>
    /// <returns>System.String.</returns>
    public static string CapitalizeEachWordOfSentence(this string sourceString)
    {
        return sourceString
            .Split(" ")
            .Where(word => !string.IsNullOrWhiteSpace(word))
            .Select(CapitalizeWord)
            .Aggregate((str1, str2) => str1 + " " + str2)
            .ToString();
    }

    /// <summary>
    /// Converts to bytearray.
    /// </summary>
    /// <param name="sourceString">The source string.</param>
    /// <returns>System.Byte[].</returns>
    public static byte[] ToByteArray(this string sourceString)
    {
        return Encoding.UTF8.GetBytes(sourceString);
    }

    public static string ToBase64String(this string inputString)
    {
        return Convert.ToBase64String(inputString.ToByteArray());
    }

    public static string CamelCaseToSentence(this string inputString)
    {
        return Regex.Replace(inputString, "([A-Z])", " $1").Trim();
    }
}