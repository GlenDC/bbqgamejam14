using UnityEngine;
using System.Collections;

public class MetricScript : MonoBehaviour {

	[ SerializeField ] float
		CharacterSize;

	[ SerializeField ] float
		TileSize;

	// Use this for initialization
	void Start () {
	
	}

	public float GetCharacterSize(){
		float character_size = CharacterSize;
		return character_size;
	}

	public float GetTileSize(){
		float tile_size = TileSize;
		return tile_size;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
