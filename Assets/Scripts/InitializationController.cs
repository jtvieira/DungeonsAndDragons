using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Characters;

public class InitializationController : MonoBehaviour
{
	int[,] mapArray = new int[,] { { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}, 
								{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
								{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
								{ 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1},
								{ 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1},
								{ 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1},
								{ 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 1},
								{ 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1},
								{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1} };

	string[] alphabet = {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", 
						"K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", 
						"U", "V", "W", "X", "Y", "Z"};

	private Tilescript[] tiles;

	private SpawnController spawnController;

	private Dictionary<string, Character> characters;

	public void buildGame()
	{
		generateTiles();
		getNeighbors();
		generateLabels();

		GameObject spawnControllerObject = new GameObject("SpawnController");
		this.spawnController = spawnControllerObject.AddComponent<SpawnController>();
		this.spawnController.initialize(tiles);

		// Important line of code... spawns all the entities; now we can have access to the characters dict
		this.spawnController.spawnEntities();

		this.characters = spawnController.getCharacters();
	}

	private void generateTiles()
	{
		int xLoc = 0;
		int zLoc = 0;
		Vector3 tilePosition = new Vector3(xLoc, 0, zLoc);


		for (int i = 0; i < mapArray.GetLength(0); i++)
		{
			for (int j = 0; j < mapArray.GetLength(1); j++) // row
			{
				if (mapArray[i,j] == 1)
				{
					GameObject tileObject = Resources.Load<GameObject>("Tile");

					GameObject instantiatedObject = Instantiate(tileObject, tilePosition, Quaternion.identity);
					Tilescript tile = instantiatedObject.GetComponent<Tilescript>();
					string coordinate = alphabet[i] + j.ToString();
					tile.initialize("tile" + i, tilePosition, coordinate);
				}

				xLoc += 1;
				tilePosition = new Vector3(xLoc, 0, zLoc);
			}
			xLoc = 0;
			zLoc += 1;
			tilePosition = new Vector3(xLoc, 0, zLoc);
		}
	}

	private void getNeighbors()
	{
		// This loop builds the neighboring tiles for each tile
		tiles = FindObjectsOfType<Tilescript>();

		for (int i = 0; i < tiles.Length; i++)
		{
			for (int j = i + 1; j < tiles.Length; j++)
			{
				if (Vector3.Distance(tiles[i].transform.position, tiles[j].transform.position) < 1.1f)
				{
					tiles[i].getNeighbors().Add(tiles[j]);
					tiles[j].getNeighbors().Add(tiles[i]);
				}
			}
		}
	}

	private void generateLabels() 
	{
		//A is positioned at (-1, 0, 0) B is positioned at (-1, 0, 1) C @ (-1, 0, 2) and so on
		//0 is positioned at (0, 0, -1) 1 @ (1, 0, -1) 2 @ (2, 0, -1) and so on
		generateLetterLabels();
		generateNumberLabels();
	}

	private void generateLetterLabels() 
	{
		int numRows = mapArray.GetLength(0);
		float zpos = -0.3f;
		for(int i = 0; i < numRows; i++) 
		{
			GameObject initialLabelObject = Resources.Load<GameObject>("Label");
			GameObject labelObject = Instantiate(initialLabelObject);
				
			Vector3 labelPosition = new Vector3(-1.5f, 0, zpos);
			labelObject.transform.position = labelPosition;

			TextMeshPro labelTMP = labelObject.GetComponent<TextMeshPro>();
			labelTMP.text = alphabet[i];
			zpos += 1.05f;
		}
	}

	private void generateNumberLabels() 
	{
		int numRows = mapArray.GetLength(1);
		float xpos = -0.3f;
		for(int i = 0; i < numRows; i++) 
		{
			GameObject initialLabelObject = Resources.Load<GameObject>("Label");
			GameObject labelObject = Instantiate(initialLabelObject);

			Vector3 labelPosition = new Vector3(xpos, 0, -1.5f);
			labelObject.transform.position = labelPosition;

			TextMeshPro labelTMP = labelObject.GetComponent<TextMeshPro>();
			labelTMP.text = i.ToString();
			xpos += 1.05f;
		}
	}

}
