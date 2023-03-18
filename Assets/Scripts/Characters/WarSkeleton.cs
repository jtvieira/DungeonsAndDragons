using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters 
{
	public class WarSkeleton : Character
	{
		public void initialize(string id, GameObject warSkeletonGameObj, Tilescript currentTile, float hp)
		{
			this.id = id;
			this.characterObject = warSkeletonGameObj;
			this.currentTile = currentTile;
			this.hp = hp;
		}
	}	
}