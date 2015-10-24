using UnityEngine;
using System.Collections;
using System.IO;

public static class DirectoryManager {

	private static PlaformDirectory.IDirectoryManager platformSpecificPath;
	static DirectoryManager()
	{
#if UNITY_EDITOR
		platformSpecificPath = new PlaformDirectory.EditorDirectoryManager();
#elif UNITY_IPHONE && !UNITY_EDITOR
		platformSpecificPath = new PlaformDirectory.iOSDirectoryManager();
#elif UNITY_ANDROID
		platformSpecificPath = new PlaformDirectory.AndroidDirectoryManager();
#elif UNITY_STANDALONE
		platformSpecificPath = new PlaformDirectory.StandaloneDirectoryManager();
#endif
	}

	public static string documentsDirectory {
		get {
			return CreateDirectoryIfNotExists(platformSpecificPath.documentsDirectory);
		}
	}
	public static string cachesDirectory {
		get {
			return CreateDirectoryIfNotExists(platformSpecificPath.cachesDirectory);
		}
	}
	public static string libraryDirectory {
		get {
			return CreateDirectoryIfNotExists(platformSpecificPath.libraryDirectory);
		}
	}
	public static string tempDirectory {
		get {
			return CreateDirectoryIfNotExists(platformSpecificPath.tempDirectory);
		}
	}

	public static string saveDirectory {
		get{
			return CreateSubDirectoryIfNotExists(platformSpecificPath.cachesDirectory, "saveData");
		}
	}

	public static string installDirectory {
		get{
			return platformSpecificPath.installDirectory;
		}
	}

	public static string applicationSupportDirectory {
		get {
			return platformSpecificPath.applicationSupportDirectory;
		}
	}

	public static string CreateSubDirectoryIfNotExists(string path, string subdirectory)
	{
		string temp = Path.Combine(path, subdirectory);
		CreateDirectoryIfNotExists(temp);

		return temp;
	}

	public static string CreateDirectoryIfNotExists(string path)
	{
		if(!Directory.Exists(path))
			Directory.CreateDirectory(path);

		return path;
	}
}