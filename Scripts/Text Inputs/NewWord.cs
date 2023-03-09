using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewWord : MonoBehaviour
{
    public DatabaseManagment databaseManager;
    public TMP_InputField word;
    public TMP_InputField wordTypes;
    public TMP_InputField wordRequire;
    public void DiscoverWord()
    {
        int suma = 0;
        foreach (var character in word.text)
        {
            suma += System.Convert.ToInt32(character);
        }
        string[] types = wordTypes.text.Split(",");
        databaseManager.dictionary.codes[suma].words.Add(new Word(word.text, types, wordRequire.text));

        word.text = "";
        wordTypes.text = "";
        wordRequire.text = "";
    }
}
