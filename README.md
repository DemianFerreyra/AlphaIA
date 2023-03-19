# AlphaIA 
## Artificial inteligence project. Powered by Unity3D
##### 🛑 Status: Canceled🛑
&nbsp;

## Description
Alpha was an AI maded in unity3D using C# as language. Alpha had a dictionary with 4000 spaces for saving words, using hash tables to get easy and quick access to them whenever she needed.

### The way it works was:
- Get the message
- Read every word and look for it in the dictionary
- If the word exists, then she would take the type of the word and based on that she can recognize if it was used as a question, as a greeting, insult, etc.
- She repeated this proccess with every word and finally retrieve one or more answers with the following structure
  _{answerToGet:extraValues}
 example: "adios" would have returned {greeting:despedidas}
- With the answer/s returned, she would access a json file based on the answerToGet parameter (example, for "hola" she would open "greetings.json" file) and then retrieve a random answer based on what is in that file

### Features

- Manual addition of words to learn
- Multiple responses for same question
- Capacity to concat answers to give a more dynamic and large answer (this was random so some answers were short and some were large)

### Why it was canceled?
Alpha was canceled because in mid development i got a new idea to make it work better, be more scalable and easy to read and understand.
 The new method, that will be used in the next project "Beta" uses some of the mechanics of Alpha, so its not so different to Alpha but is different enough to be cataloged as a separate project.


## Tech

- [C#](https://learn.microsoft.com/es-es/dotnet/csharp/) - Programming language
- [Unity3D](https://unity.com/es) - 3D game development platform
- [Github](https://github.com) - Repository managment site

## Installation

Dillinger requires Unity3D 2021.3.19f to run. All scenes and code are included on this repo.

## License

MIT

**Free Software. Feel free to use it and upgrade if you think you can**
