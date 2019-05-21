using UnityEngine;
using System.Collections;
#if UNITY_IPHONE
using System.Runtime.InteropServices;
#endif

public class PopupYoutube{

	#if UNITY_IPHONE
	[DllImport("__Internal")]
	public static extern void _HelloFromUnity();
	[DllImport("__Internal")]
	public static extern void _FullYoutube(string videoid,bool autoplay,bool showinfo);
	[DllImport("__Internal")]
	public static extern void _CustomYoutube(string videoid,bool autoplay,bool showinfo,bool isFull,float width,float height);
	#endif
	
	public static void FullYoutubeView(string videoid)
	{		
		#if UNITY_IPHONE || UNITY_ANDROID
		FullYoutubeView(videoid,true,true);
		#else
		Debug.Log ("Not Working.Please Run on Unity Ios or Android");
		#endif
	}
	
	public static void FullYoutubeView(string videoid,bool autoplay)
	{		
		#if UNITY_IPHONE || UNITY_ANDROID
		FullYoutubeView(videoid,autoplay,true);
		#else
		Debug.Log ("Not Working.Please Run on Unity Ios or Android");
		#endif
	}
	
	public static void FullYoutubeView(string videoid,bool autoplay,bool showinfo)
	{		
		#if UNITY_IPHONE
		_FullYoutube(videoid,autoplay,showinfo);
		#elif UNITY_ANDROID
		AndroidJavaClass unity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject> ("currentActivity");
		AndroidJavaClass pluginclass = new AndroidJavaClass ("com.johanfayt.PopupYoutube");		
		pluginclass.CallStatic ("ShowYoutube",currentActivity,videoid,autoplay,showinfo);		
		#else
		Debug.Log ("Not Working.Please Run on Unity Ios");
		#endif
	}
	
	public static void CustomYoutubeView(string videoid,bool autoplay,bool showinfo,bool isFull,float width,float height)
	{		
		#if UNITY_IPHONE
		_CustomYoutube(videoid,autoplay,showinfo,isFull,width,height);
		#elif UNITY_ANDROID
		AndroidJavaClass unity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject> ("currentActivity");
		AndroidJavaClass pluginclass = new AndroidJavaClass ("com.johanfayt.PopupYoutube");		
		pluginclass.CallStatic ("ShowCustomYoutube",currentActivity,videoid,autoplay,showinfo,isFull,width,height);
		#else
		Debug.Log ("Not Working.Please Run on Unity Ios or Android");
		#endif
	}
	
	
	
}
