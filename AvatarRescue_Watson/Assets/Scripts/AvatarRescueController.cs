using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum INPUT_STATE
{
    TALK,
    TYPE,
    MULTIPLE_CHOICE,
    INIT
};
public class AvatarRescueController : MonoBehaviour
{
    public INPUT_STATE input_state = INPUT_STATE.INIT;
    public SpeechToText stt;
    public Assistant assistant;
    public TextToSpeech tts;
    
    public Text typeWatsonResponse, talkWatsonResponse;
    public RawImage typeWatsonMedia, talkWatsonMedia;
    public Animator settingsPanel, typePanel, talkPanel, multipleChoicePanel;

    void SetInputMode(INPUT_STATE state)
    {
        settingsPanel.Play("Close");

        input_state = state;

        switch(input_state)
        {
            case INPUT_STATE.TALK:
                talkPanel.gameObject.SetActive(true);
                assistant.chatResponse = talkWatsonResponse;
                assistant.watsonMediaViewer = talkWatsonMedia;
                talkWatsonResponse.text = assistant.chat_response;
                talkPanel.Play("Open");
                break;
            case INPUT_STATE.TYPE:
                typePanel.gameObject.SetActive(true);
                assistant.chatResponse = typeWatsonResponse;
                assistant.watsonMediaViewer = typeWatsonMedia;
                typeWatsonResponse.text = assistant.chat_response;
                typePanel.Play("Open");
                break;
            case INPUT_STATE.MULTIPLE_CHOICE:
                multipleChoicePanel.gameObject.SetActive(true);
                multipleChoicePanel.Play("Open");
                break;
        }
    }
    IEnumerator DisablePanel(GameObject panel)
    {
        yield return new WaitForSeconds(.25f);
        panel.SetActive(false);
    }
    public void SetInputModeTalk()
    {
        SetInputMode(INPUT_STATE.TALK);
    }
    public void SetInputModeType()
    {
        SetInputMode(INPUT_STATE.TYPE);
    }
    public void SetInputModeMultipleChoice()
    {
        SetInputMode(INPUT_STATE.MULTIPLE_CHOICE);
    }
    public void CloseSettings()
    {
        SetInputMode(input_state);
    }
    public void TurnOffPanel(GameObject panel)
    {
        StartCoroutine("DisablePanel", panel);
    }
    public void StartRecording()
    {
        stt.StartRecording();
    }
    public void StopRecording()
    {
        string recorded = stt.GetRecordedText();
        stt.StopRecording();
        assistant.SendToChatbot(recorded);
    }
    public void SendMessageToChatbot(string message)
    {
        assistant.SendToChatbot(message);
    }

}
