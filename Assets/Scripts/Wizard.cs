using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : Character
{
	public GameObject wizardGameObj;

	public void initialize(string id, GameObject wizardGameObj, float hp)
	{
		this.id = id;
		this.wizardGameObj = wizardGameObj;
		this.hp = hp;
	}

	public string getId()
	{
		return this.id;
	}

	public GameObject getWizardGameObject()
	{
		return wizardGameObj;
	}

	public void setPosition(Vector3 position)
	{
		this.position = position;
	}	
}