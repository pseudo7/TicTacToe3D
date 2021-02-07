using UnityEngine;

namespace TicTacToe3D.Utilities
{
    public static class TextUtility
    {
        public static string ToColoredString(this string currentString, Color stringColor) =>
            $"<color=#{ColorUtility.ToHtmlStringRGB(stringColor)}>{currentString}</color>";
    }
}