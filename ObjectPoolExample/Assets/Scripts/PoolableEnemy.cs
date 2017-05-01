using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableEnemy : Poolable 
{
	public bool hit = false;
	public override void SetUp()
	{
		//	Setup a new enemy
	}

	public override bool RePool()
	{
		return hit;
	}

	public override void Reset()
	{
		//	Respawn
	}


}
