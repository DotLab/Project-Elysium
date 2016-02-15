using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TestConsole : MonoBehaviour {
	static int maxLineCount;

	static Text uiText;
	static readonly LinkedList<string> lines = new LinkedList<string>();

	static string s;

	static string command;

	void Awake () {
		uiText = GetComponent<Text>();
		maxLineCount = (int)(600.0f / uiText.fontSize);
	}

	void Start () {
		Log("Console Started.");
		Log();
	}

	void FixedUpdate () {
		ParseString();
	}

	void Update () {
		HandleInput();
	}

	static void HandleInput () {
		foreach (var c in Input.inputString) {
			if ((c == '\n' || c == '\r')) {
				if (!string.IsNullOrEmpty(command)) {
					Log();
					SplitCommand(command.ToLower());
					command = "";
				}
				continue;
			}

			if (c == '\b') {
				command = command.Length == 0 ? "" : command.Substring(0, command.Length - 1);
				Refresh(command);
				continue;
			}

			command += c;
			Refresh(command);
		}
	}

	static void SplitCommand (string str) {
		var objects = str.Split((string[])null, System.StringSplitOptions.RemoveEmptyEntries);
		var cmd = objects[0];

		if (objects.Length > 1) {
			var prms = new string[objects.Length - 1];
			System.Array.Copy(objects, 1, prms, 0, objects.Length - 1);

			ExecuteCommand(cmd, prms);
		} else {
			ExecuteCommand(cmd, (string[])null);
		}
	}

	static void ExecuteCommand (string cmd, params string[] prms) {
		DebugConsole.Log("Executing Command: " + cmd);

		try {
			switch (cmd) {
			case "clear":
				Clear();
				break;
			case "network":
				ExecuteNetworkCommand(prms);
				break;
			default:
				throw new System.NotImplementedException("Unrecognizable Command: " + cmd);
			}
		} catch (System.Exception e) {
			DebugConsole.Log("Execution Unsuccessful.");

			RefreshLog(e.Message);
		}
	}

	static void ExecuteNetworkCommand (params string[] prms) {
		switch (prms[0]) {
		case "test":
			NetworkHttpLayer.NetworkTest(prms[1]);

			RefreshLog("Network Test Passed!");
			break;
		case "version":
			if (!NetworkHttpLayer.VersionCheck()) {
				RefreshLog("Need Update!");
			} else {
				RefreshLog("No Update Available.");
			}
			break;
		case "update":
			if (!NetworkHttpLayer.VersionCheck()) {
				RefreshLog("Need Update!");
				Application.OpenURL(NetworkHttpLayer.GetUpdateUrl());
			}
			break;
		default:
			throw new System.NotImplementedException("Unrecognizable Network Parameter: " + prms[0]);
		}
	}

	static void Clear () {
		lines.Clear();
		Log();
	}

	static void Refresh (object o) {
		lines.Last.Value = o + "\n";
	}

	public static void Log (object o) {
		lines.AddLast(o + "\n");
		while (lines.Count > maxLineCount) {
			lines.RemoveFirst();
		}
	}

	public static void Log () {
		Log("");
	}

	public static void RefreshLog (object o) {
		Refresh(o);
		Log();
	}

	static void ParseString () {
		s = "";
		foreach (var line in lines) {
			s += line;
		}

		uiText.text = s;
	}
}
