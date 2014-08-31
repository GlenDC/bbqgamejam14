using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	private GameObject pauseMenu;
<<<<<<< HEAD
	public static bool IsPaused = false;
=======
	private GameObject gameOverMenu;
>>>>>>> 03cccdc6dabc0d0e89a126aca1c6fd851588f7c9

	// Use this for initialization
	void Start () {
		pauseMenu = GameObject.Find("PauseMenu");
<<<<<<< HEAD
		pauseMenu.SetActive (false);
		IsPaused = false;
=======
		gameOverMenu = GameObject.Find("GameOverMenu");
		pauseMenu.SetActive(false);
		gameOverMenu.SetActive(false);
>>>>>>> 03cccdc6dabc0d0e89a126aca1c6fd851588f7c9
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Menu")) {
			if (pauseMenu.activeSelf == false){

				pauseMenu.SetActive(true);
				IsPaused = true;
			} else {
				pauseMenu.SetActive(false);
				IsPaused = false;
			}
		}
	}
}
