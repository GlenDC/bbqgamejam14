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

	public LevelManagerScript levelManager;

	void Start()
	{
		levelManager.LaunchLevelBuilding(0);

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
			charName = "PLAYER_ONE";
			spawnPosition = spawnOne.position;
		}
		else
		{
			character = playerTwo;
			charName = "PLAYER_TWO";
			spawnPosition = spawnTwo.position;
		}

		if(character)
			Destroy(character);

		character = (GameObject) Instantiate(Resources.Load(type == CharacterType.Sausage ? "SAUSAGE" : "NINJA"));
		character.transform.parent = transform;
		character.GetComponent<PlayerController>().Init(playerID);
		character.GetComponent<Character>().Spawn(spawnPosition);
		character.name = charName;
		character.layer = LayerMask.NameToLayer(charName);
	}
}
