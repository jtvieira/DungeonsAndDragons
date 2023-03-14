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

	private Dictionary<string, Character> characterDict;

	public void initialize(Tilescript[] tiles)
	{
		this.tiles = tiles;

		this.numWizards = PlayerPrefs.GetInt("numWizards");
		this.numClerics = PlayerPrefs.GetInt("numClerics");
		this.numSkeletons = PlayerPrefs.GetInt("numSkeletons");
		this.numWarSkeletons = PlayerPrefs.GetInt("numWarSkeletons");

		characterDict = new Dictionary<string, Character>();
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
		spawnEnemies();
	}

	private void spawnAllies()
	{
		Tilescript tileToSpawn = allySpawn;
		tileToSpawn.hasEntity = true;
		Vector3 spawnPosition;

		for (int i = 0; i < numWizards; i++)
		{
			spawnPosition = tileToSpawn.transform.position;
			spawnPosition.y += 1;

			GameObject wizardObject = Resources.Load<GameObject>("Wizard");

			GameObject instantiatedObject = Instantiate(wizardObject, spawnPosition, Quaternion.identity);
			Wizard wizard = instantiatedObject.GetComponent<Wizard>();
			wizard.initialize("wizard" + i, instantiatedObject, 100f);

			characterDict.Add("wizard" + i, wizard);

			tileToSpawn.hasEntity = true;
			tileToSpawn = findClosestEmptyTileToSpawn(tileToSpawn);
		}

		for (int i = 0; i < numClerics; i++)
		{
			spawnPosition = tileToSpawn.transform.position;
			spawnPosition.y += 1;

			GameObject clericObject = Resources.Load<GameObject>("Cleric");

			GameObject instantiatedObject = Instantiate(clericObject, spawnPosition, Quaternion.identity);
			Cleric cleric = instantiatedObject.GetComponent<Cleric>();
			cleric.initialize("cleric" + i, instantiatedObject, 100f);

			characterDict.Add("cleric" + i, cleric);

			tileToSpawn.hasEntity = true;
			tileToSpawn = findClosestEmptyTileToSpawn(tileToSpawn);
		}
	}

	private void spawnEnemies()
	{
		Tilescript tileToSpawn = enemySpawn;
		tileToSpawn.hasEntity = true;
		Vector3 spawnPosition;

		for (int i = 0; i < numSkeletons; i++)
		{
			spawnPosition = tileToSpawn.transform.position;
			spawnPosition.y += 1;

			GameObject skeletonObject = Resources.Load<GameObject>("Skeleton");
			GameObject instantiatedObject = Instantiate(skeletonObject, spawnPosition, Quaternion.identity);
			Skeleton skeleton = instantiatedObject.GetComponent<Skeleton>();
			skeleton.initialize("skeleton" + i, instantiatedObject, 100f);

			characterDict.Add("skeleton" + i, skeleton);

			tileToSpawn.hasEntity = true;
			tileToSpawn = findClosestEmptyTileToSpawn(tileToSpawn);
		}

		for (int i = 0; i < numWarSkeletons; i++)
		{
			spawnPosition = tileToSpawn.transform.position;
			spawnPosition.y += 1;

			GameObject warSkeletonObject = Resources.Load<GameObject>("WarSkeleton");

			GameObject instantiatedObject = Instantiate(warSkeletonObject, spawnPosition, Quaternion.identity);
			WarSkeleton warSkeleton = instantiatedObject.GetComponent<WarSkeleton>();
			warSkeleton.initialize("warSkeleton" + i, instantiatedObject, 100f);

			characterDict.Add("warSkeleton" + i, warSkeleton);

			tileToSpawn.hasEntity = true;
			tileToSpawn = findClosestEmptyTileToSpawn(tileToSpawn);
		}
	}

	public Dictionary<string, Character> getCharacters()
	{
		return characterDict;
	}

	public Tilescript findClosestEmptyTileToSpawn(Tilescript tile)
	{
		float minDistance = 999999;
		Tilescript closestNeighboringTile = null;

		for (int i = 0; i < tile.neighbors.Count; i++)
		{
			float distToNeighb = Vector3.Distance(tile.neighbors[i].transform.position, allySpawn.transform.position);
			if (distToNeighb < minDistance && tile.neighbors[i].hasEntity == false)
			{
				minDistance = distToNeighb;
				closestNeighboringTile = tile.neighbors[i];
			}
		}

		return closestNeighboringTile;
	}
}
