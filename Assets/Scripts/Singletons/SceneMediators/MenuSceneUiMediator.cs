using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MenuSceneUiMediator : MonoBehaviour {
	const string UserMailStringKey = "user_login_mail";
	const string UserPassStringKey = "user_login_pass";

	static bool IsVisitor = true;

	[Header("Menu Panel")]
	public AudioClip MenuButtonSound;
	public AudioClip MenuBackButtonSound;
	public AudioClip MenuPanelSound;
	public AudioClip MenuPanelBackSound;

	public GameObject MenuBackgroundOverlayGameObject;

	public GameObject[] MenuUiIconGameObjects;
	public GameObject[] MenuUiTitleGameObjects;
	public GameObject[] MenuUiPanelGameObjects;

	public GameObject MenuGameIconGameObject;
	public GameObject MenuBackIconGameObject;

	public Text MenuUserLevelUiText;
	public Text MenuUserNameUiText;

	IHidable menuBackgroundOverlayHidable;

	IHidable[] menuUiIconHidables;
	IHidable[] menuUiTitleHidables;
	IHidable[] menuUiPanelHidables;
	readonly Stack<IHidable> menuUiPanelHidableStack = new Stack<IHidable>();

	IHidable menuGameIconHidable;
	IHidable menuBackIconHidable;


	[Header("Popup Panel")]
	public GameObject PopupGameObject;

	IInitable<UiPopupInfo> popupManager;


	void Awake () {
		menuBackgroundOverlayHidable = MenuBackgroundOverlayGameObject.GetComponent<IHidable>();

		menuUiIconHidables = new IHidable[MenuUiIconGameObjects.Length];
		for (int i = 0; i < menuUiIconHidables.Length; i++) {
			menuUiIconHidables[i] = MenuUiIconGameObjects[i].GetComponent<IHidable>();
		}
		menuUiTitleHidables = new IHidable[MenuUiTitleGameObjects.Length];
		for (int i = 0; i < menuUiTitleHidables.Length; i++) {
			menuUiTitleHidables[i] = MenuUiTitleGameObjects[i].GetComponent<IHidable>();
		}
		menuUiPanelHidables = new IHidable[MenuUiPanelGameObjects.Length];
		for (int i = 0; i < menuUiPanelHidables.Length; i++) {
			menuUiPanelHidables[i] = MenuUiPanelGameObjects[i].GetComponent<IHidable>();
		}

		menuGameIconHidable = MenuGameIconGameObject.GetComponent<IHidable>();
		menuBackIconHidable = MenuBackIconGameObject.GetComponent<IHidable>();

		popupManager = PopupGameObject.GetComponent<IInitable<UiPopupInfo>>();
	}

	void Start () {
		// First check version.
		try {
			if (!NetworkHttpLayer.VersionCheck()) {
				UpdatePopup();
			}
		} catch (System.Net.WebException e) {
			DebugConsole.Log(e.Message);
			ConnectionErrorPopup(e.Message, Start);
		}

		// Than try to login, if fail, log as visitor.
		if (PlayerPrefs.HasKey(UserMailStringKey) && PlayerPrefs.HasKey(UserPassStringKey)) {
			IsVisitor = false;

			try {
				if (!NetworkHttpLayer.Login(PlayerPrefs.GetString(UserMailStringKey), PlayerPrefs.GetString(UserPassStringKey))) {
					PlayerPrefs.DeleteKey(UserMailStringKey);
					PlayerPrefs.DeleteKey(UserPassStringKey);
					IsVisitor = true;
				}
			} catch (System.Net.WebException e) {
				DebugConsole.Log(e.Message);
				ConnectionErrorPopup(e.Message, Start);
			}
		}
		// If visitor, perform restrictions.
		if (IsVisitor) {
			MenuUserLevelUiText.text = "X";
			MenuUserNameUiText.text = "LOG IN or REGISTER";
		}
	}

	#region Ui Callbacks

	public void OnBackButtonClick () {
		if (menuUiPanelHidableStack.Count > 0) {
			// Has something on the top.
			AudioSource.PlayClipAtPoint(MenuBackButtonSound, transform.position, 0.4f);
			AudioSource.PlayClipAtPoint(MenuPanelBackSound, transform.position, 0.2f);

			menuUiPanelHidableStack.Pop().Hide();

			if (menuUiPanelHidableStack.Count == 0) {
				// Reached the root.
				menuGameIconHidable.Show();
				menuBackIconHidable.Hide();
				menuBackgroundOverlayHidable.Hide();

				for (int i = 0; i < menuUiIconHidables.Length; i++) {
					menuUiIconHidables[i].Show();
					menuUiTitleHidables[i].Show();
				}
			}
		}
	}

	public void OnMenuButtonClick (int buttonIndex) {
		if (menuUiPanelHidableStack.Count > 0 && menuUiPanelHidableStack.Peek() == menuUiPanelHidables[buttonIndex]) {
			// Pressing the Panel on the top, back to root.
			AudioSource.PlayClipAtPoint(MenuBackButtonSound, transform.position, 0.4f);
			AudioSource.PlayClipAtPoint(MenuPanelBackSound, transform.position, 0.2f);

			while (menuUiPanelHidableStack.Count > 0) {
				menuUiPanelHidableStack.Pop().Hide();
			}

			menuGameIconHidable.Show();
			menuBackIconHidable.Hide();
			menuBackgroundOverlayHidable.Hide();

			for (int i = 0; i < menuUiIconHidables.Length; i++) {
				menuUiIconHidables[i].Show();
				menuUiTitleHidables[i].Show();
			}
		} else {
			AudioSource.PlayClipAtPoint(MenuButtonSound, transform.position, 0.4f);
			AudioSource.PlayClipAtPoint(MenuPanelSound, transform.position, 0.2f);

			// Pressing another Panel, back to root and show new Panel.
			while (menuUiPanelHidableStack.Count > 0) {
				menuUiPanelHidableStack.Pop().Hide();
			}

			menuGameIconHidable.Hide();
			menuBackIconHidable.Show();
			menuBackgroundOverlayHidable.Show();

			for (int i = 0; i < menuUiIconHidables.Length; i++) {
				if (i == buttonIndex) {
					menuUiIconHidables[i].Show();
					menuUiTitleHidables[i].Show();
				} else {
					menuUiIconHidables[i].Hide();	
					menuUiTitleHidables[i].Hide();
				}
			}

			menuUiPanelHidableStack.Push(menuUiPanelHidables[buttonIndex]);
			menuUiPanelHidables[buttonIndex].Show();
		}
	}

	#endregion

	#region Popups

	void ConnectionErrorPopup (string message, UiPopupInfo.ButtonDelegate action) {
		popupManager.Init(new UiPopupInfo(
				"连接不能 (´・ω・`)",
				"您的设备无法连接到我们的服务器，再试试？\n" + message,
				"再试一次",
				140,
				action
			));
	}

	void UpdatePopup () {
		popupManager.Init(new UiPopupInfo(
				"更新啦 (๑•̀ㅂ•́)و",
				"几天不见，Project E又变得更好了，不赶紧试试？",
				"下载更新",
				135,
				() => {
					Application.OpenURL(NetworkHttpLayer.GetUpdateUrl());
					Application.Quit();
				}
			));
	}

	#endregion
}
