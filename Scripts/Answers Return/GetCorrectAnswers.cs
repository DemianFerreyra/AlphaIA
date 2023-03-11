using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
public class GetCorrectAnswers : MonoBehaviour
{
    public Answers intent;
    public MessageRecongnition msgReader;
    public DatabaseManagment dataBaseManagment;
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
    public string ReturnAnswerToQuestion(string question, string questionType, string objective)
    {
        intent = JsonUtility.FromJson<Answers>(File.ReadAllText(Application.dataPath + $"/Data/Common Answers/{objective}s.json"));
        if (questionType == "personalState")
        {
            return "estoy bien";
        }
        if (questionType == "like")
        {
            foreach (var answer in intent.answers)
            {
                if (answer.options.Contains(question.ToLower()))
                {
                    string msg2 = intent.answers.Find(intent => intent.specificIntent == "knownLikes").options[Random.Range(0, intent.answers.Find(intent => intent.specificIntent == "knownLikes").options.Count)];
                    string newmsg2 = msg2.Replace("{unknownIfLike}", question);
                    return newmsg2;
                }
            }
            string msg = intent.answers.Find(intent => intent.specificIntent == "unknownLikes").options[Random.Range(0, intent.answers.Find(intent => intent.specificIntent == "knownLikes").options.Count)];
            string newmsg = msg.Replace("{unknownIfLike}", question);
            return newmsg;
        }
        if (questionType == "tomorrow" || questionType == "yesterday" || questionType == "today")
        {
            return $"{questionType} {question}";
        }
        return "perdon... no entendi que quisiste preguntarme";
    }
    public IEnumerator SearchUnknown(string unknown, bool isForCreator)
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

        if (isForCreator == false)
        {
            if (msgReader.databaseManager.dictionary.codes[suma % 4000].likes.Count == 0)
            {
                string msg = ReturnCorrectAnswer("unknown", "gustosdesconocidos");
                string newmsg = msg.Replace("#", finalResponse[1].ToLower());
                finalResponse[1] = newmsg;
            }
            for (int i = 0; i < msgReader.databaseManager.dictionary.codes[suma % 4000].likes.Count; i++)
            {
                Like like = msgReader.databaseManager.dictionary.codes[suma % 4000].likes[i];
                if (like.name == unknown)
                {
                    if (like.likesOrNot == true)
                    {
                        string msg = ReturnCorrectAnswer("question", "gustosconocidospositivo");
                        string newmsg = msg.Replace("#", finalResponse[1].ToLower());
                        finalResponse[1] = newmsg;
                    }
                    else
                    {
                        string msg = ReturnCorrectAnswer("question", "gustosdesconocidosnegativo");
                        string newmsg = msg.Replace("#", finalResponse[1].ToLower());
                        finalResponse[1] = newmsg;
                    }
                }
                else
                {
                    string msg = ReturnCorrectAnswer("unknown", "gustosdesconocidos");
                    string newmsg = msg.Replace("#", finalResponse[1].ToLower());
                    finalResponse[1] = newmsg;
                }
            }
        }
        else
        {
            string msg = ReturnAnswerToQuestion(finalResponse[1].ToLower(), "like", "demian");
            finalResponse[1] = msg;
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

    //uri para conseguir data mas profunda $"https://en.wikipedia.org/w/api.php?action=query&prop=extracts&format=json&exintro=&titles={unknown}"
}
