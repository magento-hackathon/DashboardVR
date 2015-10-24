using UnityEngine;
using System.Collections;

namespace GammaPlatformInfo.NativeBinding {
	public class WebplayerPlatformInfoBinding : IPlatformInfo {

			#region IPlatformInfo implementation

			public string GetBundleVersion ()
			{
				return "undfined";
			}

			public string GetBundleShortVersion ()
			{
				return "undefined";
			}

			public string GetDisplayName ()
			{
				return "undefined";
			}

			#endregion
	}
}
