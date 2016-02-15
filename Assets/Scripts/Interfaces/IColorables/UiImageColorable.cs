using UnityEngine;
using UnityEngine.UI;

public class UiImageColorable : MonoBehaviour, IColorable {
	Image uiImage;

	void Awake () {
		uiImage = GetComponent<Image>();
	}

	public Color GetColor () {
		return uiImage.color;
	}

	public void SetColor (Color newColor) {
		uiImage.color = newColor;
	}
}
