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

		StartCoroutine(executeTurnSystem());
	}

	// This is the primary loop of the entire game
	private IEnumerator executeTurnSystem()
	{
		foreach (string characterId in moveOrder)
		{
			// Grab the character object from the characters dictionary
			Character currentCharacter = characters[characterId];
			
			Renderer currentCharacterRenderer = currentCharacter.getCharacterGameObj().GetComponent<Renderer>();
			Color originalMaterial = currentCharacterRenderer.material.color;
			currentCharacterRenderer.material.color = Color.red;

			string selectedMove = "";

			// If the character is dead (hp <= 0), jump to the next in the loop
			if (currentCharacter.getHp() <= 0)
				continue;

			// If the character is a wizard or a cleric...
			if (characterId.StartsWith("wizard") || characterId.StartsWith("cleric"))
			{
				// Wait for the player to make a move selection
				yield return StartCoroutine(getPlayerMoveSelection(currentCharacter, (string selection) =>
				{
					// Resume the main thread with the selected move
					selectedMove = selection;
				}));

				// Perform the selected move
				if (selectedMove == "movemove")
				{
					yield return StartCoroutine(getPlayerTileInput(currentCharacter, (string location) =>
					{
						print(location);
					}));
				}
				else if (selectedMove == "attackmove")
				{
					// executeAttackMove(currentCharacter);
				}
				else if (selectedMove == "moveattack")
				{
					// executeMoveAttack(currentCharacter);
				}
			}
			else // else, we know it's AI...
			{
				// executeAiMove(currentCharacter);
			}

			currentCharacterRenderer.material.color = originalMaterial;
		}
	}

	// This method simply grabs the users choice for their move (MM, AM, MA)
	private IEnumerator getPlayerMoveSelection(Character characterPlayer, Action<string> selectionCallback)
	{
		// Instantiate the MoveChoiceOverlay prefab (*** this happens on each turn ***)
		GameObject moveChoiceOverlayPrefab = Resources.Load<GameObject>("MoveChoiceOverlay");
		GameObject moveChoiceOverlayObject = Instantiate(moveChoiceOverlayPrefab);

		this.moveChoiceOverlay = moveChoiceOverlayObject.GetComponentInChildren<MoveChoiceOverlay>();
		this.moveChoiceOverlay.setPlayerText(characterPlayer.getId());

		// Pause the game flow while the button is not clicked
		while (!moveChoiceOverlay.isButtonClicked())
		{
			yield return null;
		}

		// Get the result of the button press (selection made by the user)
		string selection = moveChoiceOverlay.getSelection();

		// Hide the overlay
		moveChoiceOverlay.hideMoveChoiceOverlay();
		moveChoiceOverlay.destroyOverlay();

		// Pass the result of the button press (selection made by the user) back to executePlayerMove() method for handling
		selectionCallback(selection);
	}

	// This method just returns the tile location of where they want to move
	private IEnumerator getPlayerTileInput(Character currentCharacter, Action<string> selectionCallback)
	{
		// Instantiate the tileInputOverlay prefabs
		GameObject tileInputOverlayPrefab = Resources.Load<GameObject>("TileInputOverlay");
		GameObject tileInputOverlayObject = Instantiate(tileInputOverlayPrefab);

		this.tileInputOverlay = tileInputOverlayObject.GetComponentInChildren<TileInputOverlay>();

		// Pause the game flow while the button is not clicked
		while (!tileInputOverlay.isButtonClicked())
		{
			yield return null;
		}

		string location = tileInputOverlay.getTileInputString();

		// Hide the overlay
		tileInputOverlay.hideTileInputOverlay();
		tileInputOverlay.destroyOverlay();

		selectionCallback(location);
	}
}