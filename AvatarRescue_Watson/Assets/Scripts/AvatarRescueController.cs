using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum INPUT_STATE
{
    TALK,
    TYPE,
    MULTIPLE_CHOICE
};
public class AvatarRescueController : MonoBehaviour
{
    public INPUT_STATE input_state = INPUT_STATE.TYPE;
    public SpeechToText stt;
    public Assistant assistant;
    public TextToSpeech tts;
    public bool isTalking = false;
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

}
