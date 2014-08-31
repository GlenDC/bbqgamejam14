using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOver : MonoBehaviour {
	private Text gameOverTitle;
	private Text gameOverInfo;
	private int winnerID;

	private float currentTime;

	// Use this for initialization
	void Awake () {
		gameOverTitle = GameObject.Find("GameOverTitle").GetComponent<Text>();
		gameOverInfo = GameObject.Find("GameOverInfo").GetComponent<Text>();

		currentTime = 0.0f;

		gameOverInfo.text = "";
	}

	public void SetWinner(int id)
	{
		winnerID = id;
		gameOverTitle.text = "PLAYER " + winnerID + " WINS";
	}

	void Update()
	{
		if(currentTime < 3.0f)
		{
			currentTime += Time.deltaTime;
			int countDown = 3 - (int)currentTime;
			gameOverInfo.text = "Press the START button in <" + countDown + "> to meat again!";
		}
		else
		{
			gameOverInfo.text = "Press the START button to meat again!";
			if(Input.anyKey)
			{
				Application.LoadLevel("menu");
			}
		}
	}
}
