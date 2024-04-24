
using System.Text;
using System;
using UnityEngine;
public static class FormatText
{
    public static string FormatNumber(int input)
    {
        string inputString = input.ToString();
        return FormatNumber(inputString);
    }
    private static string FormatNumber(string input)
    {
        char[] charArray = input.ToCharArray();
        Array.Reverse(charArray);
        string reversedInput = new string(charArray);
        StringBuilder formattedString = new StringBuilder();
        for (int i = 0; i < reversedInput.Length; i++)
        {
            formattedString.Append(reversedInput[i]);
            if ((i + 1) % 3 == 0 && i < reversedInput.Length - 1)
            {
                formattedString.Append(",");
            }
        }
        charArray = formattedString.ToString().ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }
}
