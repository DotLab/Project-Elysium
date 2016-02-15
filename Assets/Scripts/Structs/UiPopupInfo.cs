[System.Serializable]
public class UiPopupInfo {
	public string Title;
	public string Content;

	public string ButtonText;
	public float ButtonWidth;

	public delegate void ButtonDelegate ();

	public ButtonDelegate ButtonAction;

	public UiPopupInfo (
		string title,
		string content,
		string buttonText,
		float buttonWidth,
		ButtonDelegate buttonAction) {

		Title = title;
		Content = content;
		ButtonText = buttonText;
		ButtonWidth = buttonWidth;
		ButtonAction = buttonAction;
	}
}