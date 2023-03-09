using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageRecongnition : MonoBehaviour
{
    public DatabaseManagment databaseManager;
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
            Debug.Log("la palabra existe");
            return "respuesta";
        }
        else
        {
            string[] uses = new string[1];
            uses[0] = "unknown";
            databaseManager.dictionary.codes[suma].words.Add(new Word(word, "unknown", uses));
            databaseManager.AddNewWordsToDiscover(word, suma);
            return "newWordUnlocked";
        }
    }
}
