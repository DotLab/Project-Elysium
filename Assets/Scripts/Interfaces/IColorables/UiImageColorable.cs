using UnityEngine;
using UnityEngine.UI;

public class UiImageColorable : MonoBehaviour, IColorable {
	public Image UiImage;

	public Color GetColor () {
		return UiImage.color;
	}

	public void SetColor (Color newColor) {
		UiImage.color = newColor;
	}
}
