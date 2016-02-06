using UnityEngine;
using UnityEngine.UI;

public class UiSpriteHorizontalSwapable : MonoBehaviour, ISwapable<Sprite> {
	public EasingType swapEasingType = EasingType.Cubic;
	public float swapDuration = 0.5f;

	public float startX;
	public float endX;

	public Image mainUiImage;
	public Image transitionUiImage;

	float time;
	float y;


	void Awake () {
		y = mainUiImage.rectTransform.anchoredPosition.y;
	}

	public void Swap (Sprite newSprite) {
		if (mainUiImage.sprite == newSprite || transitionUiImage.sprite == newSprite) {
			return;
		}

		transitionUiImage.sprite = newSprite;
		time = 0;
	}

	public void SilentSwap (Sprite newSprite) {
		if (mainUiImage.sprite == newSprite || transitionUiImage.sprite == newSprite) {
			return;
		}

		mainUiImage.sprite = newSprite;
	}

	void Update () {
		if (transitionUiImage.sprite == null) {
			return;
		}

		if (time < swapDuration) {
			float easedStep = Easing.EaseInOut(time / swapDuration, swapEasingType);
			transitionUiImage.rectTransform.anchoredPosition = new Vector2(Mathf.Lerp(startX, endX, easedStep), y);

			mainUiImage.color = new Color(1, 1, 1, 1.0f - easedStep);
		} else {
			mainUiImage.sprite = transitionUiImage.sprite;
			mainUiImage.color = new Color(1, 1, 1);

			transitionUiImage.rectTransform.anchoredPosition = new Vector2(startX, y);
			transitionUiImage.sprite = null;
			return;
		}

		time += Time.deltaTime;
	}
}
