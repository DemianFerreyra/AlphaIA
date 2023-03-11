using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
public class GetCorrectAnswers : MonoBehaviour
{
    public Answers intent;
    public MessageRecongnition msgReader;
    public string ReturnCorrectAnswer(string wordType, string word)
    {
        intent = JsonUtility.FromJson<Answers>(File.ReadAllText(Application.dataPath + $"/Data/Common Answers/{wordType}s.json"));
        foreach (var answer in intent.answers)
        {
            if (answer.specificIntent == word)
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
        if (questionType == "like")
        {
            return $"si, me gusta {question}";
        }
        if (questionType == "tomorrow" || questionType == "yesterday" || questionType == "today")
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
            if (character != '[' && character != ']' && character != '{' && character != '}' && character != '"')
            {
                responseToComa += character;
            }
        }
        string[] finalResponse = responseToComa.Split(",");
        int suma = 0;
        foreach (var character in finalResponse[1])
        {
            suma += System.Convert.ToInt32(character);
        }

        if (msgReader.databaseManager.dictionary.codes[suma % 4000].likes.Count == 0)
        {
            string msg = ReturnCorrectAnswer("unknown", "gustosdesconocidos");
            string newmsg = msg.Replace("#", finalResponse[1].ToLower());
            Debug.Log(newmsg);
        }
        for (int i = 0; i < msgReader.databaseManager.dictionary.codes[suma % 4000].likes.Count; i++)
        {
            Like like = msgReader.databaseManager.dictionary.codes[suma % 4000].likes[i];
            if (like.name == unknown)
            {
                if (like.likesOrNot == true)
                {
                    Debug.Log("a alpha si le gusta " + unknown);
                }
                else
                {
                    Debug.Log("a alpha no le gusta " + unknown);
                }
            }
            else
            {
                string msg = ReturnCorrectAnswer("unknown", "gustosdesconocidos");
                string newmsg = msg.Replace("#", finalResponse[1].ToLower());
                Debug.Log(newmsg);
            }
        }
        msgReader.leftToAnswer -= 1;
        for (int i = 0; i < msgReader.responsesToGive.Count; i++)
        {
            if (msgReader.responsesToGive[i] == "await")
            {
                msgReader.responsesToGive[i] = finalResponse[1];
            }
        }
    }
}
