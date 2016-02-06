using UnityEngine;
using UnityEngine.UI;

public class UiTextColorable : MonoBehaviour, IColorable {
	public Text UiText;

	public Color GetColor () {
		return UiText.color;
	}

	public void SetColor (Color newColor) {
		UiText.color = newColor;
	}
}
