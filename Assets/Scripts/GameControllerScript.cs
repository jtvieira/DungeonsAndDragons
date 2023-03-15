using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameControllerScript : MonoBehaviour
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

	Tilescript[] tiles;

	public Tilescript start, end;

	private SpawnController spawnController;

	Dictionary<string, Character> characters;

	// Start is called before the first frame update
	void Start()
	{
		generateTiles();
		
		getNeighbors();

		GameObject spawnControllerObject = new GameObject("SpawnController");
		SpawnController spawnController = spawnControllerObject.AddComponent<SpawnController>();
		spawnController.initialize(tiles);

		// Important line of code... spawns all the entities; now we can have access to the characters dict
		spawnController.spawnEntities();

		// Once spawn controller spawns!
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
					tile.initialize("tile" + i, tilePosition);
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

	// Update is called once per frame
	void Update()
	{
		//computePath(start, end);

		// List<Tilescript> tilesInRange = getTilesInRange(start, 3);
	}

	List<Tilescript> tilesQueue = new List<Tilescript>();

	public void computePath(Tilescript start, Tilescript end)
	{
		//clear all nodes from past computing
		for (int i = 0; i < tiles.Length; i++)
		{
			tiles[i].clear();
		}

		Tilescript current;

		//start of dijkstra
		start.setDistance(0);
		tilesQueue.Clear();
		tilesQueue.Add(start);

		while (tilesQueue.Count > 0)
		{
			//find least tile
			int smallestIndex = 0;
			for (int i = 1; i < tilesQueue.Count; i++)
			{
				if (tilesQueue[i].getDistance() < tilesQueue[smallestIndex].getDistance())
				{
					smallestIndex = i;
				}
			}

			current = tilesQueue[smallestIndex];
			tilesQueue.RemoveAt(smallestIndex);

			current.setVisited(true);

			if (current == end)
			{
				break;
			}

			for (int i = 0; i < current.getNeighbors().Count; i++)
			{
				if (!current.getNeighbors()[i].getVisited())
				{
					//not previously seen
					if (current.getNeighbors()[i].getDistance() == float.MaxValue)
					{
						tilesQueue.Add(current.getNeighbors()[i]);
					}

					//if you use different terrain, not 1, have to keep track of weights
					float discreteDistance = current.getDistance() + 1;

					if (current.getNeighbors()[i].getDistance() > discreteDistance)
					{
						current.getNeighbors()[i].setDistance(discreteDistance);
						current.getNeighbors()[i].setBackPointer(current);
					}

				}
			}
		}

		temp.Clear();
		path.Clear();
		current = end;
		while (current != start && current != null)
		{
			//current is the path from each node starting from the back to the front
			temp.Add(current);
			current = current.getBackPointer();
		}
		temp.Add(start);

		for (int i = temp.Count - 1; i >= 0; i--)
		{
			temp[i].setColor(Color.green * 3);
			path.Add(temp[i]);
		}
	}

	public List<Tilescript> temp = new List<Tilescript>();
	public List<Tilescript> path = new List<Tilescript>();

	public List<Tilescript> getTilesInRange(Tilescript startTile, int range)
	{
		List<Tilescript> tilesInRange = new List<Tilescript>();
		tilesInRange.Add(startTile);

		// loop through all tiles up to the specified range
		for (int i = 0; i < range; i++)
		{
			List<Tilescript> newTiles = new List<Tilescript>();

			// loop through all tiles in the current range
			foreach (Tilescript tile in tilesInRange)
			{
				// loop through all neighbors of the current tile
				foreach (Tilescript neighbor in tile.getNeighbors())
				{
					// calculate the distance from the start tile to the neighbor tile
					float distance = Vector3.Distance(startTile.transform.position, neighbor.transform.position);

					// check if the neighbor tile is within range and not already in the list
					if (distance <= range && !tilesInRange.Contains(neighbor))
					{
						// check if the neighbor tile is adjacent to the current tile
						Vector3 direction = neighbor.transform.position - tile.transform.position;
						if (Mathf.Abs(direction.x) <= 1 && Mathf.Abs(direction.z) <= 1)
						{
							// add the neighbor tile to the new tiles list
							newTiles.Add(neighbor);
						}
					}
				}
			}

			// add the new tiles to the tiles in range list
			foreach (Tilescript tile in newTiles)
			{
				tilesInRange.Add(tile);
				tile.setColor(Color.red * 3);
			}
		}

		return tilesInRange;
	}
}