using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChrsUtils.ChrsExtensionMethods;
using ChrsUtils.ChrsManagerSystem.SimpleManager;
using ChrsUtils.BehaviorTree;
using LocusNodes;
using LocusIManager;

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
		public static int zoneSize = 10;

		public bool isFlocking;
		public bool isWandering;
		public bool inDanger;
		public float maxSpeed;
		public float wanderSpeed;
		public float maxForce;
		public float visibilityRange;
		public float fleeTimer;
		public float safetyDistance;
		public float pounceDistance;
		public float neighborDistance;
		public float rotationSpeed;
		public int groupSize;
		public AudioClip clip;
		public Transform target;
		public Transform dangerousEntity;
		public Color defaultColor;
		public Color secondaryColor;
		public Color tertiaryColor;

		protected Vector3 newTargetPosition = Vector3.zero;
		protected Vector3 velocity;
		protected Vector3 acceleration;
		protected AudioSource m_AudioSource;
		protected Rigidbody m_Rigidbody;
		protected Tree<BasicLocus> m_Tree;
		protected LociManager myManager;

	

		// Use this for initialization
		protected virtual void Start () 
		{
			maxSpeed = Random.Range(5.0f, 15.0f);
			maxForce = Random.Range(1.0f, 4.0f);
			visibilityRange = Random.Range(1.0f, 4.0f);
			m_Rigidbody = GetComponent<Rigidbody>();
			wanderSpeed  = maxSpeed * 0.1f;
			myManager = Services.LociManager;
			neighborDistance = Random.Range(1.0f, 3.0f);
			rotationSpeed = Random.Range(1.0f, 4.0f);
			groupSize = 0;
			//visibilityRange = 0.5f;
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
			
			if(Random.Range(0, 10000) < 50)
			{
				newTargetPosition = new Vector2(Random.Range(-zoneSize, zoneSize), Random.Range(-zoneSize, zoneSize));
				Vector3 predictedPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z) + m_Rigidbody.velocity * Time.deltaTime;
				Seek(new Vector3(newTargetPosition.x, 0, newTargetPosition.y));
			}
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

		public virtual void Flock()
		{
			float moveSpeed = 0.1f;
			
			Vector3 averageHeadingDirection;
			Vector3 averagePostion;
			

		}

		public void ApplyFlockingRules()
		{
			Vector3 centerOfGroup = Vector3.zero;
			Vector3 avoidance = Vector3.zero;
			float groupSpeed = 1.0f;

			float distance;
			newTargetPosition = centerOfGroup;
			groupSize = 0;
			foreach(BasicLocus loci in myManager.managedObjects)
			{
				if(loci.gameObject != this.gameObject)
				{
					distance = Vector3.Distance(loci.transform.position, transform.position);
					if(distance <= neighborDistance)
					{
						centerOfGroup += loci.transform.position;
						groupSize++;

						if(distance < 0.5f)
						{
							avoidance =  avoidance + (transform.position - loci.transform.position);
						}

						groupSpeed =  groupSpeed + loci.velocity.magnitude;
					}
				}
			}

			if (groupSize > 0)
			{
				isFlocking = true;
				centerOfGroup  = centerOfGroup / groupSize + (newTargetPosition - transform.position);
				maxSpeed = groupSpeed / groupSize;

				Vector3 direction = (centerOfGroup + avoidance) - transform.position;
				
				if(direction != Vector3.zero)
				{
					transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
				}
			}
			else
			{
				isFlocking = false;
			}
		}

		protected virtual void OnCollisionEnter(Collision cols)
		{

		}

		protected virtual void FixedUpdate () 
		{
			velocity += acceleration;
			Vector3.ClampMagnitude(velocity, maxSpeed);
			acceleration = Vector3.zero;


			ApplyFlockingRules();
			// float wanderMod = Mathf.PerlinNoise(0, Time.timeSinceLevelLoad) * 360f;
			// isWandering = true;
			// Wander(transform.position, wanderMod);
			m_Tree.Update(this);
		}
	}
}