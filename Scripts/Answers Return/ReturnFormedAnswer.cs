using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ReturnFormedAnswer : MonoBehaviour
{
    public List<string> responses = new List<string>();
    public Answers intent;

    public void ReadStructure(string structure)
    {
        //la estructura es algo como  ==  greetingRandom:user       (el primer texto SIEMPRE sera un archivo json al que acceder y del segundo en adelante seran datos extra)
        //se partira en base a los : con un Split y se analizara ese texto
        string[] splitedStructure = structure.Split(":");
        string responseToAdd = "";
        Debug.Log(splitedStructure[0]);
        Debug.Log(structure);

        if (splitedStructure[0] == "jerga" && splitedStructure.Length > 1)
        {
            responses.Add(GetRandomOptionBasedOnKeys("jergas", splitedStructure[1]));
        }
        else if (splitedStructure[0] == "greeting")
        {
            if (splitedStructure.Length > 2 && splitedStructure[2] == "user")
            {
                responseToAdd = GetRandomOption("greetings", splitedStructure[1]);
                responseToAdd += " " + GetRandomOption("greetings", $"{splitedStructure[1]}HaciaUsuario");
                responses.Add(responseToAdd);
            }
            else if (splitedStructure.Length > 2 && splitedStructure[2] == "unknown")
            {
                responseToAdd = GetRandomOption("greetings", splitedStructure[1]);
                responseToAdd += " " + GetRandomOption("greetings", $"desconocido");
                responses.Add(responseToAdd);
            }
            else if (splitedStructure.Length == 2)
            {
                float getBool = Random.Range(0, 2);
                if (getBool == 0)
                {
                    responses.Add(GetRandomOption("greetings", splitedStructure[1]));
                }
                else
                {
                    responseToAdd = GetRandomOption("greetings", splitedStructure[1]);
                    responseToAdd += " " + GetRandomOption("greetings", $"{splitedStructure[1]}HaciaUsuario");
                    responses.Add(responseToAdd);
                }
            }
        }
        else if (splitedStructure[0] == "likesQuestion")
        {
            if (splitedStructure.Length > 1)
            {
                responseToAdd = GetLikesOrNot("alpha", splitedStructure[1]);
                responses.Add(responseToAdd);
            }
        }
    }




    private string GetRandomOption(string fileToOpen, string specificIntent)
    {
        try
        {
            intent = JsonUtility.FromJson<Answers>(File.ReadAllText(Application.dataPath + $"/Data/Common Answers/{fileToOpen}.json"));
            if (intent != null)
            {
                foreach (var answer in intent.answers)
                {
                    if (answer.specificIntent == specificIntent)
                    {
                        return answer.options[Random.Range(0, answer.options.Count)];
                    }
                }
            }
            else
            {
                intent = JsonUtility.FromJson<Answers>(File.ReadAllText(Application.dataPath + $"/Data/Common Answers/unableToAnswer.json"));
                if (intent != null)
                {
                    return intent.answers[0].options[Random.Range(0, intent.answers[0].options.Count)];
                }
            }
            return "wow, parece que lograron romperme... no se me ocurrio una forma de responder a esa pregunta... levantare un informe a Demian para que vea que sucedio";
        }
        catch (System.Exception)
        {
            return "wow, parece que lograron romperme... no se me ocurrio una forma de responder a esa pregunta... levantare un informe a Demian para que vea que sucedio";
            throw;
        }
    }
    private string GetRandomOptionBasedOnKeys(string fileToOpen, string specificKey)
    {
        try
        {
            intent = JsonUtility.FromJson<Answers>(File.ReadAllText(Application.dataPath + $"/Data/Common Answers/{fileToOpen}.json"));
            if (intent != null)
            {
                foreach (var answer in intent.answers)
                {
                    if (answer.keys.Contains(specificKey))
                    {
                        return answer.options[Random.Range(0, answer.options.Count)];
                    }
                }
            }
            else
            {
                intent = JsonUtility.FromJson<Answers>(File.ReadAllText(Application.dataPath + $"/Data/Common Answers/unableToAnswer.json"));
                if (intent != null)
                {
                    return intent.answers[0].options[Random.Range(0, intent.answers[0].options.Count)];
                }
            }
            return "wow, parece que lograron romperme... no se me ocurrio una forma de responder a esa pregunta... levantare un informe a Demian para que vea que sucedio";
        }
        catch (System.Exception)
        {
            return "wow, parece que lograron romperme... no se me ocurrio una forma de responder a esa pregunta... levantare un informe a Demian para que vea que sucedio";
            throw;
        }
    }
    private string GetLikesOrNot(string objective, string likesOrNot)
    {
        try
        {
            intent = JsonUtility.FromJson<Answers>(File.ReadAllText(Application.dataPath + $"/Data/Common Answers/{objective}.json"));
            if (intent != null)
            {
                List<string> closests = new List<string>();
                Answer extraIntent = null;
                foreach (var answer in intent.answers)
                {
                    if (answer.specificIntent == "likes" || answer.specificIntent == "doesntLikes")
                    {
                        foreach (var option in answer.options)
                        {
                            if (findSimilarity(option, likesOrNot) > 0.5)
                            {
                                if (answer.specificIntent == "likes")
                                {
                                    closests.Add($"{option}:{findSimilarity(option, likesOrNot)}:likes");
                                }
                                else if (answer.specificIntent == "doesntLikes")
                                {
                                    closests.Add($"{option}:{findSimilarity(option, likesOrNot)}:doesntLikes");
                                }
                            }
                        }
                    }
                    string topOption = "";
                    string topLikesOrNot = "";
                    float topValue = 0;
                    foreach (var data in closests)
                    {
                        string[] split = data.Split(":");
                        if (float.Parse(split[1]) > topValue)
                        {
                            topValue = float.Parse(split[1]);
                            topOption = split[0];
                            topLikesOrNot = split[2];
                        }
                    }
                    if (topLikesOrNot == "likes")
                    {
                        extraIntent = intent.answers.Find(aws => aws.specificIntent == "likesQuestions");
                        string finalAnswer = extraIntent.options[Random.Range(0, extraIntent.options.Count)];
                        return finalAnswer.Replace("{likesAlpha}", topOption);
                    }
                    else if (topLikesOrNot == "doesntLikes")
                    {
                        extraIntent = intent.answers.Find(aws => aws.specificIntent == "doesntLikesQuestions");
                        string finalAnswer = extraIntent.options[Random.Range(0, extraIntent.options.Count)];
                        return finalAnswer.Replace("{doesntLikesAlpha}", topOption);
                    }
                }
                extraIntent = intent.answers.Find(aws => aws.specificIntent == "unknownIfLikes");
                string unknownAnswer = extraIntent.options[Random.Range(0, extraIntent.options.Count)];
                return unknownAnswer.Replace("{likesAlpha}", likesOrNot);
            }
            return "wow, parece que lograron romperme... no se me ocurrio una forma de responder a esa pregunta... levantare un informe a Demian para que vea que sucedio";
        }
        catch (System.Exception)
        {
            return "wow, parece que lograron romperme... no se me ocurrio una forma de responder a esa pregunta... levantare un informe a Demian para que vea que sucedio";
            throw;
        }
    }

    public static int getEditDistance(string X, string Y)
    {
        int m = X.Length;
        int n = Y.Length;

        int[][] T = new int[m + 1][];
        for (int i = 0; i < m + 1; ++i)
        {
            T[i] = new int[n + 1];
        }

        for (int i = 1; i <= m; i++)
        {
            T[i][0] = i;
        }
        for (int j = 1; j <= n; j++)
        {
            T[0][j] = j;
        }

        int cost;
        for (int i = 1; i <= m; i++)
        {
            for (int j = 1; j <= n; j++)
            {
                cost = X[i - 1] == Y[j - 1] ? 0 : 1;
                T[i][j] = Mathf.Min(Mathf.Min(T[i - 1][j] + 1, T[i][j - 1] + 1),
                        T[i - 1][j - 1] + cost);
            }
        }

        return T[m][n];
    }

    public static double findSimilarity(string x, string y)
    {
        if (x == null || y == null)
        {
            return 0;
        }

        double maxLength = Mathf.Max(x.Length, y.Length);
        if (maxLength > 0)
        {
            // optionally ignore case if needed
            return (maxLength - getEditDistance(x, y)) / maxLength;
        }
        return 1.0;
    }
}