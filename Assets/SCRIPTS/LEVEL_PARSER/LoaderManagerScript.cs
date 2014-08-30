using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;

public class LoaderManagerScript : MonoBehaviour {

	[ SerializeField ] GameObject
		LevelHolderPrefab,
		GroundTilePrefab,
		StartTilePrefab,
		WarpTilePrefab;

	[ SerializeField ] Material[]
		TileMaterialArray = new Material[7];

	[ SerializeField ] Sprite[]
		GroundSpriteArray = new Sprite[9];

	GameObject[]
		StartTileArray = new GameObject[2];

	GameObject[]
		GroundTileArray = new GameObject[300];

	GameObject
		StartDoorObject,
		ExitDoorObject;

	// Use this for initialization
	void Start () {

	}

	public void SetUpLoaderManager(){

		ResetLoaderManager();
	}

	public void ResetLoaderManager(){

		for (int i=0; i < 100; i++){
			
			GroundTileArray[i] = null;
		}
		
		StartDoorObject = null;
		
		ExitDoorObject = null;
	}

	public GameObject[] GetGroundTileArray(){
		GameObject[] ground_tile_array = GroundTileArray;
		return ground_tile_array;
	}

	public GameObject GetStartDoorObject(){
		GameObject start_door_object = StartDoorObject;
		return start_door_object;
	}

	public GameObject GetExitDoorObject(){
		GameObject exit_door_object = ExitDoorObject;
		return exit_door_object;
	}

	public GameObject LoadLevel(TextAsset level_text_asset){

		GameObject level_holder = Instantiate(LevelHolderPrefab,Vector3.zero,Quaternion.identity) as GameObject;
		level_holder.name = "level_holder";

		GameObject[] ground_tile_array = new GameObject[300];

		GameObject blue_start_object = new GameObject();
		GameObject red_start_object = new GameObject();

		GameObject[] warp_tile_array = new GameObject[20];

		XmlDocument level_xml = new XmlDocument();

		level_xml.LoadXml(level_text_asset.text);

		XmlNodeList map_list = level_xml.GetElementsByTagName("map");

		int level_width = new int();
		int level_height = new int();
		int level_tile_width = new int();
		int level_tile_height = new int();

		foreach (XmlNode map_node in map_list){

			level_width = int.Parse(map_node.Attributes["width"].Value);
			level_height = int.Parse(map_node.Attributes["height"].Value);
			level_tile_width = int.Parse(map_node.Attributes["tilewidth"].Value);
			level_tile_height = int.Parse(map_node.Attributes["tileheight"].Value);
		}

		int node_counter = 0;

		int ground_tile_counter = 0;

		int warp_tile_counter = 0;

		int tile_counter = 0;
		int line_counter = 0;
		
		XmlNodeList tile_list = level_xml.GetElementsByTagName("tile");

		foreach (XmlNode tile_node in tile_list){

			int tile_value = int.Parse(tile_node.Attributes["gid"].Value);

			if (tile_value != 0){

				tile_value -= 1;

				if (tile_value >= 0 && tile_value < 9){

					Vector3 ground_tile_position = this.transform.position;

					ground_tile_position.x = tile_counter * this.GetComponent<MetricScript>().GetTileSize();
					ground_tile_position.y = level_height * this.GetComponent<MetricScript>().GetTileSize() - line_counter * this.GetComponent<MetricScript>().GetTileSize();

					ground_tile_array[ground_tile_counter] = Instantiate(GroundTilePrefab,ground_tile_position,Quaternion.identity) as GameObject;
					ground_tile_array[ground_tile_counter].name = "tile_" + ground_tile_counter;
					ground_tile_array[ground_tile_counter].transform.parent = level_holder.transform;

					ground_tile_array[ground_tile_counter].GetComponent<TileObject>().SetUpTileObject(GroundSpriteArray[tile_value]);

					ground_tile_counter++;
				}
			}

			node_counter++;

			tile_counter++;

			if (tile_counter >= level_width){

				line_counter++;

				tile_counter = 0;
			}
		}

		XmlNodeList object_list = level_xml.GetElementsByTagName("object");

		foreach (XmlNode object_node in object_list){
			
			string tile_type = object_node.Attributes["type"].Value;

			if (tile_type == "blue_start" || tile_type == "red_start"){

				Vector3 start_tile_position = this.transform.position;
				start_tile_position.x = (float) int.Parse(object_node.Attributes["x"].Value)/level_tile_width;
				start_tile_position.y = (float) int.Parse(object_node.Attributes["y"].Value)/level_tile_height;

				if (tile_type == "blue_start"){

					blue_start_object = Instantiate(StartTilePrefab,start_tile_position,Quaternion.identity) as GameObject;
					blue_start_object.name = "blue_start_tile";
					blue_start_object.transform.parent = level_holder.transform;

					blue_start_object.GetComponent<StartTileObject>().SetUpStartTile(0);
				}
				else if (tile_type == "red_start"){

					red_start_object = Instantiate(StartTilePrefab,start_tile_position,Quaternion.identity) as GameObject;
					red_start_object.name = "blue_start_tile";
					red_start_object.transform.parent = level_holder.transform;

					red_start_object.GetComponent<StartTileObject>().SetUpStartTile(1);
				}
			}
			else if (tile_type == "sausage_warp" || tile_type == "ninja_warp"){

				Vector3 warp_tile_position = this.transform.position;
				warp_tile_position.x = (float) int.Parse(object_node.Attributes["x"].Value)/level_tile_width;
				warp_tile_position.y = (float) int.Parse(object_node.Attributes["y"].Value)/level_tile_height;

				warp_tile_array[warp_tile_counter] = Instantiate(WarpTilePrefab,warp_tile_position,Quaternion.identity) as GameObject;
				warp_tile_array[warp_tile_counter].name = "warp_tile_" + warp_tile_counter;
				warp_tile_array[warp_tile_counter].transform.parent = level_holder.transform;

				int warp_type = -1;

				if (tile_type == "sausage_warp"){

					warp_type = 0;
				}
				else if (tile_type == "ninja_warp"){

					warp_type= 1;
				}

				warp_tile_array[warp_tile_counter].GetComponent<WarpTileObject>().SetUpWarpType(warp_type);

				warp_tile_counter++;
			}
		}

		level_holder.GetComponent<LevelHolder>().SetUpLevelHolder(ground_tile_array,blue_start_object,red_start_object,warp_tile_array);

		return level_holder;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
