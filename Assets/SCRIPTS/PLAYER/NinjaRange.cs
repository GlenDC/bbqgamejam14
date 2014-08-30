using UnityEngine;
using System.Collections;

public class NinjaRange : MonoBehaviour
{
	public bool enemyIsInRange { get; private set; }

	void Start()
	{
		enemyIsInRange = false;
	}

	bool IsALegalCollision(Collider other)
	{
		return other.gameObject.layer != transform.parent.gameObject.layer;
	}

	void OnTriggerEnter(Collider other)
	{
		if(IsALegalCollision(other))
			enemyIsInRange = true;
	}

	void OnTriggerExit(Collider other)
	{
		if(IsALegalCollision(other))
			enemyIsInRange = false;
	}
}
