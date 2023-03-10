using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GetCorrectAnswers : MonoBehaviour
{
    public Answers intent;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ReturnCorrectAnswer("greeting");
        }
    }
    public string ReturnCorrectAnswer(string wordType)
    {
        intent = JsonUtility.FromJson<Answers>(File.ReadAllText(Application.dataPath + $"/Data/Common Answers/{wordType}s.json"));
        if (intent.options.Count > 1)
        {
            return intent.options[Random.Range(1, intent.options.Count)];
        }
        return "ignore";
    }
}
