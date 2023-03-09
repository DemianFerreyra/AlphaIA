using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SendMessageTest : MonoBehaviour
{
    public MessageRecongnition messageRecognition;
    public TMP_InputField input;
    public void SendMessage()
    {
        messageRecognition.ReadMessage(input.text);
    }
}
