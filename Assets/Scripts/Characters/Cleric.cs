using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
	public class Cleric : Character
	{
		public void initialize(string id, GameObject clericGameObj, Tilescript currentTile, float hp, int movementRange)
		{
			this.listOfSpells = "1: Healing Word\n2: Aid\n3: Mass Healing Word\n4: Great Axe ∞";
			this.id = id;
			this.characterObject = clericGameObj;
			this.currentTile = currentTile;
			this.hp = hp;
			this.movementRange = movementRange;

			addSpell(new HealingWord());       //Add spells into List
			addSpell(new HealingWord());
			addSpell(new HealingWord());
			addSpell(new Aid());
			addSpell(new Aid());
			addSpell(new MassHealingWord());
			addSpell(new GreatAxe());

			slot1 = 3;
			slot2 = 2;
			slot3 = 1;
		}

		// clerics can have padded leather armor (12)
		public override int getArmorScore()
		{
			return 11;
		}
	}
}