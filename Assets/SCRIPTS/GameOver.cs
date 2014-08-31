using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOver : MonoBehaviour {
	private Text gameOverTitle;
	private int winnerID;

	// Use this for initialization
	void Awake () {
		gameOverTitle = GameObject.Find("GameOverTitle").GetComponent<Text>();
		//TEMP/////////////////////////
		winnerID = 1;
		gameOverTitle.text = "PLAYER " + "X" + " WINS";
	}

	public void SetWinner(int id)
	{
		winnerID = id;
		gameOverTitle.text = "PLAYER " + winnerID + " WINS";
	}
}
