using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SampleYoutube: MonoBehaviour {

	[Header("UI")]
	public InputField inputVideoId;
	public InputField inputWidth;
	public InputField inputHeight;
	public Toggle toggleautoplay;
	public Toggle toggleshowinfo;

	[Header("Youtube Variable")]
	public string youtubeid = "2MlMCIHvcnI";
	public bool autoplay = true;
	public bool showinfo = true;
	public int width = 400;
	public int height = 400;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void BtnFullYoutube()
	{
		youtubeid = inputVideoId.text;

		//display full youtube with all settings true
		PopupYoutube.FullYoutubeView(youtubeid);

	}

	public void BtnFullYoutubeSettings()
	{
		youtubeid = inputVideoId.text;
		autoplay = toggleautoplay.isOn;
		showinfo = toggleshowinfo.isOn;

		//display full youtube with settings
		PopupYoutube.FullYoutubeView(youtubeid,autoplay,showinfo);

		//you can try this too
		//PopupYoutubeAndroid.CustomYoutubeView(youtubeid,autoplay,showinfo,false,Screen.currentResolution.width,Screen.currentResolution.height);
	}

	public void BtnCustomYoutube()
	{		
		youtubeid = inputVideoId.text;
		autoplay = toggleautoplay.isOn;
		showinfo = toggleshowinfo.isOn;
		width = int.Parse(inputWidth.text);
		height =  int.Parse(inputHeight.text);

		//display custom youtube with settings 
		PopupYoutube.CustomYoutubeView(youtubeid,autoplay,showinfo,false,width,height);
	}


	/// <summary>
	/// Closes you tube.
	/// call back from mobile
	/// </summary>
	/// <param name="message">Message.</param>
	public void CloseYouTube(string message)
	{
		Debug.Log ("Message Received "+message);		
	}
}
