using UnityEngine;
using System.Collections;

public class TileObject : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
	
	}

	public void SetUpTileObject(Sprite tile_sprite){

		this.GetComponent<SpriteRenderer>().sprite = tile_sprite;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
