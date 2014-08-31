using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonQuit : MonoBehaviour {
	private Button buttonQuit;

	// Use this for initialization
	void Start () {
		buttonQuit = GetComponentInParent<Button>();
		buttonQuit.onClick.AddListener(OnClick);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnClick(){
		Application.LoadLevel("menu");
	}
}
