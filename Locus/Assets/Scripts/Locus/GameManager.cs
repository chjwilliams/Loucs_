using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChrsUtils.PrefabDataBase;
using LocusIManager;
using Locus;

public class GameManager : MonoBehaviour 
{

	public KeyCode targetYou = KeyCode.Z;
	public KeyCode targetRandom = KeyCode.X;
	public KeyCode noTargets = KeyCode.C;
	public bool alwaysWander;
	public PlayerController player;

	// Use this for initialization
	void Start () 
	{
		Services.PrefabDB = Resources.Load<PrefabDataBase>("Prefabs/PrefabDataBase");
		Services.LociManager = new LociManager();
		Services.LociManager.PopulateSpawnPoints();
		Services.LociManager.Create(30);

		alwaysWander = true;
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(targetYou))
		{
			player.clip = Resources.Load("Audio/Mallets") as AudioClip;
			for(int i = 0; i < Services.LociManager.managedObjects.Count; i++)
			{
				Services.LociManager.managedObjects[i].target = player.transform;
			}
		}
		else if (Input.GetKeyDown(targetRandom))
		{
			player.clip = Resources.Load("Audio/arpeg2") as AudioClip;
			for(int i = 0; i < Services.LociManager.managedObjects.Count; i++)
			{
				Services.LociManager.managedObjects[i].clips[0] = Resources.Load("Audio/Mallets") as AudioClip;
				Services.LociManager.managedObjects[i].RandomizeTarget();
			}
		}
		else if (Input.GetKeyDown(noTargets))
		{
			for(int i = 0; i < Services.LociManager.managedObjects.Count; i++)
			{
				alwaysWander = !alwaysWander;
				if(alwaysWander)
				{
					player.clip = Resources.Load("Audio/BellDrum") as AudioClip;
				}
				else
				{
					player.clip = Resources.Load("Audio/BellDrum2") as AudioClip;
				}
				((MelaLoci)Services.LociManager.managedObjects[i]).ReconfigureTree(alwaysWander);
			}
			
		}
	}
}
