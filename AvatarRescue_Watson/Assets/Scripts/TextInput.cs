using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextInput : MonoBehaviour
{
    public Text output;
    public string text = "";
    TouchScreenKeyboard keyboard;
    public AvatarRescueController controller;
    // Start is called before the first frame update
    void OnEnable()
    {
        keyboard = TouchScreenKeyboard.Open(text, TouchScreenKeyboardType.Default);
        StartCoroutine("EnableKeyboard");
    }
    private void OnDisable()
    {
        StopCoroutine("EnableKeyboard");
    }
    void Update()
    {
        if (keyboard != null && keyboard.status == TouchScreenKeyboard.Status.Done)
        {
            text = keyboard.text;
            output.text = text;
            if (text != "")
            {
                controller.SendMessageToChatbot(text);
                text = "";
                keyboard = TouchScreenKeyboard.Open(text, TouchScreenKeyboardType.Default);
            }
        }
    }
    public void DisableKeyboard()
    {
        StopCoroutine("EnableKeyboard");
        keyboard.active = false;
    }
    IEnumerator EnableKeyboard()
    {
        while (true)
        {
            yield return new WaitForSeconds(.5f);
            if (keyboard == null || keyboard.status != TouchScreenKeyboard.Status.Visible)
            {
                keyboard = TouchScreenKeyboard.Open(text, TouchScreenKeyboardType.Default);
            }
        }
    }
}
