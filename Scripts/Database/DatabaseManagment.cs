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
    public List<string> wordTypes = new List<string>(); //greeting,question,action,adjective,past verb,conjunction,second person,affirmation,place
    public string wordRequire; //si requiere algo especifico (como por ejemplo en requiere un lugar como siguiente palabra)
    public Word(string _word, string[] _wordTypes, string _wordRequire)
    {
        word = _word;
        foreach (var wordType in _wordTypes)
        {
            wordTypes.Add(wordType);
        }
        wordRequire = _wordRequire;
    }
}
[System.Serializable]
public class Answers
{
    public string intent;
    public List<string> options = new List<string>();
    public Answers(string _intent, string[] _options)
    {
        intent = _intent;
        foreach (var option in _options)
        {
            options.Add(option);
        }
    }
}
[System.Serializable]
public class Words
{
    public List<ASCIIcode> codes;
}
public class DatabaseManagment : MonoBehaviour
{
    public TextAsset textJSON;
    public Words dictionary;
    public List<Answers> answers = new List<Answers>();

    void Start()
    {
        dictionary = JsonUtility.FromJson<Words>(textJSON.text);
        string[] _intents = File.ReadAllText(Application.dataPath + "/Data/intents.txt").Split(",");
        string[] options = new string[1];
        foreach (var intent in _intents)
        {
            answers.Add(new Answers(intent, options));
        }
    }
    public void CreateDictionary()
    {
        string emptyArray = JsonUtility.ToJson(dictionary);
        File.WriteAllText(Application.dataPath + "/Data/Dictionary.json", emptyArray);
    }
    public void UpdateDictionary()
    {
        string updatedArray = JsonUtility.ToJson(dictionary);
        File.WriteAllText(Application.dataPath + "/Data/Dictionary.json", updatedArray);
    }

    public void AddNewWordsToDiscover(string word, int hashValue)
    {
        string[] newWord = new string[1];
        newWord[0] = $"word:{word}, hash:{hashValue}";
        File.AppendAllLines(Application.dataPath + "/Data/NewWords.txt", newWord);
    }
}
