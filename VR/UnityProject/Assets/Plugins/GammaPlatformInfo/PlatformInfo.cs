using UnityEngine;
using System;
using System.IO;
using System.Collections;
using GammaPlatformInfo.NativeBinding;

namespace GammaPlatformInfo{
	public class PlatformInfo {

		private static IPlatformInfo platformInfo;
		private static IPlatformInfo GetPlatformInfoObject()
		{
			if(platformInfo != null)
				return platformInfo;

#if UNITY_IPHONE
			platformInfo = new iOSPlatformInfoBinding();
#elif UNITY_ANDROID
			platformInfo = new AndroidPlatformInfoBinding();
#elif UNITY_STANDALONE
			platformInfo = new StandalonePlatformInfoBinding();
#elif UNITY_WEBPLAYER
			platformInfo = new WebplayerPlatformInfoBinding();
#endif
			return platformInfo;
		}

		public static string GetBundleVersion()
		{
			return GetPlatformInfoObject().GetBundleVersion();
		}

		public static string GetBundleShortVersion()
		{
			return GetPlatformInfoObject().GetBundleShortVersion();
		}

		public static string GetDisplayName()
		{
			return GetPlatformInfoObject().GetDisplayName();
		}

		public static string CreateDeviceId(bool overwriteExisting = false)
		{
#if !UNITY_WEBPLAYER
			string savePath = Path.Combine(DirectoryManager.libraryDirectory, "deviceid.txt");
			if(!File.Exists(savePath) || overwriteExisting){
				string deviceId = System.Guid.NewGuid().ToString();
				File.WriteAllText(savePath, deviceId);
				return deviceId;
			}
			return File.ReadAllText(savePath);
#else
			string deviceId = PlayerPrefs.GetString("deviceid");
			if(string.IsNullOrEmpty(deviceId) || overwriteExisting){
				deviceId = System.Guid.NewGuid().ToString();
				PlayerPrefs.SetString("deviceid", deviceId);
				PlayerPrefs.Save();
			}

			return deviceId;
#endif
		}

		public static string GetDeviceId()
		{
#if !UNITY_WEBPLAYER
			string savePath = Path.Combine(DirectoryManager.libraryDirectory, "deviceid.txt");
			if(!File.Exists(savePath)){
				return null;
			}
			return File.ReadAllText(savePath);
#else
			return PlayerPrefs.GetString("deviceid");
#endif
		}
	}
}
