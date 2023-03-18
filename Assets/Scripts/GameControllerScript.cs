using System.Collections.Generic;
using UnityEngine;

using Characters;
using System.Collections;

public class GameControllerScript : MonoBehaviour
{
	// Main containers
	private Tilescript[] tiles;
	private Dictionary<string, Character> characters;
	private List<string> moveOrder;

	// UI Overlay objects
	private MoveChoiceOverlay moveChoiceOverlay;
	private TileInputOverlay tileInputOverlay;

	// Start is called before the first frame update
	void Start()
	{
		GameObject initControllerObject = new GameObject("InitializationController");
		InitializationController initController = initControllerObject.AddComponent<InitializationController>();
		initController.buildGame();

		this.tiles = initController.getTiles();			
		this.characters = initController.getCharacters();
		this.moveOrder = initController.getMoveOrder();

		// Get the existing Overlay script components
		// **NOTE - every overlay we add will have to be added here
		this.moveChoiceOverlay = FindObjectOfType<MoveChoiceOverlay>();
		this.tileInputOverlay = FindObjectOfType<TileInputOverlay>();

		// Hide the overlays at the beginning of the program
		this.moveChoiceOverlay.hideMoveChoiceOverlay();
		this.tileInputOverlay.hideTileInputOverlay();

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
		this.moveChoiceOverlay.showMoveChoiceOverlay();

		// this.moveChoiceOverlay.buttonClicked += onMoveChoiceClicked;
	}
	
	// The commented code below was originally how I was handling the button event; not sure if it's the best way

	// private void onMoveChoiceClicked(string moveChoice)
	// {
	// 	print(moveChoice);

	// 	moveChoiceOverlay.buttonClicked -= onMoveChoiceClicked;
	// }

	// AI code goes here (at some point)
	private void executeAiMove(Character characterAi)
	{

	}
}