using UnityEngine;
using UnityEngine.UI;

public class UiTextColorable : MonoBehaviour, IColorable {
	Text uiText;

	void Start () {
		uiText = GetComponent<Text>();
	}

	public Color GetColor () {
		return uiText.color;
	}

	public void SetColor (Color newColor) {
		uiText.color = newColor;
	}
}
