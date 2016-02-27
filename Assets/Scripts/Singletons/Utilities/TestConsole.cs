using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TestConsole : MonoBehaviour {
	static int maxLineCount;

	static Text uiText;
	static readonly LinkedList<string> textLines = new LinkedList<string>();
	static string textString = "";
	static string command = "";

	void Awake () {
		uiText = GetComponent<Text>();
		maxLineCount = (int)(800.0f / uiText.fontSize);
	}

	void Start () {
		WriteLine("Console started.");
	}

	void FixedUpdate () {
		textString = "";
		foreach (var line in textLines) {
			textString += line + "\n";
		}
		textString += command;

		uiText.text = textString;
	}

	void Update () {
		foreach (var c in Input.inputString) {
			if (c == '\n' || c == '\r') {
				if (!string.IsNullOrEmpty(command)) {
					ProcessCommandString(command.ToLower());
					command = "";
				}
			} else if (c == '\b') {
				command = command.Length == 0 ? "" : command.Substring(0, command.Length - 1);
			} else {
				command += c;
			}
		}	
	}

	static void ProcessCommandString (string str) {
		var items = str.Split((string[])null, System.StringSplitOptions.RemoveEmptyEntries);
		var cmd = items[0];
		DebugConsole.Log("cmd " + cmd);
		WriteLine(str);

		try {
			switch (cmd) {
			case "cls":
				DebugConsole.Clear();
				Clear();
				break;
			case "net":
				ExecuteNetworkCommand(items);
				break;
			default:
				throw new System.NotImplementedException("Command '" + cmd + "' not implemented.");
			}
		} catch (System.Exception e) {
			DebugConsole.Log("<color=red>cmd " + cmd + " failed</color>");
			WriteLine("Execution exited with <color=cyan>" + e.Message + "</color> at <color=yellow>" + e.TargetSite + "</color>");
		}
	}

	#region Commands Handlers

	static void ExecuteNetworkCommand (string[] prms) {
		switch (prms[1]) {
		case "test":
			NetworkHttpLayer.NetworkTest(prms[2]);
			WriteLine("Network test passed.");
			break;
		case "ver":
			if (!NetworkHttpLayer.VersionCheck()) {
				WriteLine("Need update.");
			} else {
				WriteLine("No update available.");
			}
			break;
		case "upd":
			if (!NetworkHttpLayer.VersionCheck()) {
				WriteLine("Need update, trying to download.");
				Application.OpenURL(NetworkHttpLayer.GetUpdateUrl());
			} else {
				WriteLine("No update available.");
			}
			break;
		case "cname":
			if (NetworkHttpLayer.IsValidName(prms[2])) {
				WriteLine("Valid User Name.");
			} else {
				WriteLine("Invalid User Name.");
			}
			break;
		case "cmail":
			if (NetworkHttpLayer.IsValidMail(prms[2])) {
				WriteLine("Valid Email Address.");
			} else {
				WriteLine("Invalid Email Address.");
			}
			break;
		case "cpass":
			if (NetworkHttpLayer.IsValidPass(prms[2])) {
				WriteLine("Valid Password.");
			} else {
				WriteLine("Invalid Password.");
			}
			break;
		case "log":
			if (NetworkHttpLayer.Login(prms[2], prms[3])) {
				WriteLine("Login successful.");
			} else {
				WriteLine("Login failed.");
			}
			break;
		case "reg":
			if (NetworkHttpLayer.Register(prms[2], prms[3], prms[4])) {
				WriteLine("Register successful.");
			} else {
				WriteLine("Register failed.");
			}
			break;
		case "geti":
			if (NetworkHttpLayer.GetInfo()) {
				WriteLine("Info Got.");
			} else {
				WriteLine("Get failed.");
			}
			break;
		default:
			throw new System.NotImplementedException("Action '" + prms[1] + "' not implemented.");
		}
	}

	#endregion

	#region Console Implementation

	static void Clear () {
		textLines.Clear();
	}

	static void Refresh (object o) {
		textLines.Last.Value = o.ToString();
	}

	static void Write (object o) {
		textLines.Last.Value += o.ToString();
	}

	static void WriteLine (object o = null) {
		textLines.AddLast(o.ToString());
		while (textLines.Count > maxLineCount) {
			textLines.RemoveFirst();
		}
	}

	#endregion
}
