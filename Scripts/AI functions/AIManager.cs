using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class AIManager : MonoBehaviour
{
    public bool isReadyToAnswer = true;
    public GetCorrectAnswers getCorrectAnswers;

    void Update()
    {

    }

    public void ReadMessageLoud(string message)
    {
        Debug.Log("mensaje completo = " + GetMessageReplaced(message));
    }
    string GetMessageReplaced(string message)
    {
        string[] creatorNames = { "Demian", "Demi", "mi creador" };
        StringBuilder sb = new StringBuilder(message);

        sb.Replace("{videogameLikesDemi}", getCorrectAnswers.ReturnCorrectAnswer("demian", "videogameLikes"));
        sb.Replace("{likesDemi}", getCorrectAnswers.ReturnCorrectAnswer("demian", "likes"));
        sb.Replace("{usuario}", "usuario");
        sb.Replace("{creator}", creatorNames[Random.Range(0, 2)]);

        return sb.ToString();
    }
}
