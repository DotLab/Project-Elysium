using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class WelcomeSceneUiMediator : MonoBehaviour {
	public const int MenuSceneIndex = 1;

	public GameObject OverlayGameObject;

	public GameObject TrademarkSwapableGameObject;
	public GameObject BackgroundSwapableGameObject;

	public float TrademarkStayTime = 2;

	[System.Serializable]
	public class TrademarkFrameInfo {
		public Sprite TrademarkSprite;
		public Color BackgroundColor;
	}

	public TrademarkFrameInfo[] TrademarkFrameInfos;

	IHidable overlayHidable;
	ISwapable<Sprite> trademarkSwapable;
	ISwapable<Color> backgroundSwapable;

	void Start () {
		overlayHidable = OverlayGameObject.GetComponent<IHidable>();
		trademarkSwapable = TrademarkSwapableGameObject.GetComponent<ISwapable<Sprite>>();
		backgroundSwapable = BackgroundSwapableGameObject.GetComponent<ISwapable<Color>>();

		StartCoroutine(WelcomeSceneHandler());
	}

	IEnumerator WelcomeSceneHandler () {
		for (int i = 0; i < TrademarkFrameInfos.Length; i++) {
			trademarkSwapable.Swap(TrademarkFrameInfos[i].TrademarkSprite);
			backgroundSwapable.Swap(TrademarkFrameInfos[i].BackgroundColor);

			yield return new WaitForSeconds(TrademarkStayTime);
		}

		overlayHidable.Show();
		yield return new WaitForSeconds(0.5f);

		SceneManager.LoadScene(MenuSceneIndex);
	}
}
