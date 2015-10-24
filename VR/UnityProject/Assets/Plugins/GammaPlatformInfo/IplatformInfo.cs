using UnityEngine;
using System.Collections;

namespace GammaPlatformInfo.NativeBinding{
	public interface IPlatformInfo {

		string GetBundleVersion();
		string GetBundleShortVersion();
		string GetDisplayName();


	}
}
