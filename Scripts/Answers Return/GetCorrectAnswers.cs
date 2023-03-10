using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GetCorrectAnswers : MonoBehaviour
{
    public Answers intent;
    public string ReturnCorrectAnswer(string wordType, string word)
    {
        intent = JsonUtility.FromJson<Answers>(File.ReadAllText(Application.dataPath + $"/Data/Common Answers/{wordType}s.json"));
        foreach (var answer in intent.answers)
        {
            if(answer.keys.Contains(word)){
              return answer.options[Random.Range(0, answer.options.Count)];
            }
        }
        return "ignore";
    }
}
