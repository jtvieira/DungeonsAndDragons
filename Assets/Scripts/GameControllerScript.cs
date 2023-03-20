using System.Collections.Generic;
using UnityEngine;

using Characters;
using System.Collections;
using System;

public class GameControllerScript : MonoBehaviour
{
	// Main containers
	private Tilescript[] tiles;
	private Dictionary<string, Character> characters;
	private List<string> moveOrder;

	private MoveChoiceOverlay moveChoiceOverlay;
	// Start is called before the first frame update
	void Start()
	{
		GameObject initControllerObject = new GameObject("InitializationController");
		InitializationController initController = initControllerObject.AddComponent<InitializationController>();
		initController.buildGame();

		this.tiles = initController.getTiles();			
		this.characters = initController.getCharacters();
		this.moveOrder = initController.getMoveOrder();

		// // Get the existing Overlay script components
		// // **NOTE - every overlay we add will have to be added here
		// this.moveChoiceOverlay = FindObjectOfType<MoveChoiceOverlay>();
		// this.tileInputOverlay = FindObjectOfType<TileInputOverlay>();

		// // Hide the overlays at the beginning of the program
		// this.moveChoiceOverlay.hideMoveChoiceOverlay();
		// this.tileInputOverlay.hideTileInputOverlay();

		executeTurnSystem();
	}

	// This is the primary loop of the entire game.
	private	IEnumerator executeTurnSystem()
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
				// executePlayerMove(currentCharacter);
				// print ("after");
				// break;

				yield return StartCoroutine(GetPlayerMoveSelection(currentCharacter, (string selection) =>
				{
					print($"Player selected: {selection}");
				}));
			}
			else // else, we know it's AI...
			{
				executeAiMove(currentCharacter);
			}
		}
	}

	private void executePlayerMove(Character characterPlayer)
	{
		// When the player move first starts, we need to pop up a menu for their move choice
		// Call coroutine to get player's move selection
		StartCoroutine(GetPlayerMoveSelection(characterPlayer, (string selection) =>
		{
			// Handle the user's selection
			print($"Player selected: {selection}");
		}));
	}

	private IEnumerator GetPlayerMoveSelection(Character characterPlayer, Action<string> selectionCallback)
	{
		// Instantiate the MoveChoiceOverlay prefab
		// print("FIRST" + GameObject.FindObjectsOfType<MoveChoiceOverlay>().Length);

		GameObject moveChoiceOverlayPrefab = Resources.Load<GameObject>("MoveChoiceOverlay");
		GameObject moveChoiceOverlayObject = Instantiate(moveChoiceOverlayPrefab);

		// print("SECOND" + GameObject.FindObjectsOfType<MoveChoiceOverlay>().Length);
		
		this.moveChoiceOverlay = moveChoiceOverlayObject.GetComponentInChildren<MoveChoiceOverlay>();
		// print("THIRD" + GameObject.FindObjectsOfType<MoveChoiceOverlay>().Length);

		print("before yield");

		// print(this.moveChoiceOverlay.GetInstanceID());
		// Pause the game flow
		while (!moveChoiceOverlay.isButtonClicked())
		{
			yield return null;
		}

		print("after yield");

		// Get the result of the button press (selection made by the user)
		string selection = moveChoiceOverlay.getSelection();

		// Hide the overlay
		moveChoiceOverlay.hideMoveChoiceOverlay();
		moveChoiceOverlay.destroyOverlay();

		// Unpause the game flow
		yield return null;

		// Pass the result of the button press (selection made by the user) back to executePlayerMove() method for handling
		selectionCallback(selection);
	}

	// AI code goes here (at some point)
	private void executeAiMove(Character characterAi)
	{

	}
}