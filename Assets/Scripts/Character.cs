using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
	protected string id;
	protected float hp;
	protected Vector3 position;
	protected GameObject characterObject;
	
	public GameObject getCharacterGameObj()
	{
		return characterObject;
	}

	public string getId()
	{
		return this.id;
	}
}

