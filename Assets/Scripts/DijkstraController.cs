using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DijkstraController : MonoBehaviour
{
	Tilescript[] tilesComputePath;
	public Tilescript start, end;

	Tilescript[] allTiles;

	List<Tilescript> tilesQueue = new List<Tilescript>();

	public void computePath(Tilescript start, Tilescript end)
	{
		//clear all nodes from past computing
		for (int i = 0; i < tilesComputePath.Length; i++)
		{
			tilesComputePath[i].clear();
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
		HashSet<Tilescript> tilesInRange = new HashSet<Tilescript>();
		tilesInRange.Add(startTile);

		// loop through all tiles up to the specified range
		for (int i = 0; i < range; i++)
		{
			HashSet<Tilescript> newTiles = new HashSet<Tilescript>();

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
			}
		}
		
		return tilesInRange.ToList();
	}

	public bool isValidCoordinate(List<Tilescript> tiles, string coordinate)
	{
		foreach (Tilescript tile in tiles)
		{
			if (coordinate == tile.getCoordinate() && tile.hasEntity == false)
				return true;
		}
		return false;
	}

	public Tilescript getTileFromCoordinate(string coordinate)
	{
		foreach (Tilescript tile in allTiles)
		{
			if (tile.coordinate == coordinate)
				return tile;
		}
		Debug.Log("Error: no tile found");
		return null;
	}

	public void colorTiles(List<Tilescript> tilesToColor, string colorString)
	{
		if (colorString == "red")
		{
			foreach (Tilescript tile in tilesToColor)
			{
				tile.setColor(Color.red * 3);
			}
		}
		else if (colorString == "white")
		{
			foreach (Tilescript tile in tilesToColor)
			{
				tile.setColor(Color.white * 3);
			}
		}

	}

	public void setAllTiles(Tilescript[] tiles)
	{
		this.allTiles = tiles;
	}
}