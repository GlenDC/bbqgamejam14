using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour
{
	GameObject playerOne, playerTwo;
	Transform spawnOne, spawnTwo;

	void Start()
	{
		playerOne = (GameObject) Instantiate(Resources.Load("CHARACTER"));
		playerTwo = (GameObject) Instantiate(Resources.Load("CHARACTER"));

		playerOne.transform.parent = transform;
		playerTwo.transform.parent = transform;

		spawnOne = GameObject.FindWithTag("SPAWN_ONE").transform;
		spawnTwo = GameObject.FindWithTag("SPAWN_TWO").transform;

		playerOne.GetComponent<PlayerController>().Init(EPlayerID.PlayerOne);
		playerTwo.GetComponent<PlayerController>().Init(EPlayerID.PlayerTwo);

		playerOne.GetComponent<Character>().Spawn(spawnOne.position);
		playerTwo.GetComponent<Character>().Spawn(spawnTwo.position);

		playerOne.name = "Player One";
		playerTwo.name = "Player Two";
	}

	void Update()
	{
	
	}
}
