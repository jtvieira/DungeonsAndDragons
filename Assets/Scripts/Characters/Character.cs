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