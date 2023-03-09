using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewWord : MonoBehaviour
{
    public DatabaseManagment databaseManager;
    public TMP_InputField word;
    public TMP_InputField wordTypes;
    public TMP_InputField wordUses;
    public void DiscoverWord()
    {
        int suma = 0;
        foreach (var character in word.text)
        {
            suma += System.Convert.ToInt32(character);
        }
        string[] types = wordTypes.text.Split(",");
        string[] uses = wordUses.text.Split(",");
        databaseManager.dictionary.codes[suma].words.Add(new Word(word.text, types, uses));

        word.text = "";
        wordTypes.text = "";
        wordUses.text = "";
    }
}
