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
		protected float speed = 5f;
		

		// public void move(List<Tilescript> movePath)
		// {
		// 	this.currentTile.hasEntity = false;
		// 	tileToMove.hasEntity = true;
		// 	Vector3 newPosition = tileToMove.transform.position;
		// 	newPosition.y += 1;
		// 	this.characterObject.transform.position = newPosition;
		// 	this.currentTile = tileToMove;
		// }

		public IEnumerator Move(List<Tilescript> movePath){
			this.currentTile.hasEntity = false;
			Tilescript tileToMove = null;
			foreach (Tilescript tile in movePath)
			{
				tileToMove = tile;
				if (tileToMove != null)
				{
					// Calculate the direction and distance to the next tile
					Vector3 direction = (tile.transform.position - this.getCurrentTile().transform.position).normalized;
					float distance = Vector3.Distance(tile.transform.position, this.getCurrentTile().transform.position);

					// Move the character incrementally towards the next tile
					while (distance > 0)
					{
						float step = Mathf.Min(speed * Time.deltaTime, distance);
						this.characterObject.transform.position += direction * step;
						distance -= step;
						yield return null;
					}

					// Update the current tile of the character
					this.currentTile = tileToMove;
				}

				
			}

			// Update the current tile of the character for the last tile in the path
			if (tileToMove != null)
			{
				tileToMove.hasEntity = true;
				Vector3 newPosition = tileToMove.transform.position;
				newPosition.y += 1;
				this.characterObject.transform.position = newPosition;
				this.currentTile = tileToMove;
			}
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
            string returnString = slot1.ToString() + "\n" + slot2.ToString() + "\n" + slot3.ToString();
			return returnString;
		}

		public bool isValidSpell(string input)
        {
				if ((input == "1") && (slot1 > 0))
				{
					slot1--;
					return true;
				}
				if ((input == "2") && (slot2 > 0))
				{
					slot2--;
					return true;
				}
				if ((input == "4") && this.movementRange <= 5)
				{
					return true;
				}
			//must be wizard
			if (((input == "4") || (input == "5")) && this.movementRange > 5 )
				{
					return true;
				}
				if ((input == "3") && (slot3 > 0))
				{
					slot3--;
					return true;
				}
				else
					return false;
        }

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
			this.hp -= damage;
			// this.hp -=15;
			// Create a new particle system
			ParticleSystem particleSystem = new GameObject("Particle System").AddComponent<ParticleSystem>();

			// Set particle system properties
			particleSystem.transform.position = this.characterObject.transform.position;
			particleSystem.transform.rotation = this.characterObject.transform.rotation;
			particleSystem.gameObject.layer = this.characterObject.layer;

			// Set particle system main properties
			var particleMain = particleSystem.main;
			particleMain.startSize = 0.2f;
			particleMain.startLifetime = 0.5f;
			particleMain.startSpeed = 1f;
			particleMain.startColor = new Color(0.5f, 0.5f, 0.5f, 1f);

			// Set particle system emission properties
			var particleEmission = particleSystem.emission;
			particleEmission.rateOverTime = 10f;

			// Play the particle system
			particleSystem.Play();

			// Stop the particle system after 3 seconds
			StartCoroutine(StopParticleSystem(particleSystem, 3f));

			if (this.hp <= 0)
			{
				die();
				Destroy(particleSystem.gameObject);
			}
		}

		private IEnumerator StopParticleSystem(ParticleSystem particleSystem, float delay)
		{
			// Wait for the specified delay
			yield return new WaitForSeconds(delay);

			// Stop the particle system emitting new particles
			particleSystem.Stop();

			// Wait for the remaining particles to expire
			yield return new WaitForSeconds(particleSystem.main.duration);

			// Destroy the particle system object
			Destroy(particleSystem.gameObject);
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

		//I just hard coded this stuff- made it easier for me to work faster
		public int getSpellRange(string i)
        {
			//wizard
			if((i == "1" || i == "2" || i == "3" || i == "4") && this.movementRange > 5)
				return 24;
			if (i == "5")
				return 12;
			//cleric
			if ((i == "1" || i == "3") && this.movementRange <= 5)
				return 12;
			if ((i == "2" && this.movementRange <= 5))
				return 6;
			if ((i == "4" && this.movementRange <= 5))
				return 1;
			else
				return 0;
        }

		//Other variables
		static System.Random random = new System.Random(); //This is for the random dice roller

		//Function that rolls specific diceType for spell
		public static float rollDice(float diceType)
		{
			return (float)random.Next(1, (int)diceType + 1);
		}

		public float getSpellDamage(string i)
        {
			//wizard
			if (i == "1" && this.movementRange > 5)
            {
				float totalRollResult = rollDice(4) + 1;
				return totalRollResult;
			}
			if ((i == "2" || i == "3" || i == "4") && this.movementRange > 5)
			{
				float rollResult1 = rollDice(6);
				float rollResult2 = rollDice(6);
				float totalRollResult = rollResult1 + rollResult2;
				return totalRollResult;
			}
			if (i == "4" && this.movementRange > 5)
			{
				float rollResult1 = rollDice(10);
				float rollResult2 = rollDice(10);
				float totalRollResult = rollResult1 + rollResult2;
				return totalRollResult;
			}
			if (i == "5")
			{
				float rollResult1 = rollDice(8);
				float rollResult2 = rollDice(8);
				float totalRollResult = rollResult1 + rollResult2;
				return totalRollResult;
			}
			//cleric
			if (i == "1" && this.movementRange <= 5)
            {
				float totalRollResult = rollDice(4);
				return totalRollResult * -1;
			}
			if (i == "3" && this.movementRange <= 5)
			{
				float totalRollResult = rollDice(4);
				this.hp += totalRollResult;
				return totalRollResult * -1;
			}
			if (i == "4" && this.movementRange <= 5)
			{
				float rollResult1 = rollDice(12);
				float totalRollResult = rollResult1;
				return totalRollResult * -1;
			}
			if ((i == "2" && this.movementRange <= 5))
				return 5;
			else
				return 0;
		}
	}
}