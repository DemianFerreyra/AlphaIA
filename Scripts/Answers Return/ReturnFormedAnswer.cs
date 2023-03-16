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

        if (splitedStructure[0] == "greetingRandom")
        {
            if (splitedStructure.Length > 1 && splitedStructure[1] == "user")
            {
                responses.Add(GetRandomOption("greetings", "saludoshaciami"));
            }
            else if (splitedStructure.Length > 1 && splitedStructure[1] == "unknown")
            {
                responses.Add(GetRandomOption("greetings", "saludoshaciadesconocido"));
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
}