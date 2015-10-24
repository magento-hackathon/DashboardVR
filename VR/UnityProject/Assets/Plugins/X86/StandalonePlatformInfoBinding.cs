#if !UNITY_WEBPLAYER
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using  JsonFx.Json;

namespace GammaPlatformInfo.NativeBinding{
	public class StandalonePlatformInfoBinding : IPlatformInfo {

		Dictionary<string, object> infoDictionary;
		public StandalonePlatformInfoBinding()
		{
			string platformInfoPath = Path.Combine(Application.streamingAssetsPath, "platforminfo.txt");

			if(File.Exists(platformInfoPath)){
				infoDictionary = JsonReader.Deserialize<Dictionary<string, object>>(File.ReadAllText(platformInfoPath));
			}else{
				infoDictionary = new Dictionary<string, object>();
			}
		}

		#region IPlatformInfo implementation
		public string GetBundleVersion ()
		{
			if(infoDictionary.ContainsKey("bundle_version")){
				return System.Convert.ToString(infoDictionary["bundle_version"]);
			}
			return "unavailable";
		}
		public string GetBundleShortVersion ()
		{
			if(infoDictionary.ContainsKey("bundle_short_version")){
				return System.Convert.ToString(infoDictionary["bundle_short_version"]);
			}
			return "unavailable";
		}
		public string GetDisplayName ()
		{
			if(infoDictionary.ContainsKey("display_name")){
				return System.Convert.ToString(infoDictionary["display_name"]);
			}
			return "unavailable";
		}
		#endregion

#if UNITY_EDITOR
		public static void SaveEditorInfo(string path)
		{
			Dictionary<string, object> saveDictionary = new Dictionary<string, object>();
			saveDictionary.Add("display_name", UnityEditor.PlayerSettings.productName);
			saveDictionary.Add("bundle_version", UnityEditor.PlayerSettings.bundleVersion);
			saveDictionary.Add("bundle_short_version", UnityEditor.PlayerSettings.iOS.buildNumber);

			string platformInfoPath = Path.Combine(path, "StreamingAssets");

			if(!Directory.Exists (platformInfoPath)){
				Directory.CreateDirectory(platformInfoPath);
			}

			File.WriteAllText(Path.Combine(platformInfoPath, "platforminfo.txt"), JsonWriter.Serialize(saveDictionary));
		}
#endif
	}
}
#endif
