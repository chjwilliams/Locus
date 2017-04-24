using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChrsUtils.ChrsManagerSystem.SimpleManager;
using Locus;

namespace LocusIManager
{
	public class LociManager : Manager<BasicLocus>
	{
		public GameObject[] spawnPoints;
		private readonly System.Random _rng = new System.Random();

		public void PopulateSpawnPoints()
        {
            spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint"); 
        }

		public override BasicLocus Create()
        {
            BasicLocus locus = Init(GetRandomLocusType());
            
            ManagedObjects.Add(locus);
            locus.OnCreated();
            return locus;
        }

		public override void Destroy(BasicLocus locus)
        {
            ManagedObjects.Remove(locus);
            locus.OnDestroyed();
        }

		private LocusType GetRandomLocusType()
        {   
			
            //LocusType thisType = (LocusType)(_rng.Next() % 0);
            //thisType = (LocusType)(_rng.Next() % 0);
            //return thisType;
			return LocusType.Mela;
        }

		public BasicLocus Init(LocusType locusType)
		{
            BasicLocus locus = null;
			GameObject newLocus = MonoBehaviour.Instantiate(Services.PrefabDB.Locus[0], spawnPoints[0].transform.position, Quaternion.identity) as GameObject;
            
			locus = new BasicLocus();
			switch (locusType.ToString())
			{
			    case "TRIANGLE_PREFAB":
	    		   	newLocus.AddComponent <MelaLoci> ();
				    locus = newLocus.GetComponent <MelaLoci> ();
				   	break;
				case "SQUARE_PREFAB":
				   	newLocus.AddComponent <PhleLoci> ();
			    	locus = newLocus.GetComponent <PhleLoci> ();
			    	break;
                case "PENTAGON_PREFAB":
			    	newLocus.AddComponent <CholLoci> ();
				   	locus = newLocus.GetComponent <CholLoci> ();
			    	break;
                case "HEXAGON_PREFAB":
			    	newLocus.AddComponent <SangLoci> ();
			    	locus = newLocus.GetComponent <SangLoci> ();
			    	break;
				default:
					newLocus.AddComponent <MelaLoci>();
					locus = newLocus.GetComponent <MelaLoci>();
					break;
			    }

			return locus;
        }	
	}
}
