using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class AnswersCases : MonoBehaviour
{
    public int currentWordOrder;
    public string latestWord;
    public string latestWordType;
    public string GetAnswer(Word currentWord)
    {
        if (currentWordOrder == 0)
        { //es decir si es la primer palabra
            currentWordOrder += 1;
            if (currentWord.wordTypes.Contains("unknown"))
            {
                latestWord = "unknown";
                return "ignore";
            }
            if (currentWord.wordTypes.Contains("greeting"))
            {
                latestWord = "greeting";
                return $"greeting:{currentWord.answerType}:{currentWord.word}";
            }
            if (currentWord.wordTypes.Contains("action"))
            {
                //una frase no puede empezar con una accion, seria un noSense
                if (currentWord.wordTypes.Contains("conjunction"))
                {
                    latestWord = "conjunction";
                    return "ignore";
                }
                else
                {
                    return "ignoreNoSense";
                }
            }
            if (currentWord.wordTypes.Contains("question"))
            {
                latestWord = "question";
                return "ignore";
            }
            if (currentWord.wordTypes.Contains("time"))
            {
                latestWordType = "tomorrow";
                latestWord = "timeQuestion";
                return "ignore";
            }
            return "ignore";
        }

        if (currentWordOrder == 1)
        { //es decir si es la segunda palabra
            currentWordOrder += 1;
            if (latestWord == "greeting")
            {
                if (currentWord.wordTypes[0] == "greeting")
                {
                    latestWord = "";
                    currentWordOrder = 0;
                    return "ignore";
                }
                if (currentWord.wordTypes[0] == "unknown")
                {
                    latestWord = "";
                    currentWordOrder = 0;
                    if (StringCompare("alphaIA", currentWord.word) < 56)
                    {
                        return $"likes:{currentWord.answerType}:{currentWord.word}";
                        // string answer = answerManager.ReturnCorrectAnswer("unknown", "unknownName");
                        // string newAnswer = answer.Replace("{unknown}", currentWord.word);
                        // return $"...{newAnswer}";
                    }
                    else
                    {
                        return "usuario";
                    }
                }
                if (currentWord.wordTypes.Contains("question"))
                {
                    latestWord = "question";
                    return "ignore";
                }
                if (currentWord.wordTypes.Contains("conjunction"))
                {
                    latestWord = "conjunction";
                    currentWordOrder = 1;
                    return "ignore";
                }
            }
            if (latestWord == "unknown")
            {
                if (currentWord.wordTypes.Contains("question"))
                {
                    latestWord = "question";
                    string[] questionAboutLikes = { "gusta", "gusto" };
                    foreach (var question in questionAboutLikes)
                    {
                        if (question == currentWord.word)
                        {
                           latestWord = "likesQuestion";
                        return "ignore";
                        }
                    }
                    return $"question:{currentWord.answerType}:{currentWord.word}";
                }
            }
            if (latestWord == "conjunction")
            {
                if (currentWord.wordTypes.Contains("question"))
                {
                    latestWord = "question";
                    return $"question:{currentWord.answerType}:{currentWord.word}";
                }
            }
            if (latestWord == "question")
            {
                return "ignore";
            }
            if (latestWord == "timeQuestion")
            {
                if (currentWord.wordTypes.Contains("question"))
                {
                    if (currentWord.wordTypes.Contains("conjunction"))
                    {
                        latestWord = "question";
                        return "ignore";
                    }
                    latestWord = "timeQuestion";
                    return "ignore";
                }
            }
            return "ignore";
        }
        if (currentWordOrder > 1)
        {
            currentWordOrder += 1;
            if (latestWord == "question" || latestWord == "conjunction")
            {
                if (currentWord.wordTypes.Contains("conjunction"))
                {
                    return "ignore";
                }
                if (currentWord.wordTypes[0] == "unknown" || currentWord.wordTypes.Contains("conjunction") == false && currentWord.wordTypes.Contains("adjective") == false)
                {
                    return $"question:{currentWord.answerType}:{currentWord.word}";
                }
            }
            if (latestWord == "timeQuestion")
            {
                if (currentWord.wordTypes.Contains("conjunction"))
                {
                    return "ignore";
                }
                if (currentWord.wordTypes[0] == "unknown" || currentWord.wordTypes.Contains("conjunction") == false && currentWord.wordTypes.Contains("adjective") == false)
                {
                    if (latestWordType.Length > 1)
                    {
                        return $"likes:{currentWord.answerType}:{currentWord.word}";
                    }
                    return "ignore";
                }
            }
            if (latestWord == "likesQuestion")
            {
                return $"likes:{currentWord.answerType}:{currentWord.word}";
            }
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
