using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : Character
{
	public void initialize(string id, float hp)
	{
		this.id = id;
		this.hp = hp;
	}

	public void setPosition(Vector3 position)
	{
		this.position = position;
	}

	
}