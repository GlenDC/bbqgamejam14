using UnityEngine;
using System.Collections;

public class TileObjectScript : MonoBehaviour {

	[ SerializeField ] GameObject
		TileObject;

	[ SerializeField ] float
		MinimumOffset,
		MaximumOffset;

	string
		TileName,
		TileType;

	Vector2
		TilePosition,
		TileSize;

	bool
		TileCollided;

	// Use this for initialization
	void Start () {
	
	}

	public void SetUpTileObject(string tile_name, string tile_type, Material tile_material, int tile_position_x, int tile_position_y, int tile_width, int tile_height, float tile_metric_size){

		TileName = tile_name;
		TileType = tile_type;

		TilePosition = Vector2.zero;
		TilePosition.x = (float)tile_position_x;
		TilePosition.y = (float)tile_position_y;

		TileSize = Vector2.zero;
		TileSize.x = (float)tile_width;
		TileSize.y = (float)tile_height;

		Vector3 tile_object_scale = TileObject.transform.localScale;
		tile_object_scale.x = TileSize.x * tile_metric_size;
		tile_object_scale.y = TileSize.y * tile_metric_size;
		TileObject.transform.localScale = tile_object_scale;

		this.GetComponent<BoxCollider>().size = TileObject.transform.localScale;

		Vector3 tile_object_position = this.transform.position;
		tile_object_position.x += TilePosition.x * tile_metric_size + (TileSize.x * tile_metric_size)/2;
		tile_object_position.y -= TilePosition.y * tile_metric_size + (TileSize.y * tile_metric_size)/2;
		tile_object_position.z += Random.Range(MinimumOffset,MaximumOffset) * Random.Range(-1.0f,1.0f);

		this.transform.position = tile_object_position;

		TileObject.renderer.material = tile_material;

		TileCollided = false;
	}

	public string GetTileName(){
		string tile_name = TileName;
		return tile_name;
	}

	public string GetTileType(){
		string tile_type = TileType;
		return tile_type;
	}

	public Vector2 GetTileSize(){
		Vector2 tile_size = TileSize;
		return tile_size;
	}

	public Vector2 GetTilePosition(){
		Vector2 tile_position = TilePosition;
		return tile_position;
	}

	public GameObject GetTileObject(){
		GameObject tile_object = TileObject;
		return tile_object;
	}

	public bool GetTileCollided(){
		bool tile_collided = TileCollided;
		return tile_collided;
	}

	public void SetTileCollided(bool tile_collided){
		TileCollided = tile_collided;
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void OnTriggerEnter(Collider collider_object){

		if (collider_object.gameObject.tag == "Character"){

			Debug.Log("collision");
		}
	}
}
