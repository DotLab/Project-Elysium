using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UiPopupController : MonoBehaviour, IInitable<UiPopupInfo>, IClickable {
	public RectTransform PopupUiTransform;

	public Text TitleUiText;
	public Text ContentUiText;

	public RectTransform ButtonUiTransform;
	public Text ButtonUiText;

	UiPopupInfo info;

	IHidable hidable;
	IHidable popupHidable;

	void Awake () {
		hidable = GetComponent<IHidable>();
		popupHidable = PopupUiTransform.GetComponent<IHidable>();
	}

	public void Init (UiPopupInfo info) {
		this.info = info;

		TitleUiText.text = info.Title;
		ContentUiText.text = info.Content;

		ButtonUiTransform.sizeDelta = new Vector2(info.ButtonWidth, ButtonUiTransform.sizeDelta.y);
		ButtonUiText.text = info.ButtonText;

		gameObject.SetActive(true);

		hidable.Show();
		popupHidable.Show();
	}

	public void OnClick () {
		info.ButtonAction();

		hidable.Hide();
		popupHidable.Hide();

		StartCoroutine(Deactivator());
	}

	IEnumerator Deactivator () {
		yield return new WaitForSeconds(0.5f);

		gameObject.SetActive(false);
	}
}