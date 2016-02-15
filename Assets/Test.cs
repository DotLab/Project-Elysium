using UnityEngine;

public class Test : MonoBehaviour {
	void Start () {
		GetComponent<IHidable>().Hide();
	}
}
