using UnityEngine;
using System.Collections;

public class WarpTileObject : MonoBehaviour {

	[ SerializeField ] GameObject
		WarpObject;

	string
		WarpTag;

	int
		WarpType;

	bool
		WarpOn;

	// Use this for initialization
	void Start () {
	
	}

	public void SetUpWarpType(int warp_type){

		WarpType = warp_type;

		if (WarpType == 0){

			WarpTag = "SAUSAGE_WARP";

			this.tag = WarpTag;
		}
		else if (WarpType == 1){

			WarpTag = "NINJA_WARP";

			this.tag = WarpTag;
		}

		WarpOn = true;
	}

	public int GetWarpType(){
		int warp_type = WarpType;
		return warp_type;
	}

	public bool GetWarpOn(){
		bool warp_on = WarpOn;
		return warp_on;
	}

	public void DisplayWarp(bool warp_displayed){

		WarpObject.renderer.enabled = warp_displayed;

		WarpOn = warp_displayed;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
