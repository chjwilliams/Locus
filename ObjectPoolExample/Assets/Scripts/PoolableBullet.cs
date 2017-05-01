using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableBullet : Poolable 
{
	public float bulletSpeed = 400.0f;
	public Vector3 bulletOffset;
	public Player player;
	public float maxDistance;

	public override void SetUp()
	{
		bulletOffset = Vector3.up;
		player = GameObject.Find("Player").GetComponent<Player>();
		maxDistance = 10.0f;
	}

	public override bool RePool()
	{
		return Vector3.Distance(player.transform.position, transform.position) > maxDistance;
	}

	public override void Reset()
	{
		if(player == null)
		{
			player = GameObject.Find("Player").GetComponent<Player>();
		}

		Rigidbody rigidbody = GetComponent<Rigidbody>();
		
		transform.position = player.transform.position + bulletOffset;
		rigidbody.velocity = Vector3.zero;
		rigidbody.AddForce(Vector3.up * bulletSpeed);
	}
}
