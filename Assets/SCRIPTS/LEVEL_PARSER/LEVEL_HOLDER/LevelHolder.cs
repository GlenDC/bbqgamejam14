using UnityEngine;
using System.Collections;

public class LevelHolder : MonoBehaviour {

	[ SerializeField ] float
		NinjaWarpTimer;

	GameObject[]
		GroundTileArray = new GameObject[500];

	GameObject[]
		StartTileArray = new GameObject[2];

	GameObject
		BlueTileStart,
		RedTileStart;

	GameObject[]
		WarpTileArray = new GameObject[20];

	bool
		NinjaWarpTimerOn;

	float
		NinjaWarpClock;

	int
		NinjaWarpID;

	// Use this for initialization
	void Start () {
	
	}

	public void SetUpLevelHolder(GameObject[] ground_tile_array, GameObject[] start_tile_array, GameObject[] warp_tile_array){

		GroundTileArray = ground_tile_array;

		StartTileArray = start_tile_array;

		WarpTileArray = warp_tile_array;

		for (int i=0; i < warp_tile_array.Length; i++){

			if (WarpTileArray[i] != null){

				if (WarpTileArray[i].GetComponent<WarpTileObject>().GetWarpType() == 1){

					WarpTileArray[i].GetComponent<WarpTileObject>().DisplayWarp(false);
				}
			}
		}

		NinjaWarpTimerOn = false;

		NinjaWarpClock = 0.0f;

		NinjaWarpID = -1;

		this.tag = "LEVEL_HOLDER";
	}

	public void TriggerNinjaWarp(){

		int[] ninja_warp_id_array = new int[20];

		int ninja_warp_id_counter = 0;

		for (int i=0; i < WarpTileArray.Length; i++){

			if (WarpTileArray[i] != null){

				if (WarpTileArray[i].GetComponent<WarpTileObject>().GetWarpType() == 0){

					WarpTileArray[i].GetComponent<WarpTileObject>().DisplayWarp(false);
				}
				else if (WarpTileArray[i].GetComponent<WarpTileObject>().GetWarpType() == 1){

					ninja_warp_id_array[ninja_warp_id_counter] = i;

					ninja_warp_id_counter++;
				}
			}
		}

		int next_ninja_warp = Random.Range(0,ninja_warp_id_counter);

		NinjaWarpID = ninja_warp_id_array[next_ninja_warp];

		NinjaWarpTimerOn = true;
	}

	public void TriggerSausageWarp(){

		for (int i=0; i < WarpTileArray.Length; i++){
			
			if (WarpTileArray[i] != null){
				
				if (WarpTileArray[i].GetComponent<WarpTileObject>().GetWarpType() == 1){
					
					WarpTileArray[i].GetComponent<WarpTileObject>().DisplayWarp(false);
				}
				else if (WarpTileArray[i].GetComponent<WarpTileObject>().GetWarpType() == 0){

					WarpTileArray[i].GetComponent<WarpTileObject>().DisplayWarp(true);
				}
			}
		}
	}

	// Update is called once per frame
	void Update () {

		if (NinjaWarpTimerOn){

			NinjaWarpClock += Time.deltaTime;

			if (NinjaWarpClock >= NinjaWarpTimer){

				WarpTileArray[NinjaWarpID].GetComponent<WarpTileObject>().DisplayWarp(true);

				NinjaWarpID = -1;

				NinjaWarpTimerOn = false;

				NinjaWarpClock = 0.0f;
			}
		}
	}
}
