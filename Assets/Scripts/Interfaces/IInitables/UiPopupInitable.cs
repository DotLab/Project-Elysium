using UnityEngine;
using UnityEngine.UI;

public class UiPopupInitable : MonoBehaviour, IInitable<UiPopupInfo>, IClickable {
	public GameObject OverlayGameObject;

	public Text TitleUiText;
	public Text ContentUiText;

	public Button ButtonUiButton;
	public RectTransform ButtonUiTransform;
	public Text ButtonUiText;

	UiPopupInfo info;

	IHidable[] hidables;

	void Awake () {
		hidables = GetComponentsInChildren<IHidable>();
	}

	public void Init (UiPopupInfo info) {
		this.info = info;

		TitleUiText.text = info.Title;
		ContentUiText.text = info.Content;

		ButtonUiTransform.sizeDelta = new Vector2(info.ButtonWidth, ButtonUiTransform.sizeDelta.y);
		ButtonUiText.text = info.ButtonText;

		OverlayGameObject.SetActive(false);

//		foreach (var hidable in hidables) {
//			hidable.Show();
//		}
	}

	public void OnClick () {
		OverlayGameObject.SetActive(true);

		foreach (var clickable in GetComponentsInParent<IClickable>()) {
			if (clickable != (IClickable)this) {
				clickable.OnClick();
			}
		}

		info.ButtonAction();

		foreach (var hidable in hidables) {
			hidable.Hide();
		}

		Destroy(gameObject, 0.5f);
	}
}