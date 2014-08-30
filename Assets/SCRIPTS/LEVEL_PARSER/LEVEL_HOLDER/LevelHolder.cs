using UnityEngine;
using System.Collections;

public class LevelHolder : MonoBehaviour {

	GameObject[]
		GroundTileArray = new GameObject[300];

	GameObject[]
		StartTileArray = new GameObject[2];

	GameObject
		BlueTileStart,
		RedTileStart;

	GameObject[]
		WarpTileArray = new GameObject[20];

	// Use this for initialization
	void Start () {
	
	}

	public void SetUpLevelHolder(GameObject[] ground_tile_array, GameObject[] start_tile_array, GameObject[] warp_tile_array){

		GroundTileArray = ground_tile_array;

		StartTileArray = start_tile_array;

		WarpTileArray = warp_tile_array;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
