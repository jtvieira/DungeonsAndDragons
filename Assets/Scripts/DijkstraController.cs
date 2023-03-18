using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DijkstraController : MonoBehaviour
{
	Tilescript[] tiles;
	public Tilescript start, end;

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