using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChrsUtils.PrefabDataBase;
using LocusIManager;

public class GameManager : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
		Services.PrefabDB = Resources.Load<PrefabDataBase>("Prefabs/PrefabDataBase");
		Services.LociManager = new LociManager();
		Services.LociManager.PopulateSpawnPoints();
		Services.LociManager.Create();
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
