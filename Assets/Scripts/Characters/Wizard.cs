using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters 
{
	public class Wizard : Character
	{
		public void initialize(string id, GameObject wizardGameObj, Tilescript currentTile,float hp, int movementRange)
		{
			this.listOfSpells = "Magic Missle\nScorching Ray\nScorching Ray\n4: Ray of Frost ∞\n5: Fire Bolt ∞";
			this.id = id;
			this.characterObject = wizardGameObj;
			this.currentTile = currentTile;
			this.hp = hp;
			this.movementRange = movementRange;

			addSpell(new FireBolt());       //Add spells into List
			addSpell(new RayOfFrost());
			addSpell(new MagicMissile());
			addSpell(new MagicMissile());
			addSpell(new ScorchingRay());
			addSpell(new ScorchingRay());
			addSpell(new ScorchingRay());

			slot1 = 2;
			slot2 = 2;
			slot3 = 1;
		}

		// wizards can have studded leather armor (12)
		public override int getArmorScore()
		{
			return 12;
		}
	}
}