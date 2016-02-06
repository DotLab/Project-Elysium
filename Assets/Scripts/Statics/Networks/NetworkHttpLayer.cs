using System;

public static class NetworkHttpLayer {
	public const UInt32 Version = 0;

	public static void NetworkTest (string message) {
		DebugConsole.Log("Requesting Network Test...");

		HttpPoster.Post("test.php", message);

		DebugConsole.Log("Network Test Passed.");
	}

	public static bool VersionCheck () {
		DebugConsole.Log("Requesting Server Version...");

		var bytes = HttpPoster.Post("version.php", Version);
		var serverVersion = BitConverter.ToUInt32(bytes, 0);

		DebugConsole.Log("Server Version Received: " + serverVersion);
		DebugConsole.Log("Local Version: " + Version);
	
		if (Version < serverVersion) {
			DebugConsole.Log("Need Update!");
			return false;
		}

		DebugConsole.Log("Version Check Passed.");
		return true;
	}

	public static string GetUpdate () {
		DebugConsole.Log("Requesting Update Url...");

		var bytes = HttpPoster.Post("update.php", Version);
		var url = System.Text.Encoding.ASCII.GetString(bytes);

		DebugConsole.Log("Update Url: " + url);

		return url;
	}
}