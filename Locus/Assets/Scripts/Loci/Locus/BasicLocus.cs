using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChrsUtils.ChrsExtensionMethods;
using ChrsUtils.ChrsManagerSystem.SimpleManager;
using ChrsUtils.BehaviorTree;
using LocusNodes;

namespace Locus
{
	public enum LocusType
	{
        Mela,	// Earth Avoid
        Phle,	//	Water Getting
        Sang,	//	Air Social
        Chol,	//	Fire Ruling
	}

	public class BasicLocus : MonoBehaviour,  IManaged
	{
		public bool isWandering;
		public bool inDanger;
		public float maxSpeed;
		public float wanderSpeed;
		public float maxForce;
		public float visibilityRange;
		public float wanderTheta;
		public float fleeTimer;
		public float safetyDistance;
		public float pounceDistance;
		public AudioClip clip;
		public Transform target;
		public Transform dangerousEntity;
		public Color defaultColor;
		public Color secondaryColor;
		public Color tertiaryColor;

		protected Vector3 velocity;
		protected Vector3 acceleration;
		protected AudioSource m_AudioSource;
		protected Rigidbody m_Rigidbody;
		protected Tree<BasicLocus> m_Tree;

	

		// Use this for initialization
		protected virtual void Start () 
		{
			m_Rigidbody = GetComponent<Rigidbody>();
			wanderSpeed  = maxSpeed * 0.1f;
		}

		public void OnCreated()
		{

		}

		public void OnDestroyed()
		{

		}

		public bool DangerCheck()
		{
			if (Vector3.Distance(dangerousEntity.position, transform.position) < visibilityRange)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		protected void Move(Vector3 direction)
		{
			m_Rigidbody.AddForce(direction, ForceMode.Impulse);
		}

		public virtual void Seek(Vector3 targetPosition)
		{
			Vector3 desiredVelocity = targetPosition - transform.position;
			float d = desiredVelocity.magnitude;
			desiredVelocity.Normalize();

			if(d < pounceDistance)
			{
				float m = ExtensionMethods.Map(d, 0, pounceDistance, 0, maxSpeed);
				desiredVelocity = desiredVelocity * m;
			}
			else
			{
				if(isWandering)
				{
					desiredVelocity = desiredVelocity * wanderSpeed;
				}
				else
				{
					desiredVelocity = desiredVelocity * maxSpeed;
				}
			}

			Vector3 steeringForce = desiredVelocity - velocity;

			Vector3 direction = Vector3.ClampMagnitude(steeringForce, maxForce);

			Move(direction);
		}

		public virtual void Wander(Vector3 currentPosition, float angle)
		{
			Vector3 predictedPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z) + m_Rigidbody.velocity * Time.deltaTime;
			Vector2 newTargetPosition = ExtensionMethods.GetPointOnCircle(visibilityRange,angle, predictedPoint);
			newTargetPosition = Vector3.ClampMagnitude(newTargetPosition, maxForce);
			Seek(new Vector3(newTargetPosition.x, 0, newTargetPosition.y));
		}

		public virtual void Flee(Vector3 targetPosition)
		{
			Vector3 desiredVelocity = transform.position - targetPosition;
			float d = desiredVelocity.magnitude;
			desiredVelocity.Normalize();

			if(d < pounceDistance)
			{
				float m = ExtensionMethods.Map(d, 0, pounceDistance, 0, maxSpeed);
				desiredVelocity = desiredVelocity * m;
			}
			else
			{
				desiredVelocity = desiredVelocity * maxSpeed;
			}

			Vector3 steeringForce = desiredVelocity - velocity;

			Vector3 direction = Vector3.ClampMagnitude(steeringForce, maxForce);

			Move(direction);
		}

		protected virtual void Flock(Transform leader)
		{

		}

		protected virtual void OnCollisionEnter(Collision cols)
		{

		}

		protected virtual void FixedUpdate () 
		{
			velocity += acceleration;
			Vector3.ClampMagnitude(velocity, maxSpeed);
			acceleration = Vector3.zero;

			m_Tree.Update(this);
		}
	}
}