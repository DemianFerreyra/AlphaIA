using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MessageRecongnition : MonoBehaviour
{
    public DatabaseManagment databaseManager;
    public AnswersCases answerManager;
    public GetCorrectAnswers getCorrectAnswers;
    public bool stillAnswering;
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
        string fullWord = "";
        foreach (var response in responses)
        {
            if (response == "ignore")
            {
                Debug.Log("ignore");
            }
            else
            {
                string[] responseData = response.Split(":");
                if (responseData[0] == "question" || responseData[0] == "likes")
                {
                    Debug.Log(getCorrectAnswers.ReturnAnswerToQuestion(responseData[2], responseData[1]));
                    if (responseData[1] == "unknown")
                    {
                        fullWord += $"{responseData[2]}";
                    }

                }
                else
                {
                    Debug.Log(getCorrectAnswers.ReturnCorrectAnswer(responseData[0], responseData[2]));
                }
            }
        }
        if (fullWord.Length > 1)
        {
            StartCoroutine(getCorrectAnswers.SearchUnknown(fullWord));
        }
        answerManager.latestWord = "";
        answerManager.latestWordType = "";
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
