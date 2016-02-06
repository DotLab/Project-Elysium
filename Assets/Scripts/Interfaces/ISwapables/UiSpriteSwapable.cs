using UnityEngine;
using UnityEngine.UI;

public class UiSpriteSwapable : MonoBehaviour, ISwapable<Sprite> {
	public EasingType swapEasingType = EasingType.Cubic;
	public float swapDuration = 0.5f;

	public Image mainUiImage;
	public Image transitionUiImage;

	float time;


	public void Swap (Sprite newSprite) {
		if (mainUiImage.sprite == newSprite || transitionUiImage.sprite == newSprite) {
			return;
		}

		transitionUiImage.sprite = newSprite;
		transitionUiImage.color = Color.clear;
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
			mainUiImage.color = new Color(1, 1, 1, 1.0f - easedStep);
			transitionUiImage.color = new Color(1, 1, 1, easedStep); 
		} else {
			mainUiImage.sprite = transitionUiImage.sprite;
			mainUiImage.color = Color.white;

			transitionUiImage.sprite = null;
			transitionUiImage.color = Color.clear;
			return;
		}

		time += Time.deltaTime;
	}
}
