using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters 
{
	public class Wizard : Character
	{
		public void initialize(string id, GameObject wizardGameObj, Tilescript currentTile,float hp, int movementRange)
		{
			this.id = id;
			this.characterObject = wizardGameObj;
			this.currentTile = currentTile;
			this.hp = hp;
		}
	}
}