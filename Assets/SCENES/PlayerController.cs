using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

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
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxis("Horizontal") < -0.4){
			goingLeft = true;
		} else {
			goingLeft = false;
		}

		if (Input.GetAxis("Horizontal") > 0.4){
			goingRight = true;
		} else {
			goingRight = false;
		}

		jumping = Input.GetButton("Fire1");

		special = Input.GetButton("Fire3");

		hasPressedMenu = Input.GetButton("Menu");

		submit = Input.GetButton("Submit");

		cancel = Input.GetButton("Cancel");
	}
}
