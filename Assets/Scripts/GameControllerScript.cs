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
	private CharacterInfoOverlay characterInfoOverlay;
	private DijkstraController dijkstra;

	bool selectedCharacter = false;

	// Start is called before the first frame update
	void Start()
	{
		GameObject initControllerObject = new GameObject("InitializationController");
		InitializationController initController = initControllerObject.AddComponent<InitializationController>();
		initController.buildGame();

		this.tiles = initController.getTiles();
		this.characters = initController.getCharacters();
		this.moveOrder = initController.getMoveOrder();

		GameObject dijkstraControllerObject = new GameObject("DijkstraController");
		this.dijkstra = dijkstraControllerObject.AddComponent<DijkstraController>();
		dijkstra.setAllTiles(tiles);

		StartCoroutine(executeTurnSystem());
	}

	// This is the primary loop of the entire game
	private IEnumerator executeTurnSystem()
	{
		bool gameOver = false;
		while (gameOver == false)
		{
			foreach (string characterId in moveOrder)
			{
				// Grab the character object from the characters dictionary
				Character currentCharacter = characters[characterId];

				// displayCharacterInfo(currentCharacter);
				
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
						yield return StartCoroutine(executeMove(currentCharacter, selectedMove));
					}
					else if (selectedMove == "attackmove")
					{
						// executeAttac(currentCharacter);
						yield return StartCoroutine(executeMove(currentCharacter, selectedMove));
					}
					else if (selectedMove == "moveattack")
					{
						// execute	Attack(currentCharacter);
						yield return StartCoroutine(executeMove(currentCharacter, selectedMove));
					}
				}
				else // else, we know it's AI...
				{
					// executeAiMove(currentCharacter);
				}

				currentCharacterRenderer.material.color = originalMaterial;
			}

			int charactersAlive = 0;
			foreach (Character character in characters.Values)
			{
				if (character.getHp() >= 0)
				{
					charactersAlive += 1;
					if (charactersAlive > 1)
						break;
				}
			}

			if (charactersAlive == 1)
			{
				gameOver = true; 
			}
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
	private IEnumerator executeMove(Character currentCharacter, string moveType)
	{
		List<Tilescript> tilesInRange = null;
		if (moveType == "movemove")
			tilesInRange = this.dijkstra.getTilesInRange(currentCharacter.getCurrentTile(), currentCharacter.getMovementRange() * 2);
		else if (moveType == "moveattack" || moveType == "attackmove")
			tilesInRange = this.dijkstra.getTilesInRange(currentCharacter.getCurrentTile(), currentCharacter.getMovementRange());

		bool isValidCoordinate = false;
		string coordinate = "";
		dijkstra.colorTiles(tilesInRange, "red");

		bool firstIteration = true; // for controlling error label
		while (isValidCoordinate == false)
		{
			// Instantiate the tileInputOverlay prefabs
			GameObject tileInputOverlayPrefab = Resources.Load<GameObject>("TileInputOverlay");
			GameObject tileInputOverlayObject = Instantiate(tileInputOverlayPrefab);
			this.tileInputOverlay = tileInputOverlayObject.GetComponentInChildren<TileInputOverlay>();

			if (isValidCoordinate == false && !firstIteration)
				tileInputOverlay.showBadMove();

			// Pause the game flow while the button is not clicked
			while (!tileInputOverlay.isButtonClicked())
			{
				yield return null;
			}

			coordinate = tileInputOverlay.getTileInputString();

			isValidCoordinate = dijkstra.isValidCoordinate(tilesInRange, coordinate);

			tileInputOverlay.hideTileInputOverlay();
			tileInputOverlay.destroyOverlay();
			firstIteration = false;
		}

		Tilescript tileToMove = dijkstra.getTileFromCoordinate(coordinate);
		
		currentCharacter.move(tileToMove);

		dijkstra.colorTiles(tilesInRange, "white");
	}

	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			RaycastHit hit;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) {

			    // Check if the object we clicked on has a CharacterData script attached
				Character character = hit.collider.gameObject.GetComponent<Character>();
				if (character != null) {
					if (selectedCharacter == true)
						this.characterInfoOverlay.destroyOverlay();
					
					displayCharacterInfo(character);
					selectedCharacter = true;
				}
			}
		}
    }

	// This function just brings up the character info panel with the selected character
	private void displayCharacterInfo(Character character)
	{
		GameObject characterInfoOverlayPrefab = Resources.Load<GameObject>("CharacterInfoOverlay");
		GameObject characterInfoOverlayObject = Instantiate(characterInfoOverlayPrefab);
		this.characterInfoOverlay = characterInfoOverlayObject.GetComponentInChildren<CharacterInfoOverlay>();
		this.characterInfoOverlay.setCharacterChoice(character.getId());
		this.characterInfoOverlay.setHealth(character.getHp());
	}
}