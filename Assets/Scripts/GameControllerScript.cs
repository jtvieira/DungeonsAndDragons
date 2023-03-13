using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameControllerScript : MonoBehaviour
{
	Tilescript[] tiles;

	public Tilescript start, end;

	private SpawnController spawnController;

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

		// Note: Was getting an error that I could not initialize an object using 'new' if the obj was monobehavior
		// spawnController = new SpawnController(tiles);
		// spawnController.spawnEntities();

		GameObject spawnControllerObject = new GameObject("SpawnController");
		SpawnController spawnController = spawnControllerObject.AddComponent<SpawnController>();
		spawnController.initialize(tiles);

		spawnController.spawnEntities();

		// === TEST CODE ===
		// This code demonstrates the simple nature of the dictionary!

		Dictionary<string, Wizard> wizards = spawnController.getWizards();

		Wizard test = wizards["wizard0"];
		GameObject temp = test.getWizardGameObject();
		// temp.transform.position = new Vector3(5,1,3);
		// Renderer renderer = temp.GetComponent<Renderer>(); // Get the Renderer component of the game object
		// Material material = renderer.material; // Get the Material component of the Renderer
		// material.color = Color.red;
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