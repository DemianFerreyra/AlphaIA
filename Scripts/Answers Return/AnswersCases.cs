using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswersCases : MonoBehaviour
{
    public int currentWordOrder;
    public string latestWord;
    public string GetAnswer(Word currentWord)
    {
        Debug.Log("me estoy ejecutando");
        if (currentWordOrder == 0)
        { //es decir si es la primer palabra
            if (currentWord.wordTypes[0] == "greeting")
            {
                latestWord = "greeting";
                return "hola buenas";
            }
            currentWordOrder += 1;
        }

        if (currentWordOrder == 1)
        { //es decir si es la segunda palabra
            if (currentWord.wordTypes[0] == "greeting")
            {
                latestWord = "";
                currentWordOrder = 0;
                return "ignoreNoSense";
            }
            if (currentWord.wordTypes[0] == "unknown" && latestWord == "greeting")
            {
                latestWord = "";
                Debug.Log(StringCompare("alphaIA", currentWord.word));
                if (StringCompare("alphaIA", currentWord.word) < 56 || StringCompare("IA", currentWord.word) < 56)
                {
                    currentWordOrder = 0;
                    return $"who is {currentWord.word}?";
                }
                else
                {
                    currentWordOrder = 0;
                    return "usuario";
                }
            }
            currentWordOrder += 1;
        }
        return "ignore";
    }




    static double StringCompare(string a, string b)
    {
        if (a == b) //Same string, no iteration needed.
            return 100;
        if ((a.Length == 0) || (b.Length == 0)) //One is empty, second is not
        {
            return 0;
        }
        double maxLen = a.Length > b.Length ? a.Length : b.Length;
        int minLen = a.Length < b.Length ? a.Length : b.Length;
        int sameCharAtIndex = 0;
        for (int i = 0; i < minLen; i++) //Compare char by char
        {
            if (a[i] == b[i])
            {
                sameCharAtIndex++;
            }
        }
        return sameCharAtIndex / maxLen * 100;
    }
}
