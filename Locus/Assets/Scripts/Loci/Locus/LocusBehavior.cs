using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChrsUtils.BehaviorTree;
using Locus;

namespace LocusNodes
{
	public class LocusBehavior
	{
		/*
			Ambitious Loci : Wanders -> Flee Danger -> Flock
			Timid Loci: Flee Danger -> Flock -> Wander
			Loyal Loci: Flock -> Flee Danger -> Wander
		*/
	}

	public class DangerIsInRange : Node<BasicLocus>
	{
		public override bool Update(BasicLocus locus)
		{
			return locus.DangerCheck();
		}
	}

	public class FleeDanger : Node<BasicLocus>
	{
		public override bool Update(BasicLocus locus)
		{
			locus.isWandering = false;
			locus.Flee(locus.dangerousEntity.position);
			return true;
		}
	}

	public class Wander : Node<BasicLocus>
	{
		public override bool Update(BasicLocus locus)
		{
			float wanderMod = Mathf.PerlinNoise(0, Time.timeSinceLevelLoad) * 360f;
			locus.isWandering = true;
			locus.Wander(locus.transform.position, wanderMod);
			return true;
		}
	}

	public class FindTarget : Node<BasicLocus>
	{
		public override bool Update(BasicLocus locus)
		{
			locus.isWandering = false;
			locus.Seek(locus.target.position);
			return true;
		}
	}

	public class Flock : Node<BasicLocus>
	{
		public override bool Update(BasicLocus locus)
		{
			locus.isWandering = false;
			//	Sets less ambitious loci to flock behind an ambitious locus
			return true;
		}
	}
}