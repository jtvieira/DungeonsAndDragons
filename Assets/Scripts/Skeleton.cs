using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Character
{
	public void initialize(string id, GameObject skeletonGameObj, float hp)
	{
		this.id = id;
		this.characterObject = skeletonGameObj;
		this.hp = hp;
	}
}