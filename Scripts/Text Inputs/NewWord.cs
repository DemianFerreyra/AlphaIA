using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewWord : MonoBehaviour
{
    public DatabaseManagment databaseManager;
    public TMP_InputField word;
    public TMP_InputField wordTypes;
    public TMP_InputField answerType;
    public Toggle alphaLikes;
    public void DiscoverWord()
    {
        int suma = 0;
        foreach (var character in word.text.ToLower())
        {
            suma += System.Convert.ToInt32(character);
        }
        string[] types = wordTypes.text.Split(",");
        databaseManager.dictionary.codes[suma % 4000].words.Add(new Word(word.text.ToLower(), types, answerType.text.ToLower()));
        word.text = "";
        wordTypes.text = "";
        answerType.text = "";
    }
    public void NewLike()
    {
        int suma = 0;
        foreach (var character in word.text.ToLower())
        {
            suma += System.Convert.ToInt32(character);
        }
        databaseManager.dictionary.codes[suma % 4000].likes.Add(new Like(word.text.ToLower(), alphaLikes.isOn));
        word.text = "";
        alphaLikes.isOn = true;
    }
}
