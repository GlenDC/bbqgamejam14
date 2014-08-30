using UnityEngine;
using System.Collections;

public enum EPlayerID {
	PlayerOne,
	PlayerTwo
};

public class PlayerController : MonoBehaviour {

	EPlayerID playerID;

	public bool goingLeft { private set; get; }
	public bool goingRight { private set; get; }
	public bool jumping { private set; get; }
	public bool special { private set; get; }
	public bool hasPressedMenu { private set; get; }
	public bool submit { private set; get; }
	public bool cancel { private set; get; }

	// Use this for initialization
	void Start () {
		goingLeft = false;
		goingRight = false;
		jumping = false;
		special = false;
		hasPressedMenu = false;
		submit = false;
		cancel = false;
	}

	public void Init(EPlayerID player)
	{
		playerID = player;
	}
	
	// Update is called once per frame
	void Update () {
		if (playerID == EPlayerID.PlayerOne){
			UpdatePlayerOne();
		} else {
			UpdatePlayerTwo();
		}
	}

	void UpdatePlayerOne () {
		if (Input.GetAxis("p1Horizontal") < -0.4){
			goingLeft = true;
		} else {
			goingLeft = false;
		}
		
		if (Input.GetAxis("p1Horizontal") > 0.4){
			goingRight = true;
		} else {
			goingRight = false;
		}
		
		jumping = Input.GetButton("p1Jump");		
		special = Input.GetButton("p1Special");		
		hasPressedMenu = Input.GetButton("Menu");		
		submit = Input.GetButton("Submit");		
		cancel = Input.GetButton("Cancel");
	}

	void UpdatePlayerTwo () {
		if (Input.GetAxis("p2Horizontal") < -0.4){
			goingLeft = true;
		} else {
			goingLeft = false;
		}
		
		if (Input.GetAxis("p2Horizontal") > 0.4){
			goingRight = true;
		} else {
			goingRight = false;
		}
		
		jumping = Input.GetButton("p2Jump");		
		special = Input.GetButton("p2Special");		
		hasPressedMenu = Input.GetButton("Menu");		
		submit = Input.GetButton("Submit");		
		cancel = Input.GetButton("Cancel");
	}
}