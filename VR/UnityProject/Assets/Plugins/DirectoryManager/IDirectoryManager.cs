using UnityEngine;
using System.Collections;

namespace PlaformDirectory {
	interface IDirectoryManager {
		string documentsDirectory {get;}
		string cachesDirectory {get;}
		string libraryDirectory {get;}
		string tempDirectory {get;}
		string installDirectory {get;}
		string applicationSupportDirectory {get;}
	}
}
