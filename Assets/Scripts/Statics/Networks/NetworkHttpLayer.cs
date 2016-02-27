using System;
using System.Text;
using System.Text.RegularExpressions;

public static class NetworkHttpLayer {
	public const UInt32 Version = 1;

	// User Login Session Data
	static int lsid;
	static string lshash;
	// User Infos
	static string name;
	static int exp;

	#region app

	public static void NetworkTest (string message) {
		DebugConsole.Log("network test started");

		HttpPoster.Post("app_test.php", message);

		DebugConsole.Log("network test passed");
	}

	public static bool VersionCheck () {
		DebugConsole.Log("network version check started");

		var bytes = HttpPoster.Post("app_version.php", Version);
		var serverVersion = BitConverter.ToUInt32(bytes, 0);

		DebugConsole.Log("<color=cyan>servr</color> " + serverVersion);
		DebugConsole.Log("<color=cyan>local</color> " + Version);
	
		return Version >= serverVersion;
	}

	public static string GetUpdateUrl () {
		DebugConsole.Log("network update url requested");

		var bytes = HttpPoster.Post("app_update.php", Version);
		var url = Encoding.ASCII.GetString(bytes);

		DebugConsole.Log("<color=cyan>url</color> " + url);

		return url;
	}

	#endregion

	#region user

	public static bool IsValidName (string name) {
		if (string.IsNullOrEmpty(name) || name.Length < 4 || name.Contains("|")) {
			return false;
		}

		const string pattern = "^(?=.{4,16})([a-zA-Z0-9]+[-_.])*[a-zA-Z0-9]+$";
		var regex = new Regex(pattern, RegexOptions.IgnoreCase);
		return regex.IsMatch(name);
	}

	public static bool IsValidMail (string mail) {
		if (string.IsNullOrEmpty(mail) || mail.Contains("|")) {
			return false;
		}

//		const string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
//		                       + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
//		                       + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
		// https://www.w3.org/TR/html5/forms.html#valid-e-mail-address
		const string pattern = "^[a-zA-Z0-9.!#$%&'*+/=?^_`{}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";
		var regex = new Regex(pattern, RegexOptions.IgnoreCase);
		return regex.IsMatch(mail);
	}

	public static bool IsValidPass (string pass) {
		if (string.IsNullOrEmpty(pass) || pass.Length < 4 || pass.Contains("|")) {
			return false;
		}

		// const string patter = "^(?=.*[\!\#\@\$\%\&\/\(\)\=\?\*\-\+\-\_\.\:\;\,\]\[\{\}])(?=.*[a-z])(?=.*[A-Z])‌​(?=.*\d)[a-zA-Z\d]{8,}$";
		//const string pattern = "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[a-zA-Z\\d]{8,}$";
		const string pattern = "^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])[a-zA-Z0-9\\!\\#\\@\\$\\%\\&\\/\\(\\)\\=\\?\\*\\-\\+\\_\\.\\:\\;\\,\\[\\]\\{\\}]{8,}$";
		var regex = new Regex(pattern, RegexOptions.IgnoreCase);
		return regex.IsMatch(pass);
	}

	public static bool Login (string mail, string pass) {
		DebugConsole.Log("network login with " + mail);

//		if (!IsValidMail(mail)) {
//			DebugConsole.Log("<color=red>login fail</color> invalid mail");
//			return false;
//		} else if (!IsValidPass(pass)) {
//			DebugConsole.Log("<color=red>login fail</color> invalid hash");
//			return false;
//		}

		var bytes = HttpPoster.Post("user_login.php", Version, mail + "|" + pass);
		if ((bytes.Length != 4) && (bytes.Length != 20)) {
			DebugConsole.Log("<color=red>login fail</color> invalid bytes");
			return false;
		}

		if (bytes.Length == 4) {
			var errno = BitConverter.ToInt32(bytes, 0);
			switch (errno) {
			case 0:
				DebugConsole.Log("<color=red>login fail</color> invalid comb");
				return false;
			default:
				DebugConsole.Log("<color=red>login fail</color> invalid errno");
				return false;
			}
		}

		lsid = BitConverter.ToInt32(bytes, 0);
		lshash = Encoding.UTF8.GetString(bytes, 4, bytes.Length - 4);

		DebugConsole.Log("<color=cyan>lsid</color> " + lsid);
		DebugConsole.Log("<color=cyan>lshash</color> " + lshash);

		return true;
	}

