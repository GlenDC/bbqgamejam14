using UnityEngine;
using System.Collections;

public class ObjectRotation : MonoBehaviour {

	[ SerializeField ] float
		RotationSpeed;

	bool
		RotationOn;

	float
		RotationDirection;

	// Use this for initialization
	void Start () {
	
	}

	public void LaunchObjectRotation(float rotation_direction){

		RotationOn = true;

		RotationDirection = rotation_direction;
	}
	
	// Update is called once per frame
	void Update () {

		if (RotationOn){

			this.transform.RotateAround(this.transform.position,Vector3.forward,RotationDirection * RotationSpeed * Time.deltaTime);
		}
	}
}
