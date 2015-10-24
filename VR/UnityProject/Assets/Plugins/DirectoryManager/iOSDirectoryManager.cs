using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

namespace PlaformDirectory {
	
	public class iOSDirectoryManager : IDirectoryManager {
		
		#region IDirectoryManager implementation
		public string documentsDirectory {
			get{
				return System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
			}
		}
		
		public string cachesDirectory {
			get {
				return _getCachesPath();
			}
		}
		
		public string libraryDirectory {
			get {
				return _getLibraryPath();
			}
		}
		
		public string tempDirectory {
			get {
				return _getTempPath();
			}
		}

		public string applicationSupportDirectory {
			get {
				return _getAppSupportPath();
			}
		}

		public string installDirectory {
			get {
				return documentsDirectory.Remove(documentsDirectory.LastIndexOf("/"));
			}
		}
		#endregion
		
		[DllImport("__Internal")]
		private static extern string _getCachesPath();
		
		[DllImport("__Internal")]
		private static extern string _getLibraryPath();
		
		[DllImport("__Internal")]
		private static extern string _getTempPath();

		[DllImport("__Internal")]
		private static extern string _getAppSupportPath();
	}
}
