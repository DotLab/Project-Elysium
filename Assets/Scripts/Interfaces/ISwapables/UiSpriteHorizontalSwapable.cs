using UnityEngine;
using UnityEngine.UI;

public class UiSpriteHorizontalSwapable : MonoBehaviour, ISwapable<Sprite> {
	public EasingType TransitionEasingType = EasingType.Cubic;
	public float TransitionDuration = 0.5f;

	public float StartX;
	public float EndX;

	public Image MainUiImage;
	public Image TransitionUiImage;

	float y;
	float transitionTime;


	void Awake () {
		y = MainUiImage.rectTransform.anchoredPosition.y;
	}

	public void Swap (Sprite newSprite) {
		if (MainUiImage.sprite == newSprite || TransitionUiImage.sprite == newSprite) {
			return;
		}

		TransitionUiImage.sprite = newSprite;
		transitionTime = 0;
	}

	public void SilentSwap (Sprite newSprite) {
		if (MainUiImage.sprite == newSprite || TransitionUiImage.sprite == newSprite) {
			return;
		}

		MainUiImage.sprite = newSprite;
	}

	void Update () {
		if (TransitionUiImage.sprite == null) {
			return;
		}

		if (transitionTime < TransitionDuration) {
			float easedStep = Easing.EaseInOut(transitionTime / TransitionDuration, TransitionEasingType);
			TransitionUiImage.rectTransform.anchoredPosition = new Vector2(Mathf.Lerp(StartX, EndX, easedStep), y);

			MainUiImage.color = new Color(1, 1, 1, 1.0f - easedStep);
		} else {
			MainUiImage.sprite = TransitionUiImage.sprite;
			MainUiImage.color = new Color(1, 1, 1);

			TransitionUiImage.rectTransform.anchoredPosition = new Vector2(StartX, y);
			TransitionUiImage.sprite = null;
			return;
		}

		transitionTime += Time.deltaTime;
	}
}
