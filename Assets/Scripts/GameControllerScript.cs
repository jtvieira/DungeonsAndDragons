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
	private AiMoveOverlay aiMoveOverlay;
	private AttackMoveOverlay attackOverlay;
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
						yield return StartCoroutine(executeAttack(currentCharacter));
						yield return StartCoroutine(executeMove(currentCharacter, selectedMove));
					}
					else if (selectedMove == "moveattack")
					{
						// execute	Attack(currentCharacter);
						yield return StartCoroutine(executeMove(currentCharacter, selectedMove));
						yield return StartCoroutine(executeAttack(currentCharacter));
					}
				}
				else // else, we know it's AI...
				{
					yield return StartCoroutine(executeAi(currentCharacter));
				}

				currentCharacterRenderer.material.color = originalMaterial;
			}

			int goodCharactersAlive = 0;
			int badCharactersAlive = 0;
			foreach (Character character in characters.Values)
			{
				string id = character.getId();
				if ((id.StartsWith("wizard") || id.StartsWith("cleric")) && character.getHp() >= 0)
					goodCharactersAlive += 1;
				else if ((id.StartsWith("skeleton") || id.StartsWith("war")) && character.getHp() >= 0)
					badCharactersAlive += 1;
			}

			if (goodCharactersAlive == 0 || badCharactersAlive == 0)
				gameOver = true;
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

	private IEnumerator executeAi(Character currentCharacter)
	{
		// ======= find closest good guy ========
		float closestDistance = 9999999;
		Character closestCharacter = null;
		Vector3 currentCharacterPosition = currentCharacter.getCharacterGameObj().transform.position;
		foreach (Character character in characters.Values)
		{
			if (character.getId().StartsWith("skeleton") || character.getId().StartsWith("war"))
				break;
			
			float distance = Vector3.Distance(currentCharacterPosition, character.getCharacterGameObj().transform.position);
			if (distance < closestDistance)
			{
				closestDistance = distance;
				closestCharacter = character;
			}
		}
		// ======================================
		
		// ====== find closest tile to the good guy ======

		List<Tilescript> tilesInRange = dijkstra.getTilesInRange(currentCharacter.getCurrentTile(), currentCharacter.getMovementRange());
		closestDistance = 9999999;
		Tilescript closestTile = null;
		foreach (Tilescript tile in tilesInRange)
		{
			float distance = Vector3.Distance(tile.transform.position, closestCharacter.getCurrentTile().transform.position);

			if (distance < closestDistance && tile.hasEntity == false)
			{
				closestDistance = distance;
				closestTile = tile;
			}
		}

		currentCharacter.move(closestTile);
		// ==============================================
		
		string attack = "";
		// if tile is adjacent, attack good guy
		if (Vector3.Distance(closestCharacter.getCharacterGameObj().transform.position, closestTile.transform.position) < 1.5f)
		{
			int hitDiceRoll = UnityEngine.Random.Range(1, 21);
			
			if (hitDiceRoll + 3 > closestCharacter.getArmorScore())
			{
				// print(currentCharacter.getId() + " dice roll + 3 = " + hitDiceRoll + " against " + closestCharacter.getId() + " armor of " + closestCharacter.getArmorScore() );
				int damageRoll = UnityEngine.Random.Range(1, 12);
				int temp = hitDiceRoll + 3;
				attack = "The attack hit (1d20 + 3 = " + temp + ") against " + closestCharacter.getId() + " for (1d12 = " + damageRoll + ") damage!";  
				closestCharacter.takeDamage(damageRoll);
			}
			else
			{
				attack = "The attack missed" + closestCharacter.getId();
			}
		}
		

		GameObject aiMoveOverlayPrefab = Resources.Load<GameObject>("AiMoveOverlay");
		GameObject aiMoveOverlayObject = Instantiate(aiMoveOverlayPrefab);
		this.aiMoveOverlay = aiMoveOverlayObject.GetComponentInChildren<AiMoveOverlay>();
		this.aiMoveOverlay.setAiMoveResults(currentCharacter.getId() + "'s Results");
		this.aiMoveOverlay.setAiMoveLocation("Moved to: " + closestTile.getCoordinate());
		this.aiMoveOverlay.setAiAttackResults(attack);

		// Pause the game flow while the button is not clicked
		while (!this.aiMoveOverlay.isButtonClicked())
		{
			yield return null;
		}

		this.aiMoveOverlay.destroyOverlay();
	}

	//start of attack move overlay
	private IEnumerator executeAttack(Character currentCharacter)
	{
		bool invalidSpell = true;
		bool badSpell = false;
		string spellInput = "";

		while (invalidSpell == true)
		{
			GameObject attackOverlayPrefab = Resources.Load<GameObject>("AttackOverlay");
			GameObject attackOverlayObject = Instantiate(attackOverlayPrefab);
			this.attackOverlay = attackOverlayObject.GetComponentInChildren<AttackMoveOverlay>();

			//get spells
			this.attackOverlay.setSpellList(currentCharacter.getSpells());
			this.attackOverlay.setSpellsAvaliable(currentCharacter.getSpellCount());
			this.attackOverlay.setPlayer("Attack Move: " + currentCharacter.getId());

			if (badSpell)
				this.attackOverlay.showBadSpell();

			//===================================================================================
			//spell input and valdation
			// Pause the game flow while the button is not clicked

			// Get the result of the button press (selection made by the user)

			while (!this.attackOverlay.isButton3Clicked())
			{
				yield return null;
			}

			//check if valid spell
			if (this.attackOverlay.isButton3Clicked())
			{
				spellInput = attackOverlay.getSpellInputString();

				if (!currentCharacter.isValidSpell(spellInput))
				{
					this.attackOverlay.destroyOverlay();
					badSpell = true;
				}
				else
				{
					this.attackOverlay.showGoodSpell();
					invalidSpell = false;
					break;
				}
			}
		}

		//check to see if move valid for spell
		string moveSpellLocation = "";

		//get valid tiles
		List<Tilescript> tilesInRangeOfSpell = null;
		tilesInRangeOfSpell = this.dijkstra.getTilesInRange(currentCharacter.getCurrentTile(), currentCharacter.getSpellRange(spellInput));

		bool isVal = false;

		dijkstra.colorTiles(tilesInRangeOfSpell, "red");

		while (isVal == false)
		{
			// Pause the game flow while the button is not clicked
			while (!this.attackOverlay.isButton2Clicked())
			{
				yield return null;
			}

			if (this.attackOverlay.isButton2Clicked())
			{
				moveSpellLocation = attackOverlay.getTileInputString();
				if(isVal = dijkstra.isValidCoordinate2(tilesInRangeOfSpell, moveSpellLocation))
                {
					this.attackOverlay.setValid();
					isVal = true;
				}
				else
					this.attackOverlay.setinValid();
			}
		}

		//use spell!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

		Tilescript tileToAttack = dijkstra.getTileFromCoordinate(moveSpellLocation);

		float damage = currentCharacter.getSpellDamage(spellInput);
		string attack = "";

		//find character on tile
		foreach (string characterId in moveOrder)
		{
			int hitDiceRoll = UnityEngine.Random.Range(1, 21);

			// Grab the character object from the characters dictionary
			Character curr = characters[characterId];
			if(curr.getCurrentTile() == tileToAttack && (hitDiceRoll + 3 > curr.getArmorScore()))
            {
				curr.takeDamage(damage);
				attack = "The attack hit " + curr.getId() + " for (" + damage + ") damage!";
			}
		}

		if(attack == "")
        {
			attack = "The spell missed";
		}

		this.attackOverlay.setAttackMoveResults(attack);

		dijkstra.colorTiles(tilesInRangeOfSpell, "white");

		//===================================================================================

		// Pause the game flow while the button is not clicked
		while (!this.attackOverlay.isButtonClicked())
		{
			yield return null;
		}

		this.attackOverlay.destroyOverlay();
	}

	// In update, we check for mouse clicks on the prefab objects
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