using UnityEngine;
using System.Collections;

public class DeathEffect : MonoBehaviour {

	[ SerializeField ] GameObject
		BloodParticle;

	// Use this for initialization
	void Start () {
	
	}

	public void SetUpDeathEffect(){

		BloodParticle.GetComponent<ParticleSystem>().Play();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
