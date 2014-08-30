using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {
	private PlayerController playercontroller;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton("Menu")) {
			Application.LoadLevel("game");
		}
	}
}
