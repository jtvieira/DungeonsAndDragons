using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
	public class Character : MonoBehaviour
	{
		protected string id;
		protected float hp;
		protected int movementRange;
		protected GameObject characterObject;
		protected Tilescript currentTile;

		public List<Spell> Spells = new List<Spell>();
        protected string listOfSpells;
		protected int slot3;
		protected int slot2;
		protected int slot1;

		public void move(Tilescript tileToMove)
		{
			this.currentTile.hasEntity = false;
			tileToMove.hasEntity = true;
			Vector3 newPosition = tileToMove.transform.position;
			newPosition.y += 1;
			this.characterObject.transform.position = newPosition;
			this.currentTile = tileToMove;
		}

		public GameObject getCharacterGameObj()
		{
			return characterObject;
		}

		public string getId()
		{
			return this.id;
		}

		public float getHp()
		{
			return this.hp;
		}

		public int getMovementRange()
		{
			return this.movementRange;
		}

		public void addSpell(Spell newSpell)
		{
			Spells.Add(newSpell); //add spell to list
		}

		public string getSpells()
		{
			return this.listOfSpells;
		}

		public string getSpellCount()
		{
			for (int i = 0; i < Spells.Count; i++)
			{
				Spell currSpell = Spells[i];
				if (currSpell.name == "MagicMissile" || currSpell.name == "HealingWord")
				{
					slot1++;
				}
				if ((currSpell.name == "ScorchingRay" && slot2 < 2) || currSpell.name == "Aid" )
				{
					slot2++;
				}
				else if (currSpell.name == "ScorchingRay" || currSpell.name == "MassHealingWord")
				{
					slot3++;
				}
			}

            string returnString = slot1.ToString() + "\n" + slot2.ToString() + "\n" + slot3.ToString();
			return returnString;
		}

		public bool isValidSpell(string input)
        {
			for (int i = 0; i < Spells.Count; i++)
			{
				Spell currSpell = Spells[i];
				if ((input == "1") && (slot1 > 0))
				{
					return true;
				}
				if ((input == "2") && (slot2 > 0))
				{
					return true;
				}
				//must be wizard
				if (((input == "4") || (input == "5")) && this.movementRange > 5 )
				{
					return true;
				}
				else if ((input == "3") && (slot3 > 0))
				{
					return true;
				}
			}
			return false;
        }

		/*public void useSpell(string spell)
		{
			for (int i = 0; i < Spells.Count; i++)
			{
				Spell currSpell = Spells[i];
				if (currSpell.name == spell && (currSpell.name == "FireBolt" || currSpell.name == "RayOfFrost"))
				{
					//Use spell and dont pop off list cause its CANTRIP type spell
					//Console.WriteLine("Using Spell: " + currSpell.name);
					break;
				}
				else if (currSpell.name == spell && (currSpell.name == "MagicMissile" || currSpell.name == "ScorchingRay")) //if there is an instance of the Spell in the list then use it
				{
					//use spell then pop it off the list 
					//Console.WriteLine("Using Spell: " + currSpell.name);
					Spells.RemoveAt(i);
					break;
				}
			}
		}*/

		public void decreaseHealth(Character character, float damage)
		{
			//function to decrease the health of a Character if attack hit 
			character.hp -= damage;
			//Console.WriteLine("After attack hp is: " + character.hp);
		}

		public void setCurrentTile(Tilescript currentTile)
		{
			this.currentTile = currentTile;
		}

		public Tilescript getCurrentTile()
		{
			return this.currentTile;
		}

		public void takeDamage(float damage)
		{
			this.hp = this.hp - damage;

			if (this.hp <= 0)
			{
				die();
			}
		}

		public void die()
		{
			this.currentTile.hasEntity = false;
			Destroy(gameObject);
		}

		public virtual int getArmorScore()
		{
			return -9999;
		}

	}
}