using System.Collections.Generic;
using UnityEngine;

using Characters;

public class GameControllerScript : MonoBehaviour
{
	private Tilescript[] tiles;
	private Dictionary<string, Character> characters;
	private List<string> moveOrder;

	// Start is called before the first frame update
	void Start()
	{
		GameObject initControllerObject = new GameObject("InitializationController");
		InitializationController initController = initControllerObject.AddComponent<InitializationController>();
		initController.buildGame();

		this.tiles = initController.getTiles();			
		this.characters = initController.getCharacters();
		this.moveOrder = initController.getMoveOrder();

		executeTurnSystem();
	}

	// This is the primary loop of the entire game.
	private	void executeTurnSystem()
	{
		foreach (string characterId in moveOrder)
		{
			// Grab the character object from the characters dictionary
			Character currentCharacter = characters[characterId];

			// If the character is dead (hp <= 0), jump to the next in the loop
			if (currentCharacter.getHp() <= 0)
				continue;
			
			// If the character is a wizard or a cleric...
			if (characterId.StartsWith("wizard") || characterId.StartsWith("cleric"))
			{
				executePlayerMove(currentCharacter);
			}
			else // else, we know it's AI...
			{
				executeAiMove(currentCharacter);
			}
		}
	}

	private void executePlayerMove(Character characterPlayer)
	{
		
	}

	// AI code goes here (at some point)
	private void executeAiMove(Character characterAi)
	{

	}
}