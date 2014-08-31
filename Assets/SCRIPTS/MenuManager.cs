using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {
	private PlayerController playercontroller;
	private Text leftArrow;
	private Text rightArrow;
	private Text levelName;

	public PlayerSettings playerSettings;

	public string[] levelNames = new string[3] {"BUTCHER BAY", "HEAVEN", "CHRISTMAS"};

	public float delayTimeButtons = 0.25f;
	float currentTime = 0.0f;

	// Use this for initialization
	void Start () {
		leftArrow = GameObject.Find ("LevelSelectorLeftText").GetComponent<Text>();
		rightArrow = GameObject.Find ("LevelSelectorRightText").GetComponent<Text>();
		levelName = GameObject.Find ("LevelName").GetComponent<Text>();

		levelName.text = levelNames[PlayerSettings.playerLevel];
	}
	
	// Update is called once per frame
	void Update () {
		if(currentTime < delayTimeButtons)
		{
			currentTime += Time.deltaTime;
			leftArrow.color = Color.white;
			rightArrow.color = Color.white;
		}
		else
		{
			if (Input.GetButton("Submit")) {
				Application.LoadLevelAdditive("game");
				Destroy(gameObject);
			}
			if ((Input.GetAxis("p1Horizontal") > 0.4) || (Input.GetAxis ("p2Horizontal") > 0.4)) {
				rightArrow.color = Color.black;
				++PlayerSettings.playerLevel;
				if(PlayerSettings.playerLevel > 2)
					PlayerSettings.playerLevel = 2;
				currentTime = 0.0f;
			} else if ((Input.GetAxis("p1Horizontal") < -0.4) || (Input.GetAxis ("p2Horizontal") < -0.4)) {
				leftArrow.color = Color.black;
				--PlayerSettings.playerLevel;
				if(PlayerSettings.playerLevel < 0)
					PlayerSettings.playerLevel = 0;
				currentTime = 0.0f;
			}
		}

		leftArrow.enabled = PlayerSettings.playerLevel != 0;
		rightArrow.enabled = PlayerSettings.playerLevel != 2;

		levelName.text = levelNames[PlayerSettings.playerLevel];
	}
}
