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
		public List<BasicLocus> managedObjects = ManagedObjects;
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

		public List<BasicLocus> Create(uint n)
        {
            List<BasicLocus> loci = null;

            loci = new List<BasicLocus>();
            for (var i = 0; i < n; i++)
            {
                loci.Add(Create());
            }
            return loci;
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
			GameObject newLocus = MonoBehaviour.Instantiate(Services.PrefabDB.Locus[0], spawnPoints[_rng.Next(0, spawnPoints.Length)].transform.position, Quaternion.identity) as GameObject;
            
			locus = new BasicLocus();
			switch (locusType.ToString())
			{
			    case "0":
	    		   	newLocus.AddComponent <MelaLoci> ();
				    locus = newLocus.GetComponent <MelaLoci> ();
				   	break;
				case "1":
				   	newLocus.AddComponent <PhleLoci> ();
			    	locus = newLocus.GetComponent <PhleLoci> ();
			    	break;
                case "2":
			    	newLocus.AddComponent <CholLoci> ();
				   	locus = newLocus.GetComponent <CholLoci> ();
			    	break;
                case "3":
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
