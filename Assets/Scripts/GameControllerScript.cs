using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour
{
	Tilescript[] tiles;

	public Tilescript start, end, allySpawn, enemySpawn;

	// Start is called before the first frame update
	void Start()
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

		spawnEntities();
	}

	private void spawnEntities()
	{
		float maxDistance = 0;

		for (int i = 0; i < tiles.Length; i++)
		{
			for (int j = i + 1; j < tiles.Length; j++)
			{
				if (Vector3.Distance(tiles[i].transform.position, tiles[j].transform.position) > maxDistance)
				{
					maxDistance = Vector3.Distance(tiles[i].transform.position, tiles[j].transform.position);
					this.allySpawn = tiles[i];
					this.enemySpawn = tiles[j];
				}
			}
		}

		// Vector3 spawnPosition = new Vector3(4, 1, 3); // Replace with your own method for calculating the spawn position
		// GameObject wizard = Resources.Load<GameObject>("Wizard"); // Replace with the path to your ally prefab
		// GameObject allyObject = Instantiate(wizard, spawnPosition, Quaternion.identity); // Spawn the prefab at the spawn position and parent it to the tile grid
		spawnAllies();
	}

	public void spawnAllies()
	{
		// ==== TEMP CODE ====
		int numWizards = PlayerPrefs.GetInt("numWizards");

		print(allySpawn.transform.position);

		Vector3 spawnPosition = allySpawn.transform.position; // Replace with your own method for calculating the spawn position
		spawnPosition.y += 1;
		GameObject wizard = Resources.Load<GameObject>("Wizard"); // Replace with the path to your ally prefab
		GameObject allyObject = Instantiate(wizard, spawnPosition, Quaternion.identity); // Spawn the prefab at the spawn position and parent it to the tile grid
	}

	public void spawnEnemies()
	{

	}

	// Update is called once per frame
	void Update()
	{
		computePath(start, end);
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
}