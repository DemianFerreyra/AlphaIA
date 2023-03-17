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
        if (splitedStructure[0] == "greeting")
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
}