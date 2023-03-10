using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewWord : MonoBehaviour
{
    public DatabaseManagment databaseManager;
    public TMP_InputField word;
    public TMP_InputField wordTypes;
    public TMP_InputField answerType;
    public void DiscoverWord()
    {
        int suma = 0;
        foreach (var character in word.text)
        {
            suma += System.Convert.ToInt32(character) % 4000;
        }
        string[] types = wordTypes.text.Split(",");
        databaseManager.dictionary.codes[suma].words.Add(new Word(word.text, types, answerType.text));
        word.text = "";
        wordTypes.text = "";
        answerType.text = "";
    }
}
