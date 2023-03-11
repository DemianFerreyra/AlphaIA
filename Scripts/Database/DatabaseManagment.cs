using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


[System.Serializable]
public class ASCIIcode
{
    public List<Word> words;
    public List<Like> likes;
}
[System.Serializable]
public class Word
{
    public string word;
    public List<string> wordTypes = new List<string>(); //greeting,question,action,adjective,past verb,conjunction,second person,affirmation,place
    public string answerType; //si requiere algo especifico (como por ejemplo en requiere un lugar como siguiente palabra)
    public Word(string _word, string[] _wordTypes, string _answerType)
    {
        word = _word;
        foreach (var wordType in _wordTypes)
        {
            wordTypes.Add(wordType);
        }
        answerType = _answerType;
    }
}
[System.Serializable]
public class Answers
{
    public string intent;
    public List<Answer> answers = new List<Answer>();
}
[System.Serializable]
public class Answer
{
    public string specificIntent;
    public List<string> keys = new List<string>();
    public List<string> options = new List<string>();
    public Answer(string[] _keys, string[] _options, string _specificIntent)
    {
        specificIntent = _specificIntent;
        foreach (var key in _keys)
        {
            keys.Add(key);
        }
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
[System.Serializable]
public class Like
{
    public string name;
    public bool likesOrNot;
    public Like(string _name, bool _likesOrNot){
        name = _name;
        likesOrNot = _likesOrNot;
    }
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
        foreach (var intent in _intents)
        {
            answers.Add(JsonUtility.FromJson<Answers>(File.ReadAllText(Application.dataPath + $"/Data/Common Answers/{intent}s.json")));
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
        string[] _intents = File.ReadAllText(Application.dataPath + "/Data/intents.txt").Split(",");
        foreach (var intent in _intents)
        {
            UpdateAnswersDictionary(intent);
        }
    }
    private void UpdateAnswersDictionary(string intent)
    {
        string updatedArray = JsonUtility.ToJson(answers.Find(answer => answer.intent == intent));
        File.WriteAllText(Application.dataPath + $"/Data/Common Answers/{intent}s.json", updatedArray);
    }

    public void AddNewWordsToDiscover(string word, int hashValue)
    {
        string[] newWord = new string[1];
        newWord[0] = $"word:{word}, hash:{hashValue}";
        File.AppendAllLines(Application.dataPath + "/Data/NewWords.txt", newWord);
    }
}
