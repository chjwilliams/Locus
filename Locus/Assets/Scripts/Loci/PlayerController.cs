using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
	public float moveSpeed = 20.0f;

	private Rigidbody _rigidbody;
	private const string Y_AIXS = "Vertical";
	private const string X_AXIS = "Horizontal";

	// Use this for initialization
	void Start () 
	{
		_rigidbody = GetComponent<Rigidbody>();
	}

	private void Move(float dx, float dy)
	{
		Vector3 movement = new Vector3(dx, 0, dy) ;
		_rigidbody.AddForce(movement * moveSpeed * Time.deltaTime, ForceMode.Impulse);
	}
	
	// Update is called once per frame
	void Update () 
	{
		float y = Input.GetAxis(Y_AIXS);
		float x = Input.GetAxis(X_AXIS);

		Move(x,y);
	}
}
