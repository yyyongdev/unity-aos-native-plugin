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