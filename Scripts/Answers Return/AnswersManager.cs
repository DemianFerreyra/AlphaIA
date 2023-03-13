using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class AnswersManager : MonoBehaviour
{
    public bool newPhrase = true;
    public int order;
    public List<string> lastWordTypes;
    public List<string> answers;

    public string GetAnswerStructure(Word currentWord, int wordCount)
    {

        order++;
        if (newPhrase == true)
        {
            Debug.Log(JsonUtility.ToJson(currentWord,true));
            newPhrase = false;
            AddLastWord(currentWord);
            if (currentWord.wordTypes.Contains("greeting"))
            {
                answers.Add("{greetingRandom}");
                return "{greetingRandom}";
            }
            if (currentWord.wordTypes.Contains("action"))
            {
                if (currentWord.wordTypes.Contains("referenceAlpha"))
                {
                    lastWordTypes.Add("actionAlpha");
                    answers.Add("alpha:action");
                }
                return "ignore";
            }
            if (currentWord.wordTypes.Contains("question"))
            {
                if (currentWord.wordTypes.Contains("placeQuestion"))
                {
                    answers.Add("placeQuestion");
                    lastWordTypes.Add("placeQuestion");
                }
                if (currentWord.wordTypes.Contains("likesQuestion"))
                {
                    answers.Add("likesQuestion");
                    lastWordTypes.Add("likesQuestion");
                }
                return "ignore";
            }
            return "end";
        }
        else  //--------------------------------------------//
        {
            if (lastWordTypes.Contains("greeting"))
            {
                if (currentWord.wordTypes.Contains("unknown") && currentWord.word.Length > 2)
                {
                    newPhrase = true;
                    if (StringCompare(currentWord.word, "AlphaIA") > 54)
                    {
                        answers[answers.Count - 1] = answers[answers.Count - 1] + " {usuario}";
                        return "end";
                    }
                    else
                    {
                        answers[answers.Count - 1] = answers[answers.Count - 1] + $" no conozco a {currentWord.word} del que estas hablando";
                        return "end";
                    }
                }
                order--;
                return "repeat";
            }
            if (lastWordTypes.Contains("placeQuestion"))
            {
                if (currentWord.wordTypes.Contains("place"))
                {
                    answers[answers.Count - 1] = answers[answers.Count - 1] + $":{currentWord.word}";
                }
                return "ignore";
            }
            if (lastWordTypes.Contains("likesQuestion"))
            {
                if (currentWord.wordTypes.Contains("unknown"))
                {
                    lastWordTypes.Clear();
                    lastWordTypes.Add("mayContinueWord");
                    answers[answers.Count - 1] = answers[answers.Count - 1] + $":{currentWord.word}";
                }
                return "ignore";
            }
            if (lastWordTypes.Contains("question"))
            {
                if (currentWord.wordTypes.Contains("unknown"))
                {
                    return "ignoreNoSense";
                }
                if (currentWord.wordTypes.Contains("greeting"))
                {
                    if (lastWordTypes.Contains("conjunction"))
                    {
                        lastWordTypes.Clear();
                        lastWordTypes.Add("greeting");
                        answers.Add("{greetingRandom}");
                        return "ignore";
                    }
                }
                else
                {
                    lastWordTypes.Clear();
                    lastWordTypes.Add("needsAnswer");
                    answers.Add("needstoAnswer");
                    return "ignore";
                }
            }
            if (lastWordTypes.Contains("needsAnswer"))
            {
                if (currentWord.wordTypes.Contains("affirmation") && currentWord.answerType.Length > 1)
                {
                    newPhrase = true;
                    lastWordTypes.Clear();
                    answers[answers.Count - 1] = answers[answers.Count - 1] + $":{currentWord.answerType}";
                }
                else if (currentWord.wordTypes.Contains("question") && currentWord.answerType.Length > 1)
                {
                    newPhrase = true;
                    lastWordTypes.Clear();
                    answers[answers.Count - 1] = answers[answers.Count - 1] + $":{currentWord.answerType}";
                }
                else
                {
                    return "ignore";
                }

            }
            if (lastWordTypes.Contains("actionAlpha"))
            {
                if (currentWord.wordTypes.Contains("action"))
                {
                    lastWordTypes.Clear();
                    lastWordTypes.Add("mayNeedComplement");
                    answers[answers.Count - 1] = answers[answers.Count - 1] + $":{currentWord.answerType}";
                    return "ignore";
                }
            }
            if (lastWordTypes.Contains("mayNeedComplement"))
            {
                if (currentWord.wordTypes.Contains("conjunction"))
                {
                    return "ignore";
                }
                if (currentWord.wordTypes.Contains("unknown"))
                {
                    lastWordTypes.Clear();
                    lastWordTypes.Add("mayContinueWord");
                    answers[answers.Count - 1] = answers[answers.Count - 1] + $":{currentWord.word}";
                    return "ignore";
                }
            }
            if (lastWordTypes.Contains("mayContinueWord"))
            {
                if (currentWord.wordTypes.Contains("conjunction"))
                {
                    return "ignore";
                }
                if (currentWord.wordTypes.Contains("unknown"))
                {
                    lastWordTypes.Clear();
                    lastWordTypes.Add("mayContinueWord");
                    answers[answers.Count - 1] = answers[answers.Count - 1] + $" {currentWord.word}";
                    return "ignore";
                }
            }
            return "end";
        }
    }


    private void AddLastWord(Word currentWord)
    {
        foreach (var wordType in currentWord.wordTypes)
        {
            lastWordTypes.Add(wordType);
        }
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