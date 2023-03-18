using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters 
{
	public class WarSkeleton : Character
	{
		public void initialize(string id, GameObject warSkeletonGameObj, float hp)
		{
			this.id = id;
			this.characterObject = warSkeletonGameObj;
			this.hp = hp;
		}
	}	
}