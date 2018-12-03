using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeepleManager : MonoBehaviour
{

	private static MeepleManager _instance;
	public static MeepleManager getInstance()
	{
		if (!_instance)
		{
			_instance = GameObject.FindObjectOfType<MeepleManager>();
		}

		return _instance;
	}
	
	public Transform[] waypoints;

	public int[] usage;

	public GameObject meeplePrefab;
	
	

	public Transform particlePosition;
	public GameObject particle;

	private void Start()
	{
		for (int i = 0; i < 50; i++)
		{
			Vector3 v = getRandomWaypoint();
			v.y = 0f;
			Instantiate(meeplePrefab, v, Quaternion.identity);
		}
		
	}


	public Vector3 getRandomWaypoint()
	{

		return waypoints[Random.Range(0,waypoints.Length)].position;
	}

	public void showParticle(Vector3 position)
	{
		Instantiate(particle, position, Quaternion.Euler(new Vector3(-90,0,0)));
		//Instantiate(particle, particlePosition.position, Quaternion.Euler(new Vector3(-90,0,0)));
	}
	
}
