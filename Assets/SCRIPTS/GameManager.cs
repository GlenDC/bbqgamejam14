using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	private GameObject pauseMenu;
	private GameObject gameOverMenu;

	// Use this for initialization
	void Start () {
		pauseMenu = GameObject.Find("PauseMenu");
		gameOverMenu = GameObject.Find("GameOverMenu");
		pauseMenu.SetActive(false);
		gameOverMenu.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Menu")) {
			if (pauseMenu.activeSelf == false){

				pauseMenu.SetActive(true);
			} else {
				pauseMenu.SetActive(false);
			}
		}
	}
}
