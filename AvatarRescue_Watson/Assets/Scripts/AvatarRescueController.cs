using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IBM.Watsson.Examples;

[System.Serializable]
public class ConersationPoint
{
    public string triggerStatement = "";
    public string statementToChatbot = "";
    public float speechTime = 4f;
    public bool hasFiredOff = false;
}
[System.Serializable]
public class PopUpImage
{
    public Animator anim;
    public int elementToPop;
}
public class AvatarRescueController : MonoBehaviour
{
    bool imageLastTalkingPoint = false;
    public ExampleStreaming stt;
    public AvatarRescue_WatsonInterface assistant;
    public TextToSpeech tts;

    public ConersationPoint[] conversations;
    int counter = 0;

    public PopUpImage[] popUps;
    int popUpCounter = 0;

    void Start()
    {
        StartCoroutine(StartAvatar());
    }
    
    void Update()
    {
        if (counter < conversations.Length)
        {
            if (stt.GetRecordedText().Contains(conversations[counter].triggerStatement) && !conversations[counter].hasFiredOff)
            {
                conversations[counter].hasFiredOff = true;
                StartCoroutine(SendToAvatar());
                if(imageLastTalkingPoint)
                {
                    imageLastTalkingPoint = false;
                    if (popUpCounter > 0)
                    {
                        popUps[popUpCounter - 1].anim.Play("Close");
                    }
                }
                if (popUpCounter < popUps.Length)
                {
                    if (popUps[popUpCounter].elementToPop == counter)
                    {
                        imageLastTalkingPoint = true;
                        popUps[popUpCounter].anim.Play("Open");
                        popUpCounter++;
                    }
                }
            }
        }
    }
    IEnumerator StartAvatar()
    {
        yield return new WaitForSeconds(5);
        tts.SayThisText(assistant.GetChatResponse());
        stt.StartRecording();
    }
    IEnumerator SendToAvatar()
    {
        stt.StopRecording();
        yield return new WaitForSeconds(.5f);
        assistant.SendToChatbot(conversations[counter].statementToChatbot);
        yield return new WaitForSeconds(1);
        tts.SayThisText(assistant.GetChatResponse());
        yield return new WaitForSeconds(conversations[counter].speechTime);
        if (counter < conversations.Length)
        {
            counter++;
            stt.StartRecording();
        }

    }
}
