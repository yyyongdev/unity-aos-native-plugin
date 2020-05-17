using UnityEngine;
using UnityEngine.UI;

public class AOSManager : MonoBehaviour
{
    public Text batteryValue;
    public InputField webPageURL;
    
    private AndroidJavaObject javaObject;

    private void Awake()
    {
        AndroidJavaClass javaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        javaObject = javaClass.GetStatic<AndroidJavaObject>("currentActivity");
    }

    // AOS to Unity
    public void ReceiveFromNativeGetBattery(string msg)
    {
        batteryValue.text = msg;
    }

    // Button Events
    public void OnButtonGetBattery()
    {
        javaObject.Call("NativeGetBattery");
    }
    
    public void OnButtonOpenWebPage()
    {
        javaObject.Call("NativeOpenWebPage", webPageURL.text);
    }
}
