using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class CemeraFollow : MonoBehaviour
{

	public Transform targett;
	private Vector3 Dist;
	
	public float followSpeed;
	
	// Use this for initialization
	void Start ()
	{
		Dist = transform.position - targett.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.position = Vector3.Slerp(transform.position, targett.position + Dist, followSpeed);
	}
	
	
}
