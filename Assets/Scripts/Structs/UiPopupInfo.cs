[System.Serializable]
public class UiPopupInfo {
	public string Title;
	public string Content;

	public string ButtonText;
	public float ButtonWidth;
	public delegate void ButtonDelegate();
	public ButtonDelegate ButtonAction;
}