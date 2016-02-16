using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MenuSceneUiMediator : MonoBehaviour {
	public enum MenuButtonType {
		Game,
		News,
		Academy,
		Allies,
		Friends,
		Market,
		Uses
	}

	[Header("Menu Panel")]
	public GameObject[] MenuUiPanels;

	public GameObject MenuGameIconGameObject;
	public GameObject MenuBackIconGameObject;

	public Text MenuUserLevelUiText;
	public Text MenuUserNameUiText;

	IHidable[] menuUiPanelHidables;
	readonly Stack<IHidable> menuUiPanelHidableStack = new Stack<IHidable>();

	IHidable menuGameIconHidable;
	IHidable menuBackIconHidable;


	[Header("Popup Panel")]
	public GameObject PopupGameObject;

	IInitable<UiPopupInfo> popupInitable;


	void Awake () {
		menuUiPanelHidables = new IHidable[MenuUiPanels.Length];
		for (int i = 0; i < menuUiPanelHidables.Length; i++) {
			menuUiPanelHidables[i] = MenuUiPanels[i].GetComponent<IHidable>();
		}

		menuGameIconHidable = MenuGameIconGameObject.GetComponent<IHidable>();
		menuBackIconHidable = MenuBackIconGameObject.GetComponent<IHidable>();

		popupInitable = PopupGameObject.GetComponent<IInitable<UiPopupInfo>>();
	}

	public void OnBackButtonClick () {
		if (menuUiPanelHidableStack.Count > 0) {
			menuUiPanelHidableStack.Pop().Hide();

			if (menuUiPanelHidableStack.Count < 1) {
				menuGameIconHidable.Show();
				menuBackIconHidable.Hide();
			}
		}
	}

	public void OnMenuButtonClick (int buttonType) {
		if (menuUiPanelHidableStack.Count > 0 && menuUiPanelHidableStack.Peek() == menuUiPanelHidables[buttonType]) {
			Home();

			menuGameIconHidable.Show();
			menuBackIconHidable.Hide();
		} else {
			Home();

			menuGameIconHidable.Hide();
			menuBackIconHidable.Show();

			menuUiPanelHidableStack.Push(menuUiPanelHidables[buttonType]);
			menuUiPanelHidables[buttonType].Show();
		}
	}

	void Home () {
		while (menuUiPanelHidableStack.Count > 0) {
			menuUiPanelHidableStack.Pop().Hide();
		}
	}

	public void DummyPopup () {
		Popup(new UiPopupInfo("连接不能 (´・ω・`)", "您的设备无法连接到我们的服务器，再试试？\n" + Random.value.ToString("E"), "再试一次", 140, () => {
			return;
		}));
	}

	public void Popup (UiPopupInfo info) {
		popupInitable.Init(info);
	}
}
