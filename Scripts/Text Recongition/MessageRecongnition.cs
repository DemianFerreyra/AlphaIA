using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MessageRecongnition : MonoBehaviour
{
    public DatabaseManagment databaseManager;
    public AnswersCases answerManager;
    public List<string> intents = new List<string>();

    void Start()
    {
        string[] _intents = File.ReadAllText(Application.dataPath + "/Data/intents.txt").Split(",");
        foreach (var intent in _intents)
        {
            intents.Add(intent);
        }
    }
    public void ReadMessage(string message)
    {
        //supongamos caso "hola como estas"
        //primero se dividira el texto por palabras  -->  [hola, como, estas];
        string[] _words = message.Split(" ");
        //luego se invocara la funcion ReadWord() que leera cada palabra y buscara en un archivo json lo que significa

        List<string> responses = new List<string>();
        foreach (var word in _words)
        {
            responses.Add(ReadWord(word));
        }
        answerManager.latestWord = "";
        answerManager.currentWordOrder = 0;
    }
    private string ReadWord(string word)
    {
        //las palabras se guardaran en un json usando hash tables para reducir la complejidad algoritmica a la hora de buscar la palabra concreta. En el archivo json cada palabra estara guardada con su hash (por ejemplo hola => 104 111 108 97 => 420 lo cual dividido por la cantidad de entradas que tendra la tabla, nos da su indice)
        //luego una vez se acceda a esa palabra, se leera su informacion (por ejemplo, "hola" sera un objeto con informacion que diga que es un saludo, si puede ser una pregunta, si puede ser un verbo, etc) y en base a eso se sacaran las respuestas correspondientes
        int suma = 0;
        foreach (var character in word)
        {
            suma += System.Convert.ToInt32(character);
        }
        if (databaseManager.dictionary.codes[suma].words.Find(_word => _word.word == word) != null)
        {
            Word currentWord = databaseManager.dictionary.codes[suma].words.Find(_word => _word.word == word);
            string answer = answerManager.GetAnswer(currentWord);
            Debug.Log(answer);
            return answer;

            // foreach (var intent in currentWord.wordTypes)
            // {
            //     Answers currentAnswer = databaseManager.answers.Find(answer => answer.intent == intent);
            //     if(intents.Contains(intent) && currentAnswer.options.Count > 1){
            //         Debug.Log(currentAnswer.options[Random.Range(1, currentAnswer.options.Count)]);
            //         return currentAnswer.options[Random.Range(1, currentAnswer.options.Count)];
            //     }
            // }
            // return "unknownAnswer";
        }
        else
        {
            string[] unknown = new string[1];
            unknown[0] = "unknown";
            databaseManager.AddNewWordsToDiscover(word, suma);

            string answer = answerManager.GetAnswer(new Word(word, unknown, "unknown"));
            Debug.Log(answer);
            return answer;
        }
    }
}
