using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
	private Tilescript[] tiles;
    private Tilescript allySpawn, enemySpawn;

	private int numWizards;
	private int numClerics;
	private int numSkeletons;
	private int numWarSkeletons;

	private List<GameObject> wizards;

	public void initialize(Tilescript[] tiles)
	{
		this.tiles = tiles;

		this.numWizards = PlayerPrefs.GetInt("numWizards");
		this.numClerics = PlayerPrefs.GetInt("numClerics");
		this.numSkeletons = PlayerPrefs.GetInt("numSkeletons");
		this.numWarSkeletons = PlayerPrefs.GetInt("numWarSkeletons");

		for (int i = 0; i < numWizards; i++)
		{
			wizards.Add(Resources.Load<GameObject>("Wizard"));
		}
	}

	public void spawnEntities()
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

		spawnAllies();
	}

	private void spawnAllies()
	{
		// ==== TEMP CODE ====
		int numWizards = PlayerPrefs.GetInt("numWizards");

		Tilescript tileToSpawn = allySpawn;
		tileToSpawn.hasEntity = true;
		Vector3 spawnPosition;

		for (int i = 0; i < numWizards; i++)
		{
			spawnPosition = tileToSpawn.transform.position;
			spawnPosition.y += 1;
			GameObject wizard = Resources.Load<GameObject>("Wizard"); // Replace with the path to your ally prefab
			GameObject allyObject = Instantiate(wizard, spawnPosition, Quaternion.identity); // Spawn the prefab at the spawn position and parent it to the tile grid

			tileToSpawn.hasEntity = true;
			tileToSpawn = findClosestEmptyTileToSpawn(tileToSpawn);
		}
	}

	public void spawnEnemies()
	{

	}

	public Tilescript findClosestEmptyTileToSpawn(Tilescript tile)
	{
		float minDistance = 999999;
		Tilescript closestNeighboringTile = null;

		for (int i = 0; i < tile.neighbors.Count; i++)
		{
			float distToNeighb = Vector3.Distance(tile.neighbors[i].transform.position, allySpawn.transform.position);
			print(distToNeighb);
			if (distToNeighb < minDistance && tile.neighbors[i].hasEntity == false)
			{
				minDistance = distToNeighb;
				closestNeighboringTile = tile.neighbors[i];
			}
		}

		return closestNeighboringTile;
	}

}
