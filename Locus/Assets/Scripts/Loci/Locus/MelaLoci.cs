using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChrsUtils.BehaviorTree;
using LocusNodes;

namespace Locus
{

	//	Earth Avoiding
	public class MelaLoci : BasicLocus 
	{
		// Use this for initialization
		override protected void Start () 
		{
			base.Start();
			if(Random.Range(0, 10) < 1)
			{
				dangerousEntity = myManager.managedObjects[Random.Range(0, myManager.managedObjects.Count)].transform;
			}
			else
			{
				dangerousEntity = GameObject.FindGameObjectWithTag("Player").transform;
			}
			m_Tree = 
			new Tree<BasicLocus>
			(
				new Selector<BasicLocus>
				(
					//	Flee
					new Sequence<BasicLocus>
					(
						new DangerIsInRange(),
						new FleeDanger()
					),
					//	Wander
					new Sequence<BasicLocus>
					(
						new Wander()
					),
					//	Seek
					new Sequence<BasicLocus>
					(
						new FindTarget()
					)
				)
			);
		}
	
	}
}