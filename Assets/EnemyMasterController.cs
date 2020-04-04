using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class EnemyMasterController : MonoBehaviour
{
	[field: SerializeField]
	public float timeBetweenSpawns { get; set; }            //how long between enemy spawns
	[field: SerializeField]
	public float xBound { get; set; }                          //map xbound
	[field: SerializeField]
	public float zBound { get; set; }                       //map zbound
	[field: SerializeField]
	public int numberToSpawn { get; set; }

	private float nextSpawnTime;
	private bool canSpawn;

	// Start is called before the first frame update
	void Start()
	{
		nextSpawnTime = Time.time + timeBetweenSpawns;
		canSpawn = true;
	}

	// Update is called once per frame
	void Update()
	{
		if (canSpawn)
		{
			spawnEnemy();
			canSpawn = false;
			nextSpawnTime = Time.time + timeBetweenSpawns;
		}
		else if (Time.time >= nextSpawnTime)
		{
			canSpawn = true;
		}
	}

	void spawnEnemy()
	{
		//for now, randomly select 1 of 4 sides, spawn enemies
		int[] choices = { -1, 1 };
		
		Vector3 spawnPoint;
		spawnPoint.x = (xBound - 5) * choices[Random.Range(0, 2)];          //Random.Range is exclusive of max, so gives either 0 or 1
		spawnPoint.z = (zBound - 5) * choices[Random.Range(0, 2)];
		spawnPoint.y = 0;

		for (int i = 0; i < numberToSpawn; i++)
		{
			GameObject newEnemy = Instantiate(Resources.Load("Enemy"), spawnPoint + i*Vector3.forward, transform.rotation) as GameObject;
			newEnemy.GetComponent<NavMeshAgent>().SetDestination(this.transform.position);
		}
	}
}
