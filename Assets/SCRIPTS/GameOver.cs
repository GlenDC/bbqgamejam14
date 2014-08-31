using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOver : MonoBehaviour {
	private Text gameOverTitle;
	private int winnerID;

	// Use this for initialization
	void Start () {
		gameOverTitle = GameObject.Find("GameOverTitle").GetComponent<Text>();
		//TEMP/////////////////////////
		winnerID = 1;
		gameOverTitle.text = "PLAYER " + "X" + " WINS";
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKey){
			Application.LoadLevel("menu");
		}
	}
}
