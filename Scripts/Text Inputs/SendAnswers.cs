using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SendAnswers : MonoBehaviour
{
    public DatabaseManagment databaseManager;
    public TMP_InputField intent;
    public TMP_InputField answers;
    public void SendAnswer()
    {
        string[] _answers = answers.text.Split(",");
        foreach (var _answer in _answers)
        {
            databaseManager.answers.Find(answer => answer.intent == intent.text).options.Add(_answer);
        }

        intent.text = "";
        answers.text = "";
    }
}