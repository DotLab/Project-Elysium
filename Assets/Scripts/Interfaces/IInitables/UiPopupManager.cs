using UnityEngine;
using System.Collections;

public class UiPopupManager : MonoBehaviour, IInitable<UiPopupInfo>, IClickable {
	public GameObject PopupUiPrototype;

	IHidable hidable;

	void Awake () {
		hidable = GetComponent<IHidable>();
	}

	public void Init (UiPopupInfo info) {
		StopAllCoroutines();

		gameObject.SetActive(true);
		hidable.Show();

		var popupInstance = Instantiate(PopupUiPrototype);
		popupInstance.transform.SetParent(transform, false);
		popupInstance.GetComponent<IInitable<UiPopupInfo>>().Init(info);
	}

	public void OnClick () {
		if (transform.childCount <= 1) {
			// Has only one child, disappear.
			hidable.Hide();
			StartCoroutine(Deactivator());
		}
	}

	IEnumerator Deactivator () {
		yield return new WaitForSeconds(0.5f);

		gameObject.SetActive(false);
	}
}