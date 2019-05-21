This is simple plugin to call access native function youtube webview in android or ios
and display popup screen in Unity.

Setup to Use on Android :  
on Menu Player Settings > Android Tab > Other Settings > Configuration set Internet Access to Require 

Setup to Use on Ios :
For Unity 4.xx
- After Build Export to Xcode Project you need add file assets.bundle(find in Folder Assets/Plugin/Ios/Assets.bundle)    
  into Xcode Project
 
For Unity 5.xx
- on Plugin Inspector file libPopupYoutube.a(in Folder Assets/Plugin/Ios/libPopupYoutube.a) check framework dependencies corefoundation to true 

How to use :

There are 2 function to call from PopupYoutube Class

- To Display FullScreen Youtube
PopupYoutube.FullYoutubeView(youtubeid);
PopupYoutube.FullYoutubeView(youtubeid,autoplay);
PopupYoutube.FullYoutubeView(youtubeid,autoplay,showinfo);

- To Display Custom Screen Youtube
PopupYoutube.CustomYoutubeView(youtubeid,autoplay,showinfo,false,width,height);;

youtubeid = your video youtube id
autoplay =  autoplay when video load
show info = show video title info
width = screen width
height = screen height


Reference:
Android
- https://developers.google.com/youtube/iframe_api_reference
- https://developer.android.com/reference/android/webkit/WebView.html
Ios
- https://developers.google.com/youtube/v3/guides/ios_youtube_helper
- https://developer.apple.com/library/ios/documentation/UIKit/Reference/UIWebView_Class/






