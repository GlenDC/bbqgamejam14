using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonResume : MonoBehaviour {

	private Button buttonResume;
	private GameObject pauseMenu;

	// Use this for initialization
	void Start () {
		pauseMenu = GameObject.Find("PauseMenu");
		buttonResume = GetComponentInParent<Button>();
		buttonResume.onClick.AddListener(OnClick);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnClick(){
		pauseMenu.SetActive(false);
		GameManager.IsPaused = false;
	}
}
