using UnityEngine;
using System.Collections;

public enum CharacterType
{
	Sausage,
	Ninja
}

public class Game : MonoBehaviour
{
	GameObject playerOne, playerTwo;
	Transform spawnOne, spawnTwo;

	void Start()
	{
		spawnOne = GameObject.FindWithTag("SPAWN_ONE").transform;
		spawnTwo = GameObject.FindWithTag("SPAWN_TWO").transform;

		CreateCharacter(EPlayerID.PlayerOne, CharacterType.Ninja);
		CreateCharacter(EPlayerID.PlayerTwo, CharacterType.Ninja);
	}

	void Update()
	{
	
	}

	void CreateCharacter(EPlayerID playerID, CharacterType type)
	{
		GameObject character;
		Vector3 spawnPosition;
		string charName;

		if(playerID == EPlayerID.PlayerOne)
		{
			character = playerOne;
			charName = "Player One";
			spawnPosition = spawnOne.position;
		}
		else
		{
			character = playerTwo;
			charName = "Player Two";
			spawnPosition = spawnTwo.position;
		}

		if(character)
			Destroy(character);

		character = (GameObject) Instantiate(Resources.Load(type == CharacterType.Sausage ? "SAUSAGE" : "NINJA"));
		character.transform.parent = transform;
		character.GetComponent<PlayerController>().Init(playerID);
		character.GetComponent<Character>().Spawn(spawnPosition);
		character.name = charName;
	}
}
