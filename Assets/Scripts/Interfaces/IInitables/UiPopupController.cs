using UnityEngine;
using UnityEngine.UI;

public class UiPopupController : MonoBehaviour, IInitable<UiPopupInfo>, IClickable {
	public Text TitleUiText;
	public Text ContentUiText;

	public RectTransform ButtonUiTransform;
	public Text ButtonUiText;

	UiPopupInfo info;
	RectTransform trans;

	void Awake () {
		trans = GetComponent<RectTransform>();
	}

	void Start () {
		trans.sizeDelta = new Vector2(0, 0);
	}

	public void Init (UiPopupInfo info) {
		this.info = info;

		TitleUiText.text = info.Title;
		ContentUiText.text = info.Content;

		ButtonUiTransform.sizeDelta = new Vector2(info.ButtonWidth, ButtonUiTransform.sizeDelta.y);
		ButtonUiText.text = info.ButtonText;
	}

	public void OnClick () {
		info.ButtonAction();


	}
}