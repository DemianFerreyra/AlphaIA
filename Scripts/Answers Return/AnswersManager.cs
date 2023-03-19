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
            newPhrase = false;
            AddLastWord(currentWord);
            if (StringCompare("Demian", currentWord.word) > 60)
            {
                answers.Add("demian");
                return "ignore";
            }
            if (currentWord.wordTypes.Contains("insult"))
            {
                answers.Add($"insult");
                return "end";
            }
            if (currentWord.wordTypes.Contains("jerga"))
            {
                answers.Add($"jerga:{currentWord.word}");
                return "end";
            }
            if (currentWord.wordTypes.Contains("greeting"))
            {
                if (currentWord.answerType == "despedidas")
                {
                    answers.Add("greeting:despedidas");
                }
                else
                {
                    answers.Add("greeting:saludos");
                }
                return "ignore";
            }
            if (currentWord.wordTypes.Contains("question"))
            {
                if (currentWord.wordTypes.Contains("placeQuestion"))
                {
                    lastWordTypes.Clear();
                    answers.Add("placeQuestion");
                    lastWordTypes.Add("placeQuestion");
                    return "ignore";
                }
                if (currentWord.wordTypes.Contains("likesQuestion"))
                {
                    lastWordTypes.Clear();
                    answers.Add("likesQuestion");
                    lastWordTypes.Add("likesQuestion");
                    return "ignore";
                }
                lastWordTypes.Clear();
                answers.Add("needsToAnswer");
                lastWordTypes.Add("needsAnswer");
                return "ignore";
            }
            if (currentWord.wordTypes.Contains("action"))
            {
                if (currentWord.wordTypes.Contains("referenceAlpha"))
                {
                    lastWordTypes.Clear();
                    lastWordTypes.Add("actionAlpha");
                    answers.Add("alpha:action");
                }
                return "ignore";
            }
            if (currentWord.wordTypes.Contains("obligation"))
            {
                answers.Add($"obligation");
                return "ignore";
            }
            if (currentWord.wordTypes.Contains("reference"))
            {
                return "ignore";
            }
            return "end";
        }
        else  //--------------------------------------------//
        {
            if (currentWord.wordTypes.Contains("insult"))
            {
                answers.Add($"insult");
                return "end";
            }
            if (lastWordTypes.Contains("reference"))
            {
                if (StringCompare("Demian", currentWord.word) > 60 && answers.Count == 0)
                {
                    answers.Add("demian");
                    return "ignore";
                }
                if (currentWord.wordTypes.Contains("unknown"))
                {
                    return "ignore";
                }
                if (currentWord.wordTypes.Contains("question"))
                {
                    if (currentWord.wordTypes.Contains("likesQuestion"))
                    {
                        lastWordTypes.Clear();
                        answers[answers.Count - 1] = answers[answers.Count - 1] + ":likesQuestion";
                        lastWordTypes.Add("likesQuestion");
                        return "ignore";
                    }
                    lastWordTypes.Clear();
                    answers[answers.Count - 1] = answers[answers.Count - 1] + ":needsToAnswer";
                    lastWordTypes.Add("needsAnswer");
                    return "ignore";
                }
            }
            if (lastWordTypes.Contains("greeting"))
            {
                if (currentWord.wordTypes.Contains("unknown") && currentWord.word.Length > 2)
                {
                    newPhrase = true;
                    if (StringCompare(currentWord.word, "AlphaIA") > 54)
                    {
                        answers[answers.Count - 1] = answers[answers.Count - 1] + ":user";
                        return "end";
                    }
                    else
                    {
                        answers[answers.Count - 1] = answers[answers.Count - 1] + $":unknown";
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
                if (currentWord.wordTypes.Contains("reference"))
                {
                    lastWordTypes.Clear();
                    lastWordTypes.Add("referenceNeedsContinue");
                    return "ignore";
                }
                if (StringCompare("Demian", currentWord.word) > 60)
                {
                    answers[answers.Count - 1] = answers[answers.Count - 1] + $":demian";
                    return "ignore";
                }
                if (currentWord.wordTypes.Contains("unknown"))
                {
                    lastWordTypes.Clear();
                    lastWordTypes.Add("mayContinueWord");
                    answers[answers.Count - 1] = answers[answers.Count - 1] + $":{currentWord.word}";
                    return "ignore";
                }
                return "end";
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
                    answers.Add("needsToAnswer");
                    return "ignore";
                }
            }
            if (lastWordTypes.Contains("referenceNeedsContinue"))
            {
                if (StringCompare(currentWord.word, "Demian") > 40)
                {
                    answers[answers.Count - 1] = "Demian:" + answers[answers.Count - 1];
                }
                return "end";
            }
            if (lastWordTypes.Contains("needsAnswer"))
            {
                if (currentWord.wordTypes.Contains("reference"))
                {
                    lastWordTypes.Clear();
                    lastWordTypes.Add("referenceNeedContinue");
                    return "ignore";
                }
                if (currentWord.wordTypes.Contains("likesQuestion"))
                {
                    lastWordTypes.Clear();
                    lastWordTypes.Add("likesQuestion");
                    answers[answers.Count - 1] = answers[answers.Count - 1] + $":{currentWord.answerType}";
                    return "ignore";
                }
                if (currentWord.wordTypes.Contains("affirmation") && currentWord.answerType.Length > 1)
                {
                    newPhrase = true;
                    lastWordTypes.Clear();
                    answers[answers.Count - 1] = answers[answers.Count - 1] + $":{currentWord.answerType}";
                    return "ignore";
                }
                if (currentWord.wordTypes.Contains("question") && currentWord.answerType.Length > 1)
                {
                    newPhrase = true;
                    lastWordTypes.Clear();
                    answers[answers.Count - 1] = answers[answers.Count - 1] + $":{currentWord.answerType}";
                    return "ignore";
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
                if (currentWord.wordTypes.Contains("unknown"))
                {
                    lastWordTypes.Clear();
                    answers[answers.Count - 1] = answers[answers.Count - 1] + $":{currentWord.word}";
                    return "ignore";
                }
            }
            if (lastWordTypes.Contains("obligation"))
            {
                if (currentWord.wordTypes.Contains("affirmation") || currentWord.wordTypes.Contains("negation"))
                {
                    answers[answers.Count - 1] = answers[answers.Count - 1] + $":{currentWord.answerType}";
                    return "ignore";
                }
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