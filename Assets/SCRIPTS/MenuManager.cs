using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {
	private PlayerController playercontroller;
	private Text leftArrow;
	private Text rightArrow;
	private Text levelName;

	// Use this for initialization
	void Start () {
		leftArrow = GameObject.Find ("LevelSelectorLeftText").GetComponent<Text>();
		rightArrow = GameObject.Find ("LevelSelectorRightText").GetComponent<Text>();
		levelName = GameObject.Find ("LevelName").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton("Menu")) {
			Application.LoadLevel("game");
		}
		if ((Input.GetAxis("p1Horizontal") > 0.4) || (Input.GetAxis ("p2Horizontal") > 0.4)) {
			rightArrow.color = Color.black;
		} else if ((Input.GetAxis("p1Horizontal") < -0.4) || (Input.GetAxis ("p2Horizontal") < -0.4)) {
			leftArrow.color = Color.black;
		} else {
			leftArrow.color = Color.white;
			rightArrow.color = Color.white;
			levelName.text = "BUTCHER BAY";
		}
	}
}
