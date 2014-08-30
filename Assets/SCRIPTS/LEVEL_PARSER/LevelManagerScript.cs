using UnityEngine;
using System.Collections;
using System.Xml;

public class LevelManagerScript : MonoBehaviour {
	
	[ SerializeField ] TextAsset[]
		LevelList = new TextAsset[30];

	// Use this for initialization
	void Start () {

		//LaunchLevelLoading();
	}

	public void LaunchLevelBuilding(int level_id){

		this.GetComponent<LoaderManagerScript>().LoadLevel(LevelList[level_id]);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
