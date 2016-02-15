using UnityEngine;
using UnityEngine.UI;

public class UiSpriteVerticalSwapable : MonoBehaviour, ISwapable<Sprite> {
	public EasingType TransitionEasingType = EasingType.Cubic;
	public float TransitionDuration = 0.5f;

	public float StartY;
	public float EndY;

	public Image MainUiImage;
	public Image TransitionUiImage;

	float x;
	float transitionTime;


	void Awake () {
		x = MainUiImage.rectTransform.anchoredPosition.x;
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
			TransitionUiImage.rectTransform.anchoredPosition = new Vector2(x, Mathf.Lerp(StartY, EndY, easedStep));

			MainUiImage.color = new Color(1, 1, 1, 1.0f - easedStep);
		} else {
			MainUiImage.sprite = TransitionUiImage.sprite;
			MainUiImage.color = new Color(1, 1, 1);

			TransitionUiImage.rectTransform.anchoredPosition = new Vector2(x, StartY);
			TransitionUiImage.sprite = null;
			return;
		}

		transitionTime += Time.deltaTime;
	}
}
