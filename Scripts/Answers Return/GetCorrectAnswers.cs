using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
public class GetCorrectAnswers : MonoBehaviour
{
    public Answers intent;
    public string ReturnCorrectAnswer(string wordType, string word)
    {
        intent = JsonUtility.FromJson<Answers>(File.ReadAllText(Application.dataPath + $"/Data/Common Answers/{wordType}s.json"));
        foreach (var answer in intent.answers)
        {
            if (answer.keys.Contains(word))
            {
                return answer.options[Random.Range(0, answer.options.Count)];
            }
        }
        return "ignore";
    }
    public string ReturnAnswerToQuestion(string question, string questionType)
    {
        if (questionType == "personalState")
        {
            return "estoy bien";
        }
        if (questionType == "likes")
        {
            return $"si, me gusta {question}";
        }
        if (questionType == "tomorrow" || questionType == "yesterday" || questionType == "hoy")
        {
            return $"{questionType} {question}";
        }
        return "perdon... no entendi que quisiste preguntarme";
    }
    public IEnumerator SearchUnknown(string unknown)
    {
        UnityWebRequest webInfo = UnityWebRequest.Get($"https://en.wikipedia.org/w/api.php?action=opensearch&format=json&search={unknown}&limit=1");
        yield return webInfo.SendWebRequest();
        string responseToComa = "";
        foreach (var character in webInfo.downloadHandler.text)
        {
            if(character != '[' && character != ']' && character != '{' && character != '}' && character != '"'){
                responseToComa += character;
            }
        }
        string[] finalResponse = responseToComa.Split(",");
        Debug.Log(finalResponse[1]);
    }
}
