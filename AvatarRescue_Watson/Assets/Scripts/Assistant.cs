
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IBM.Cloud.SDK;
using IBM.Cloud.SDK.Utilities;
using IBM.Watson.Assistant.V2;
using IBM.Watson.Assistant.V2.Model;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Video;

public class Assistant : MonoBehaviour
{
    public IBM_Service assistantService;
    string assistant_iamApikey;
    private string assistant_serviceUrl;
    private string assistant_versionDate = "2018-11-08";
    string assistant_id;// = "028334ed-0283-4f1c-845d-47abd7f370b9";  //Ksenia's : "028334ed-0283-4f1c-845d-47abd7f370b9"

    public Text chatResponse;
    public string currentStringOut = "";
    public string chat_response;
    public RawImage watsonMediaViewer;
    public bool imageWatsonDisplayed = false;

    string imageURL;
    string videoURL;

    private AssistantService service;

    private bool createSessionTested = false;
    public bool messageReturned = false;
    private bool deleteSessionTested = false;
    private string sessionId;

    private void Start()
    {
        assistant_iamApikey = assistantService.APIKey;
        assistant_serviceUrl = assistantService.ServiceURL;
        assistant_id = assistantService.workspaceID;

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
        service.CreateSession(OnCreateSession, assistant_id);

        while (!createSessionTested)
        {
            yield return null;
        }

        Log.Debug("ExampleAssistantV2.RunTest()", "Attempting to Message");
        service.Message(OnDisplayResponse, assistant_id, sessionId);

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

        service.Message(OnDisplayResponse, assistant_id, sessionId, input: mInput);
        while (!messageReturned)
        {
            yield return null;
        }
        currentStringOut = "";
    }

    IEnumerator End()
    {
        Log.Debug("ExampleAssistantV2.RunTest()", "Attempting to delete session");
        if (sessionId != "" && sessionId != null)
        {
            service.DeleteSession(OnDeleteSession, assistant_id, sessionId);

            while (!deleteSessionTested)
            {
                yield return null;
            }

            Log.Debug("ExampleAssistantV2.Examples()", "Assistant examples complete.");
        }
    }

    private void OnDisplayResponse(DetailedResponse<MessageResponse> response, IBMError error)
    {
        if (response.Result.Output.Generic.Count > 0)
        {
            Debug.Log("Response Type : " + response.Result.Output.Generic[0].ResponseType);
            switch(response.Result.Output.Generic[0].ResponseType)
            {
                case "text":
                    Debug.Log("URL: " + response.Result.Output.Generic[0].Text);
                    DisplayWatsonText(response.Result.Output.Generic[0].Text);
                    break;
                case "image":
                    Debug.Log("URL: " + response.Result.Output.Generic[0].Source);
                    imageURL = response.Result.Output.Generic[0].Source;
                    StartCoroutine("DisplayWatsonImage");
                    break;
            }
            messageReturned = true;
        }
    }

    void DisplayWatsonText( string text)
    {
        if(imageWatsonDisplayed) //last input was image or video
        {
            watsonMediaViewer.GetComponent<Animator>().Play("FadeToZero");
            imageWatsonDisplayed = false;
        }
        if (text.Contains("https://www.youtube.com/"))
        {
            videoURL = text;
            StartCoroutine("PlayVideo");
        }
        else
        {
            chatResponse.text = chat_response = text;
        }
    }

    IEnumerator DisplayWatsonImage()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageURL);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            watsonMediaViewer.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }
        if (imageWatsonDisplayed == false)
        {
            watsonMediaViewer.GetComponent<Animator>().Play("FadeToOne");
            imageWatsonDisplayed = true;
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
