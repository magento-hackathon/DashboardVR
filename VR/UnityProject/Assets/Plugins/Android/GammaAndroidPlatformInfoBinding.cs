using UnityEngine;
using System.Collections;


#if UNITY_ANDROID
namespace GammaPlatformInfo.NativeBinding{
	public class AndroidPlatformInfoBinding : IPlatformInfo
	{

		public string GetBundleVersion()
		{
			AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject joActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaObject joPackageManager = joActivity.Call<AndroidJavaObject>("getPackageManager");
			AndroidJavaObject joPackageName = joActivity.Call<AndroidJavaObject>("getPackageName");
			AndroidJavaObject joPackageInfo = joPackageManager.Call<AndroidJavaObject>("getPackageInfo", joPackageName, 0);
			int versionCode = joPackageInfo.Get<int>("versionCode");
			return versionCode.ToString();
		}

		public string GetBundleShortVersion()
		{
			AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject joActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaObject joPackageManager = joActivity.Call<AndroidJavaObject>("getPackageManager");
			AndroidJavaObject joPackageName = joActivity.Call<AndroidJavaObject>("getPackageName");
			AndroidJavaObject joPackageInfo = joPackageManager.Call<AndroidJavaObject>("getPackageInfo", joPackageName, 0);
			string versionName = joPackageInfo.Get<string>("versionName");
			return versionName;
		}

		public string GetDisplayName()
		{
			throw new System.NotImplementedException();
		}
	}
}
#endif
