using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
	public class Cleric : Character
	{
		public void initialize(string id, GameObject clericGameObj, Tilescript currentTile, float hp, int movementRange)
		{
			this.id = id;
			this.characterObject = clericGameObj;
			this.currentTile = currentTile;
			this.hp = hp;
			this.movementRange = movementRange;
		}
	}
}