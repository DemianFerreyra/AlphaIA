using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public bool isReadyToAnswer = true;

    void Update()
    {

    }

    public void ReadMessageLoud(string message){
      string newmsg = message.Replace("{usuario}", "usuario");
      Debug.Log("mensaje completo = " + newmsg);
    }
}
