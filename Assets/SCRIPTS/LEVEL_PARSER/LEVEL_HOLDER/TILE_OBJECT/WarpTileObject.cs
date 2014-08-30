using UnityEngine;
using System.Collections;

public class WarpTileObject : MonoBehaviour {

	int
		WarpType;

	// Use this for initialization
	void Start () {
	
	}

	public void SetUpWarpType(int warp_type){

		WarpType = warp_type;

		if (WarpType == 0){

		}
		else if (WarpType == 1){


		}
	}

	public int GetWarpType(){
		int warp_type = WarpType;
		return warp_type;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
