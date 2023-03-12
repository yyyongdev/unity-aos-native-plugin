# Unity와 Android 네이티브 플러그인 (Unity - Android Native Plugin)
### Unity Scene
AOSManager라는 이름의 게임오브젝트와 스크립트가 있고 이벤트를 받을 버튼 2개 생성했다  
Get Battery는 디바이스의 남은 배터리를 가져오는 기능, Open Web Page는 해당 URL을 웹뷰로 여는 기능이며 Android 네이티브에서 작성된 코드로 동작한다  

### Unity C#
AndroidJavaObject 라는 유니티에서 제공하는 헬퍼클래스를 사용해서 실행중인 액티비티 객체를 받아온다  
com.unity3d.player.UnityPlayer클래스는 자바프로젝트로 익스포트했을때 유니티에서 자동으로 만들어주는 코드인데  
이 네이티브 코드를 직접 수정할일이있다면 유니티쪽에서 호출하는 이름과 달라지지 않는지 체크해야한다  
Android쪽에서 실행된 결과를 다시 유니티에서 받을수 있도록 ReceiveFromNativeGetBattery (string msg) 함수도 만들어둔다  
```cs
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
```  
  
  
   
### Android Java
Android Studio 로 빌드하고 아래 코드를 적어준다   
다시 유니티쪽으로 결과를 보낼때는 UnityPlayer.UnitySendMessage를 호출하는데 각각의 매개변수는 ("유니티 하이어라키 게임오브젝트이름", "함수이름", "전달할 매개변수값") 이다  
```java
public void NativeGetBattery()
{
    IntentFilter ifilter = new IntentFilter(Intent.ACTION_BATTERY_CHANGED);
    Intent batteryStatus = registerReceiver(null, ifilter);

    int level = batteryStatus.getIntExtra(BatteryManager.EXTRA_LEVEL, -1);
    int scale = batteryStatus.getIntExtra(BatteryManager.EXTRA_SCALE, -1);

    float batteryPct = level / (float)scale;

    int batLevel = (int)(batteryPct * 100);

    UnityPlayer.UnitySendMessage("AOSManager", "ReceiveFromNativeGetBattery", Integer.toString(batLevel));
}

public void NativeOpenWebPage(String url)
{
    Intent browserIntent = new Intent(Intent.ACTION_VIEW, Uri.parse(url));
    startActivity(browserIntent);
}
```
  
이제 자신의 Android 디바이스에 실행을 해보고 버튼을 눌러보면 잘 동작하는 것을 확인 할 수 있다  