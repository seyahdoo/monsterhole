using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
	//hello from git
	public Animator anim;
	public Rigidbody rb;
	
	public Camera cam;
	public LayerMask mask;
	public float speed;

	public GameObject handTrigger;
	public float handTriggerStart;
	public float handTriggerStay = 1f;

	public Transform meepleHolder;

	public Meeple carriedMeeple;

	public bool working = false;

	private static Boss _instance;
	public static Boss GetBoss()
	{
		if (!_instance)
		{
			_instance = GameObject.FindObjectOfType<Boss>();
		}
		
		return _instance;
	}

	public void EnableMoving()
	{
		working = true;
	}

	// Update is called once per frame
	private void Update () {

		if (!working) return;
		
		RaycastHit hit;
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        
		if (Physics.Raycast(ray, out hit, 100f, mask)) {

			Vector3 v = hit.point;
			v.y = transform.position.y;
			transform.LookAt(v,Vector3.up);

			// Do something with the object that was hit by the raycast.
		}
        

        Vector3 camshaft = cam.transform.forward;
        camshaft.y = 0f;
        camshaft = camshaft.normalized;

        Vector3 move = 
            (
            camshaft * Input.GetAxis("Vertical")) 
            + 
            (cam.transform.right.normalized * Input.GetAxis("Horizontal")
            );

        rb.velocity = Vector3.ClampMagnitude(move, 1f) * speed;

        if(rb.velocity.magnitude > 0.1f)
        {
            anim.SetBool("Running", true);
        }
        else
        {
            anim.SetBool("Running", false);
        }


        rb.velocity = Vector3.ClampMagnitude(rb.velocity, speed);
		
		if (Input.GetMouseButtonDown(0))
		{
			
			//catch sequence
			handTrigger.SetActive(true);
			handTriggerStart = Time.time;

		}

		if (handTrigger.activeSelf && Time.time - handTriggerStart > handTriggerStay)
		{
			handTrigger.SetActive(false);
		}

		if (Input.GetMouseButtonUp(0))
		{
			carriedMeeple = null;
			anim.SetBool("CaugtAMeeple",false);
		}
		
	}


	public void MeepleEnteredTrigger(Meeple meeple)
	{
		if (carriedMeeple) return; 
		
		
		meeple.transform.position = meepleHolder.position;
		meeple.transform.rotation = meepleHolder.rotation;
		meeple.transform.SetParent(this.transform);
		meeple.toCarried();
		carriedMeeple = meeple;
		
		anim.SetBool("CaugtAMeeple",true);

	}


	
	
}
