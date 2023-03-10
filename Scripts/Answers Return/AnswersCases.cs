using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class AnswersCases : MonoBehaviour
{
    public int currentWordOrder;
    public string latestWord;
    public GetCorrectAnswers answerManager;
    public string GetAnswer(Word currentWord)
    {
        if (currentWordOrder == 0)
        { //es decir si es la primer palabra
            currentWordOrder += 1;
            if (currentWord.wordTypes.Contains("unknown"))
            {
                return "ignore";
            }
            if (currentWord.wordTypes.Contains("greeting"))
            {
                latestWord = "greeting";
                return answerManager.ReturnCorrectAnswer("greeting", currentWord.word);
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
                latestWord = "timeQuestion";
                return currentWord.word;
            }
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
                    return "ignoreNoSense";
                }
                if (currentWord.wordTypes[0] == "unknown")
                {
                    latestWord = "";
                    currentWordOrder = 0;
                    if (StringCompare("alphaIA", currentWord.word) < 56)
                    {
                        string answer = answerManager.ReturnCorrectAnswer("unknown", "unknownName");
                        string newAnswer = answer.Replace("{unknown}", currentWord.word);
                        return $"...{newAnswer}";
                    }
                    else
                    {
                        return "usuario";
                    }
                }
                if (currentWord.wordTypes.Contains("conjunction"))
                {
                    latestWord = "conjunction";
                    currentWordOrder = 1;
                    return "ignore";
                }
            }
            if (latestWord == "conjunction")
            {
                if (currentWord.wordTypes.Contains("question"))
                {
                    return answerManager.ReturnCorrectAnswer("question", currentWord.word);
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
                    return $" si habra";
                }
            }
        }
        if (currentWordOrder >= 2)
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
                    return $"me hicieron una pregunta sobre: {currentWord.word}";
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
                    return $" {currentWord.word}";
                }
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