	public static bool Register (string name, string mail, string pass) {
		DebugConsole.Log("network register as " + name);

		//		if (!IsValidName(name)) {
		//			DebugConsole.Log("<color=red>register fail</color> invalid name");
		//			return false;
		//		} else if (!IsValidMail(mail)) {
		//			DebugConsole.Log("<color=red>register fail</color> invalid mail");
		//			return false;
		//		} else if (!IsValidPass(pass)) {
		//			DebugConsole.Log("<color=red>register fail</color> invalid hash");
		//			return false;
		//		}

		var bytes = HttpPoster.Post("user_register.php", Version, name + "|" + mail + "|" + pass);
		if ((bytes.Length != 4) && (bytes.Length != 20)) {
			DebugConsole.Log("<color=red>register fail</color> invalid bytes");
			return false;
		}

		if (bytes.Length == 4) {
			var errno = BitConverter.ToInt32(bytes, 0);
			switch (errno) {
			case 0:
				DebugConsole.Log("<color=red>register fail</color> existing name");
				return false;
			case 1:
				DebugConsole.Log("<color=red>register fail</color> existing name");
				return false;
			default:
				DebugConsole.Log("<color=red>register fail</color> invalid errno");
				return false;
			}
		}
			
		lsid = BitConverter.ToInt32(bytes, 0);
		lshash = Encoding.UTF8.GetString(bytes, 4, bytes.Length - 4);

		DebugConsole.Log("<color=cyan>lsid</color> " + lsid);
		DebugConsole.Log("<color=cyan>lshash</color> " + lshash);

		return true;
	}

	public static bool IsLoggedIn () {
		return lsid != 0 && !string.IsNullOrEmpty(lshash);
	}

	public static bool GetInfo () {
		DebugConsole.Log("network getinfo ");

		if (!IsLoggedIn()) {
			DebugConsole.Log("<color=red>getinfo fail</color> not logged in");
			return false;
		}

		var bytes = HttpPoster.Post("user_get_info.php", lsid, lshash);
		if ((bytes.Length != 4) && (bytes.Length < 5)) {
			DebugConsole.Log("<color=red>getinfo fail</color> invalid bytes");
			return false;
		}

		if (bytes.Length == 4) {
			var errno = BitConverter.ToInt32(bytes, 0);
			switch (errno) {
			case 0:
				DebugConsole.Log("<color=red>getinfo fail</color> invalid session");
				return false;
			case 1:
				DebugConsole.Log("<color=red>getinfo fail</color> invalid userid");
				return false;
			default:
				DebugConsole.Log("<color=red>getinfo fail</color> invalid errno");
				return false;
			}
		}
			
		exp = BitConverter.ToInt32(bytes, 0);
		//var infos = Encoding.UTF8.GetString(bytes).Split(new [] { '|' }, StringSplitOptions.RemoveEmptyEntries);
		name = Encoding.UTF8.GetString(bytes, 4, bytes.Length - 4);

		DebugConsole.Log("<color=cyan>exp</color> " + exp);
		DebugConsole.Log("<color=cyan>name</color> " + name);

		return true;
	}

	#endregion

	#region ultility

	static byte[] GetBytes (string str) {
		byte[] bytes = new byte[str.Length * sizeof(char)];
		Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
		return bytes;
	}

	static string GetString (byte[] bytes) {
		var chars = new char[bytes.Length / sizeof(char)];
		Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
		return new string(chars);
	}

	#endregion
}