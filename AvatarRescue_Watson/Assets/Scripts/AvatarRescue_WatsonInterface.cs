
using System.Collections;
using IBM.Cloud.SDK;
using IBM.Cloud.SDK.Utilities;
using IBM.Watson.Assistant.V2;
using IBM.Watson.Assistant.V2.Model;
using UnityEngine;
using UnityEngine.UI;


public class AvatarRescue_WatsonInterface : MonoBehaviour
{
    private string assistant_iamApikey = "8icRS1gSvMJ7bzg16Pq8C7kfyXopK2_xY2NQgTfp0JL7";
    private string assistant_serviceUrl = "https://gateway.watsonplatform.net/assistant/api";
    private string assistant_versionDate = "2018-11-08";
    private string assistant_assistantId = "028334ed-0283-4f1c-845d-47abd7f370b9";
    
    string stt_iamApikey = "p2xqJ_BNXaRssiDTt1MASxpSV4SYeUoefLd4oo7Wq7Kt";
    string stt_serviceUrl = "https://stream.watsonplatform.net/speech-to-text/api";
    

    public Text chatResponse;
    public string currentStringOut = "";
    public string chat_response;

    private AssistantService service;

    private bool createSessionTested = false;
    public bool messageReturned = false;
    private bool deleteSessionTested = false;
    private string sessionId;

    private void Start()
    {
        LogSystem.InstallDefaultReactors();
        Runnable.Run(CreateService());
    }
    void OnApplicationQuit()
    {
        Runnable.Run(End());
    }
    private IEnumerator CreateService()
    {
        if (string.IsNullOrEmpty(assistant_iamApikey))
        {
            throw new IBMException("Plesae provide IAM ApiKey for the service.");
        }

        //  Create credential and instantiate service
        Credentials credentials = null;

        //  Authenticate using iamApikey
        TokenOptions tokenOptions = new TokenOptions()
        {
            IamApiKey = assistant_iamApikey
        };

        credentials = new Credentials(tokenOptions, assistant_serviceUrl);

        //  Wait for tokendata
        while (!credentials.HasIamTokenData())
            yield return null;

        service = new AssistantService(assistant_versionDate, credentials);

        Log.Debug("ExampleAssistantV2.RunTest()", "Attempting to CreateSession");
        service.CreateSession(OnCreateSession, assistant_assistantId);

        while (!createSessionTested)
        {
            yield return null;
        }

        Log.Debug("ExampleAssistantV2.RunTest()", "Attempting to Message");
        service.Message(OnDisplayResponse, assistant_assistantId, sessionId);

        while (!messageReturned)
        {
            yield return null;
        }
    }
    public void SendToChatbot(string text)
    {
        currentStringOut = text;
        Runnable.Run(SendChatOut());
    }
    public void EndSession()
    {
        Runnable.Run(End());
    }
    private IEnumerator SendChatOut()
    {
        messageReturned = false;
        Log.Debug("Sending out: ", currentStringOut);

        MessageInput mInput = new MessageInput()
        {
            Text = currentStringOut,
            Options = new MessageInputOptions()
            {
                ReturnContext = true
            }
        };

        service.Message(OnDisplayResponse, assistant_assistantId, sessionId, input: mInput);
        while (!messageReturned)
        {
            yield return null;
        }
        currentStringOut = "";
    }
    IEnumerator End()
    {
        Log.Debug("ExampleAssistantV2.RunTest()", "Attempting to delete session");
        service.DeleteSession(OnDeleteSession, assistant_assistantId, sessionId);

        while (!deleteSessionTested)
        {
            yield return null;
        }

        Log.Debug("ExampleAssistantV2.Examples()", "Assistant examples complete.");
    }

    private void OnDisplayResponse(DetailedResponse<MessageResponse> response, IBMError error)
    {
        if (response.Result.Output.Generic.Count > 0)
        {
            Log.Debug("ExampleAssistantV2.OnMessage0()", "response: {0}", response.Result.Output.Generic[0].Text);
            messageReturned = true;
            chatResponse.text = chat_response = response.Result.Output.Generic[0].Text;
        }
    }
    public string GetChatResponse()
    {
        return chat_response;
    }
    
    private void OnDeleteSession(DetailedResponse<object> response, IBMError error)
    {
        Log.Debug("ExampleAssistantV2.OnDeleteSession()", "Session deleted.");
        deleteSessionTested = true;
    }
    
    private void OnCreateSession(DetailedResponse<SessionResponse> response, IBMError error)
    {
        Log.Debug("ExampleAssistantV2.OnCreateSession()", "Session: {0}", response.Result.SessionId);
        sessionId = response.Result.SessionId;
        createSessionTested = true;
    }
}
