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
	GameManager gameManager;

	void Start()
	{
		levelManager.LaunchLevelBuilding(PlayerSettings.playerLevel);

		gameManager = GetComponent<GameManager>();

		spawnOne = GameObject.FindWithTag("SPAWN_ONE").transform;
		spawnTwo = GameObject.FindWithTag("SPAWN_TWO").transform;

		CreateCharacter(EPlayerID.PlayerOne, CharacterType.Ninja, spawnOne.position);
		CreateCharacter(EPlayerID.PlayerTwo, CharacterType.Ninja, spawnTwo.position);
	}

	void Update()
	{
	
	}

	public void CreateCharacter(EPlayerID playerID, CharacterType type, Vector3 spawnPosition)
	{
		GameObject character;
		string charName;
		Color charColor;

		if(playerID == EPlayerID.PlayerOne)
		{
			character = playerOne;
			charName = "PLAYER_ONE";
			charColor = Color.red;
		}
		else
		{
			character = playerTwo;
			charName = "PLAYER_TWO";
			charColor = Color.blue;
		}

		if(character)
			Destroy(character);

		character = (GameObject) Instantiate(Resources.Load(type == CharacterType.Sausage ? "SAUSAGE" : "NINJA"));
		character.transform.parent = transform;
		character.GetComponent<PlayerController>().Init(playerID);
		character.name = charName;
		character.tag = charName;
		character.layer = LayerMask.NameToLayer(charName);

		foreach (Transform child in character.transform)
		{
		 	Renderer rndr = child.gameObject.GetComponent<Renderer>();
		 	if(rndr)
		 	{
				rndr.material.color = charColor;
		 	}
		}

		Character charaterScript = character.GetComponent<Character>();
		charaterScript.Spawn(spawnPosition);
		charaterScript.SetPlayerID(playerID);
		charaterScript.game = this;
		charaterScript.gameManager = gameManager;

		if(playerID == EPlayerID.PlayerOne)
		{
			playerOne = character;
		}
		else
		{
			playerTwo = character;
		}
	}
}
