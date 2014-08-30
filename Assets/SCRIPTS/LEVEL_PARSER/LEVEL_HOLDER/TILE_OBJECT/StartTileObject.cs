using UnityEngine;
using System.Collections;

public class StartTileObject : MonoBehaviour {

	int
		StartColor;

	// Use this for initialization
	void Start () {
	
	}

	public void SetUpStartTile(int start_color){

		StartColor = start_color;
	}

	public int GetStartColor(){
		int start_color = StartColor;
		return start_color;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
