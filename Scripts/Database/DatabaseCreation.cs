using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


[System.Serializable]
public class ASCIIcode
{
    public List<Word> words;
}
[System.Serializable]
public class Word
{
    public string word;
    public string wordType; //verb, sustantive, adjective
    public List<string> wordUses = new List<string>(); //arreglo de usos de esa palabra, como por ejemplo, saludo, insulto, nombre, pregunta, etc (es un arreglo debido a que una palabra puede tener varios significados)
    public Word(string _word, string _wordType, string[] _wordUses)
    {
        word = _word;
        wordType = _wordType;
        foreach (var wordUse in _wordUses)
        {
            wordUses.Add(wordUse);
        }
    }
}
[System.Serializable]
public class Words
{
    public List<ASCIIcode> codes;
}
public class DatabaseCreation : MonoBehaviour
{
    public TextAsset dictionary;
    public void CreateDictionary()
    {
        Words newDictionary = new Words();
        //esta funcion creara la base de datos del diccionario de palabras agregandole su largo correspondiente
        for (int i = 0; i < 10000; i++)
        {
            newDictionary.codes.Add(new ASCIIcode());
        }
        string emptyArray = JsonUtility.ToJson(newDictionary);
        File.WriteAllText(Application.dataPath + "/Data/Dictionary.json", emptyArray);
    }
    // public void UpdateDictionary(Words updatedDictionary)
    // {
    //     string updatedArray = JsonUtility.ToJson(updatedDictionary);
    //     File.WriteAllText(Application.dataPath + "/Data/Dictionary.json", updatedArray);
    // }

    public void AddNewWordsToDiscover(string word, int hashValue)
    {
        string [] newWord = new string[1];
        newWord[0] = $"word:{word}, hash:{hashValue}";
        File.AppendAllLines(Application.dataPath + "/Data/NewWords.txt", newWord);
    }
}
