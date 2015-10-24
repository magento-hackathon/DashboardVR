using UnityEngine;
using System.Collections;

namespace PlaformDirectory {
	public class AndroidDirectoryManager : IDirectoryManager {


		#region IDirectoryManager implementation
		public string documentsDirectory {
			get {
				return Application.persistentDataPath + "/Documents";
			}
		}

		public string cachesDirectory {
			get {
				Debug.Log(libraryDirectory + "/Caches");

				return libraryDirectory + "/Caches";
			}
		}

		public string libraryDirectory {
			get {
				return Application.persistentDataPath + "/Library";
			}
		}

		public string tempDirectory {
			get {
				return Application.persistentDataPath + "/Temp";
			}
		}

		public string installDirectory {
			get {
				return Application.dataPath;
			}
		}

		public string applicationSupportDirectory {
			get {
				return System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "/ApplicationSupport";
			}
		}

		#endregion



	}
}
