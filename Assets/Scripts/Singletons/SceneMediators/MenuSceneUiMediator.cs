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

	public GameObject[] MenuUiPanels;

	public GameObject GameIcon;
	public GameObject BackIcon;

	public Text UserLevelUiText;
	public Text UserNameUiText;

	IHidable gameIconHidable;
	IHidable backIconHidable;

	IHidable[] menuUiPanelHidables;

	readonly Stack<IHidable> menuUiPanelHidableStack = new Stack<IHidable>();

	void Awake () {
		gameIconHidable = GameIcon.GetComponent<IHidable>();
		backIconHidable = BackIcon.GetComponent<IHidable>();

		menuUiPanelHidables = new IHidable[MenuUiPanels.Length];
		for (int i = 0; i < menuUiPanelHidables.Length; i++) {
			menuUiPanelHidables[i] = MenuUiPanels[i].GetComponent<IHidable>();
		}
	}

	public void OnBackButtonClick () {
		if (menuUiPanelHidableStack.Count > 0) {
			menuUiPanelHidableStack.Pop().Hide();

			if (menuUiPanelHidableStack.Count < 1) {
				gameIconHidable.Show();
				backIconHidable.Hide();
			}
		}
	}

	public void OnMenuButtonClick (int buttonType) {
		if (menuUiPanelHidableStack.Count > 0 && menuUiPanelHidableStack.Peek() == menuUiPanelHidables[buttonType]) {
			Home();

			gameIconHidable.Show();
			backIconHidable.Hide();
		} else {
			Home();

			gameIconHidable.Hide();
			backIconHidable.Show();

			menuUiPanelHidableStack.Push(menuUiPanelHidables[buttonType]);
			menuUiPanelHidables[buttonType].Show();
		}
	}

	void Home () {
		while (menuUiPanelHidableStack.Count > 0) {
			menuUiPanelHidableStack.Pop().Hide();
		}
	}

}
