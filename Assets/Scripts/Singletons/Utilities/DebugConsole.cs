using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DebugConsole : MonoBehaviour {
	static int maxLineCount;

	static Text uiText;
	static LinkedList<string> lines = new LinkedList<string>();

	static string s;

	void Awake () {
		uiText = GetComponent<Text>();
		maxLineCount = (int)(600.0f / uiText.fontSize);
	}

	void FixedUpdate () {
		ParseString();

		uiText.text = s;
	}

	public static void Clear () {
		lines.Clear();
	}

	public static void Refresh (object o) {
		Debug.Log(o);

		lines.Last.Value = o + "\n";
	}

	public static void Log (object o) {
		Debug.Log(o);

		lines.AddLast(o + "\n");
		while (lines.Count > maxLineCount) {
			lines.RemoveFirst();
		}
	}

	public static void Log () {
		Log("");
	}

	static void ParseString () {
		s = "";
		foreach (var line in lines) {
			s += line;
		}
	}
}
