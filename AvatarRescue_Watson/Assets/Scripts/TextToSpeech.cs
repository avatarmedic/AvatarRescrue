
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using IBM.Watson.SpeechToText.V1;
using IBM.Cloud.SDK;
using IBM.Cloud.SDK.Utilities;
using IBM.Cloud.SDK.DataTypes;
using IBM.Watson.TextToSpeech.V1;
using IBM.Watson.TextToSpeech.V1.Model;

public class TextToSpeech : MonoBehaviour
{
        private TextToSpeechService service;
        private string allisionVoice = "en-US_AllisonVoice";
        private string synthesizeText = "Hello, welcome to the Watson Unity SDK!";
        private string synthesizeMimeType = "audio/wav";
        private string voiceModelName = "unity-sdk-voice-model";
        private string voiceModelNameUpdated = "unity-sdk-voice-model-updated";
        private string voiceModelDescription = "Custom voice model for the Unity SDK integration tests. Safe to delete";
        private string voiceModelDescriptionUpdated = "Custom voice model for the Unity SDK integration tests. Safe to delete. (Updated)";
        private string voiceModelLanguage = "en-US";
        private string customizationId;
        private string customWord = "IBM";
        private string customWordTranslation = "eye bee m";

        private string _serviceUrl = "https://stream.watsonplatform.net/text-to-speech/api";
        string _iamApikey = "KAeU5UTXXaKmDyGqN6xh3txppYIDr2-goiWI4KoMH9Ca";
        private string _recognizeModel;

        public AudioSource source;
        void Start()
        {
            LogSystem.InstallDefaultReactors();
            Runnable.Run(CreateService());
        }

        private IEnumerator CreateService()
        {
            if (string.IsNullOrEmpty(_iamApikey))
            {
                throw new IBMException("Plesae provide IAM ApiKey for the service.");
            }

            //  Create credential and instantiate service
            Credentials credentials = null;

            //  Authenticate using iamApikey
            TokenOptions tokenOptions = new TokenOptions()
            {
                IamApiKey = _iamApikey
            };

            credentials = new Credentials(tokenOptions, _serviceUrl);

            //  Wait for tokendata
            while (!credentials.HasIamTokenData())
                yield return null;

            service = new TextToSpeechService(credentials);
            

        }
        public void SayThisText(string speech)
        {
        Debug.Log("SPEECH: " + speech);
            synthesizeText = speech;
            StartCoroutine(Synthesize());
        }

        #region Synthesize
        public IEnumerator Synthesize()
        {
            Log.Debug("TextToSpeechServiceV1IntegrationTests", "Attempting to Synthesize...");
            byte[] synthesizeResponse = null;
            AudioClip clip = null;
            service.Synthesize(
                callback: (DetailedResponse<byte[]> response, IBMError error) =>
                {
                    synthesizeResponse = response.Result;
                    clip = WaveFile.ParseWAV("myClip", synthesizeResponse);
                    PlayClip(clip);

                },
                text: synthesizeText,
                voice: allisionVoice,
                accept: synthesizeMimeType
            );

            while (synthesizeResponse == null)
                yield return null;

            yield return new WaitForSeconds(clip.length);
        }
        #endregion
        
        #region PlayClip
        private void PlayClip(AudioClip clip)
        {
            if (Application.isPlaying && clip != null)
            {
                source.spatialBlend = 0.0f;
                source.loop = false;
                source.clip = clip;
                source.Play();
            
            }
        }
        #endregion

}
