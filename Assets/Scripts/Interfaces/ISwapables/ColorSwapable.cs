using UnityEngine;

public class ColorSwapable : MonoBehaviour, ISwapable<Color> {
	public EasingType SwapEasingType = EasingType.Cubic;
	public float SwapDuration = 0.5f;

	public GameObject ColorableGameObject;

	IColorable colorable;

	float transitionTime;
	bool isSwaping;

	Color srcColor;
	Color dstColor;

	void Awake () {
		colorable = ColorableGameObject.GetComponent<IColorable>();
	}

	public void Swap (Color newColor) {
		if (colorable.GetColor() == newColor) {
			return;
		}

		srcColor = colorable.GetColor();
		dstColor = newColor;

		transitionTime = 0;
		isSwaping = true;
	}

	public void SilentSwap (Color newColor) {
		if (colorable.GetColor() == newColor) {
			return;
		}

		colorable.SetColor(newColor);
	}

	void Update () {
		if (!isSwaping) {
			return;
		}

		if (transitionTime < SwapDuration) {
			float easedStep = Easing.EaseInOut(transitionTime / SwapDuration, SwapEasingType);
			colorable.SetColor(Color.Lerp(srcColor, dstColor, easedStep));
		} else {
			colorable.SetColor(dstColor);
			isSwaping = false;
			return;
		}

		transitionTime += Time.deltaTime;
	}
}
