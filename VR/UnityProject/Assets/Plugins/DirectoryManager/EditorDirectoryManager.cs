using UnityEngine;
using System.Collections;
using System.IO;

namespace PlaformDirectory {

	public class EditorDirectoryManager : IDirectoryManager {
		
		#region IDirectoryManager implementation
		public string documentsDirectory {
			get {
				return Path.Combine(Application.dataPath.Remove(Application.dataPath.Length-7), "Documents");
			}
		}
		
		public string cachesDirectory {
			get {
				return Path.Combine(Application.dataPath.Remove(Application.dataPath.Length-7), "Caches");
			}
		}
		
		public string libraryDirectory {
			get {
				return Path.Combine(Application.dataPath.Remove(Application.dataPath.Length-7), "Library");
			}
		}
		
		public string tempDirectory {
			get {
				return Path.Combine(Application.dataPath.Remove(Application.dataPath.Length-7), "Temp");
			}
		}

		public string installDirectory {
			get {
				return Application.dataPath.Remove(Application.dataPath.Length-7);
			}
		}

		public string applicationSupportDirectory {
			get {
				return Path.Combine(Application.dataPath.Remove(Application.dataPath.Length-7), "ApplicationSupport");
			}
		}
		#endregion
	}
}