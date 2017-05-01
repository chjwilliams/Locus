using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Poolable : MonoBehaviour 
{	
	void Start()
	{
		SetUp();
	}

	public void Init()
	{
		
	}

	public void ToggleActive(bool b)
	{
		gameObject.SetActive(b);
	}

	public abstract void SetUp();
	public abstract bool RePool();

	public abstract void Reset();

	// Update is called once per frame
	void Update () 
	{
		if(RePool())
		{
			ObjectPool<Poolable>.AddToPool(gameObject.GetComponent<Poolable>());
		}	
	}
}
