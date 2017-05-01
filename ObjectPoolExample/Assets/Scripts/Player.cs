using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 
{
	public KeyCode shoot = KeyCode.Space;
	// Use this for initialization
	void Start () {	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(shoot))
		{
			ObjectPool<PoolableBullet>.GetFromPool("Bullet");
		}
	}
}
