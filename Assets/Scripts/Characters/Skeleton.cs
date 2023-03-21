using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
	public class Skeleton : Character
	{
		public void initialize(string id, GameObject skeletonGameObj, Tilescript currentTile, float hp, int movementRange)
		{
			this.id = id;
			this.characterObject = skeletonGameObj;
			this.currentTile = currentTile;
			this.hp = hp;
			this.movementRange = movementRange;
		}
	}
}