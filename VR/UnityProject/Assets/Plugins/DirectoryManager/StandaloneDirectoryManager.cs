using UnityEngine;
using System.Collections;
using System.IO;

namespace PlaformDirectory {
	
	public class StandaloneDirectoryManager : IDirectoryManager {
		
		#region IDirectoryManager implementation
		public string documentsDirectory {
			get {
				return Path.Combine(Application.persistentDataPath, "Documents");
			}
		}
		
		public string cachesDirectory {
			get {
				return Path.Combine(libraryDirectory, "Caches");
			}
		}
		
		public string libraryDirectory {
			get {
				return Path.Combine(Application.dataPath, "Library");
			}
		}
		
		public string tempDirectory {
			get {
				return Path.Combine(Application.dataPath, "Temp");
			}
		}

		public string installDirectory {
			get {
				return Application.dataPath.Remove(Application.dataPath.LastIndexOf("/"));
			}
		}

		public string applicationSupportDirectory {
			get {
				return Path.Combine(libraryDirectory, "Application Support");
			}
		}
		#endregion
	}
}