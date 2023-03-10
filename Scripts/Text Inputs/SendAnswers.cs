using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SendAnswers : MonoBehaviour
{
    public DatabaseManagment databaseManager;
    public TMP_InputField intent;
    public TMP_InputField specificIntent;
    public TMP_InputField keys;
    public TMP_InputField answers;
    public void SendAnswer()
    {
        string[] _answers = answers.text.Split(",");
        string[] _keys = keys.text.Split(",");
        foreach (var _answer in _answers)
        {
            databaseManager.answers.Find(answer => answer.intent == intent.text).answers.Find(answer => answer.specificIntent == specificIntent.text).options.Add(_answer);
        }
        foreach (var _key in _keys)
        {
            databaseManager.answers.Find(answer => answer.intent == intent.text).answers.Find(answer => answer.specificIntent == specificIntent.text).keys.Add(_key);
        }
        specificIntent.text = "";
        keys.text = "";
        intent.text = "";
        answers.text = "";
    }
}