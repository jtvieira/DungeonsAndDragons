using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleric : Character
{
	public void initialize(string id, GameObject clericGameObj, float hp)
	{
		this.id = id;
		this.characterObject = clericGameObj;
		this.hp = hp;
	}
}