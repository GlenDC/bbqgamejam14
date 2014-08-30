using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;

public class LoaderManagerScript : MonoBehaviour {

	[ SerializeField ] GameObject
		TileObjectPrefab;

	[ SerializeField ] Material[]
		TileMaterialArray = new Material[7];

	GameObject[]
		TileObjectArray = new GameObject[100];

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
			
			TileObjectArray[i] = null;
		}
		
		StartDoorObject = null;
		
		ExitDoorObject = null;
	}

	public GameObject[] GetTileObjectArray(){
		GameObject[] tile_object_array = TileObjectArray;
		return tile_object_array;
	}

	public GameObject GetStartDoorObject(){
		GameObject start_door_object = StartDoorObject;
		return start_door_object;
	}

	public GameObject GetExitDoorObject(){
		GameObject exit_door_object = ExitDoorObject;
		return exit_door_object;
	}

	public void LoadLevel(TextAsset level_text_asset){

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
		
		int tile_object_counter = 0;

		for (int i=0; i < 2; i++){

			TileObjectArray[tile_object_counter] = Instantiate(TileObjectPrefab,this.transform.position,Quaternion.identity) as GameObject;
			TileObjectArray[tile_object_counter].name = "tile_object_" + tile_object_counter;

			int border_width = level_width;
			int border_height = level_height;

			int border_position_x = - level_width + 2 * level_width * i;
			int border_position_y = 0;

			TileObjectArray[tile_object_counter].GetComponent<TileObjectScript>().SetUpTileObject("level_border_" + i,"level_border",TileMaterialArray[4],border_position_x,border_position_y,border_width,border_height,this.GetComponent<MetricScript>().GetTileSize());

			tile_object_counter++;
		}

		for (int j=0; j < 2; j++){
			
			TileObjectArray[tile_object_counter] = Instantiate(TileObjectPrefab,this.transform.position,Quaternion.identity) as GameObject;
			TileObjectArray[tile_object_counter].name = "tile_object_" + tile_object_counter;

			int border_width = level_width * 3;
			int border_height = level_height;
			
			int border_position_x = - level_width;
			int border_position_y = - level_height + 2 * level_height * j;
			
			TileObjectArray[tile_object_counter].GetComponent<TileObjectScript>().SetUpTileObject("level_border_" + j,"level_border",TileMaterialArray[4],border_position_x,border_position_y,border_width,border_height,this.GetComponent<MetricScript>().GetTileSize());
			
			tile_object_counter++;
		}

		XmlNodeList object_list = level_xml.GetElementsByTagName("object");

		foreach (XmlNode object_node in object_list){
			
			string tile_name = object_node.Attributes["name"].Value;
			string tile_type = object_node.Attributes["type"].Value;

			if (tile_type != "start_door_object" && tile_type != "exit_door_object"){

				TileObjectArray[tile_object_counter] = Instantiate(TileObjectPrefab,this.transform.position,Quaternion.identity) as GameObject;
				TileObjectArray[tile_object_counter].name = "tile_object_" + tile_object_counter;

				Material tile_material = TileMaterialArray[0];

				if (tile_type == "red_object"){
					tile_material = TileMaterialArray[0];
				}
				else if (tile_type == "blue_object"){
					tile_material = TileMaterialArray[1];
				}
				else if (tile_type == "yellow_object"){
					tile_material = TileMaterialArray[2];
				}
				else if (tile_type == "green_object"){
					tile_material = TileMaterialArray[3];
				}

				int tile_position_x = int.Parse(object_node.Attributes["x"].Value)/level_tile_width;
				int tile_position_y = int.Parse(object_node.Attributes["y"].Value)/level_tile_height;
				int tile_width = int.Parse(object_node.Attributes["width"].Value)/level_tile_width;
				int tile_height = int.Parse(object_node.Attributes["height"].Value)/level_tile_height;

				TileObjectArray[tile_object_counter].GetComponent<TileObjectScript>().SetUpTileObject(tile_name,tile_type,tile_material,tile_position_x,tile_position_y,tile_width,tile_height,this.GetComponent<MetricScript>().GetTileSize());

				tile_object_counter++;
			}
			else if (tile_type == "start_door_object"){

				StartDoorObject = Instantiate(TileObjectPrefab,this.transform.position,Quaternion.identity) as GameObject;
				StartDoorObject.name = "start_door_object";

				int start_door_position_x = int.Parse(object_node.Attributes["x"].Value)/level_tile_width;
				int start_door_position_y = int.Parse(object_node.Attributes["y"].Value)/level_tile_height;
				int start_door_width = int.Parse(object_node.Attributes["width"].Value)/level_tile_width;
				int start_door_height = int.Parse(object_node.Attributes["height"].Value)/level_tile_height;

				StartDoorObject.GetComponent<TileObjectScript>().SetUpTileObject(tile_name,tile_type,TileMaterialArray[5],start_door_position_x,start_door_position_y,start_door_width,start_door_height,this.GetComponent<MetricScript>().GetTileSize());
				
				Vector3 start_door_object_position = StartDoorObject.transform.position;

				start_door_object_position.z += this.GetComponent<MetricScript>().GetTileSize();

				StartDoorObject.transform.position = start_door_object_position;
			}
			else if (tile_type == "exit_door_object"){
				
				ExitDoorObject = Instantiate(TileObjectPrefab,this.transform.position,Quaternion.identity) as GameObject;
				ExitDoorObject.name = "start_door_object";

				int exit_door_position_x = int.Parse(object_node.Attributes["x"].Value)/level_tile_width;
				int exit_door_position_y = int.Parse(object_node.Attributes["y"].Value)/level_tile_height;
				int exit_door_width = int.Parse(object_node.Attributes["width"].Value)/level_tile_width;
				int exit_door_height = int.Parse(object_node.Attributes["height"].Value)/level_tile_height;
				
				ExitDoorObject.GetComponent<TileObjectScript>().SetUpTileObject(tile_name,tile_type,TileMaterialArray[6],exit_door_position_x,exit_door_position_y,exit_door_width,exit_door_height,this.GetComponent<MetricScript>().GetTileSize());
				
				Vector3 exit_door_object_position = ExitDoorObject.transform.position;
				
				exit_door_object_position.z += this.GetComponent<MetricScript>().GetTileSize();
				
				ExitDoorObject.transform.position = exit_door_object_position;
			}
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
