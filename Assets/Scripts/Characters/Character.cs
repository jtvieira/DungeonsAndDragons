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
		protected Vector3 position;
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
	}
}


// Attach this script to your game object that contains the grid of characters
public class CharacterClickHandler : MonoBehaviour {
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            // RaycastHit hit;
            // if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) {
            //     // Check if the object we clicked on has a CharacterData script attached
            //     Character characterData = hit.collider.gameObject.GetComponent<Character>();
            //     if (characterData != null) {
            //         // Output the character's data to the console
            //         Debug.Log("Clicked on character " + characterData.characterName + " with " + characterData.health + " health.");
            //     }
            // }
			print("test");
        }
    }
}