using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Poolable
{
	private static Queue<T> pool = new Queue<T>();

	public static T GetFromPool(string type)
	{
		GameObject gObj;
		T obj;
		Debug.Log(pool.Count);
		if(pool.Count > 0)
		{
			obj = pool.Dequeue();
			obj.ToggleActive(true);
		}
		else
		{
			Debug.Log("Init");
			gObj = MonoBehaviour.Instantiate(Resources.Load(type)) as GameObject;
			obj = gObj.GetComponent<T>();	
		}

		if(obj == null)
		{
			Debug.Log("!!!!!!!!");
		}

		obj.Reset();

		return obj;
	}

	public static void AddToPool(T obj)
	{
		obj.ToggleActive(false);
		// It's not adding to the pool
		pool.Enqueue(obj);
	}
}
