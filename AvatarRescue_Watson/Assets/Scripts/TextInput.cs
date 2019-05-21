using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextInput : MonoBehaviour
{
    public string text = "";
    TouchScreenKeyboard keyboard;
    // Start is called before the first frame update
    void Start()
    {
        keyboard = TouchScreenKeyboard.Open(text, TouchScreenKeyboardType.Default);
    }
    void Update()
    {
        if (keyboard != null && keyboard.status == TouchScreenKeyboard.Status.Done)
        {
            text = keyboard.text;
            print("User input is: " + text);
        }
    }
}
