using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : Character
{
	public void initialize(string id, GameObject wizardGameObj, float hp)
	{
		this.id = id;
		this.characterObject = wizardGameObj;
		this.hp = hp;
	}
}