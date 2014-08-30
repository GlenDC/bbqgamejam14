using UnityEngine;
using System.Collections;

public class LevelHolder : MonoBehaviour {

	GameObject[]
		GroundTileArray = new GameObject[300];

	GameObject
		BlueTileStart,
		RedTileStart;

	GameObject[]
		WarpTileArray = new GameObject[20];

	// Use this for initialization
	void Start () {
	
	}

	public void SetUpLevelHolder(GameObject[] ground_tile_array, GameObject blue_tile_start, GameObject red_tile_start, GameObject[] warp_tile_array){

		GroundTileArray = ground_tile_array;

		BlueTileStart = blue_tile_start;

		RedTileStart = red_tile_start;

		WarpTileArray = warp_tile_array;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
