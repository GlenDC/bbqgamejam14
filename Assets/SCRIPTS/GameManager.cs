using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	private GameObject pauseMenu;

	// Use this for initialization
	void Start () {
		pauseMenu = GameObject.Find("PauseMenu");
		pauseMenu.SetActive (false);
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
