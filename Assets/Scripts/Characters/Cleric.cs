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

		// clerics can have padded leather armor (12)
		public override int getArmorScore()
		{
			return 11;
		}
	}
}