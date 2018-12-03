using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using Boo.Lang.Environments;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;

public class Meeple : MonoBehaviour {
    
    //Run away from boss
    //hop as you walk
    //when cought stay at top of boss and flail
    //when thrown contunie flailing
    //if boss too near run
    //if boss away, walk

    //commented more
    
    
    public Animator anim;
    public Rigidbody rb;
    
    public static float meepleThrowForwardForce = 10f;
    public static float meepleThrowUpwardsForce = 10f;
    
    public static float RandomInRadius = 20f;
    public static float RandomOutRadius = 99f;
    
    
    
    public bool cought;
    public bool thrown;

    public bool running;
    public float stamina = 100;

    public NavMeshAgent agent;
    public float nearTreshold = 1f;
    
    public Boss boss;

    public float runTreshold = 10f;
    
    public enum State
    {
        runningAway,
        walkingRandomly,
        beingCarried,
        thrown,
        thrashed
    }

    public State state;

    public float thrownStart;
    public float recoverCooldown = 2f;

    public Collider thrownCollider;
    public Collider normalCollider;

    public float thrashedStart;
    public float thrashCooldown = 2f;

    void Awake()
    {
        boss = Boss.GetBoss();
    }
    
    private void Update()
    {

        switch (state)
        {
            case State.walkingRandomly:
                
                Debug.DrawLine(agent.destination, agent.destination + Vector3.up);
                
                if (Vector3.Distance(transform.position, agent.destination) < nearTreshold)
                    pickANewDestination();

                if (Vector3.Distance(boss.transform.position, transform.position) < runTreshold)
                {
                    toRun();
                }
                    
                
                break;
            case State.runningAway:

                agent.destination = transform.position + (transform.position - boss.transform.position) ;
                
                Debug.DrawLine(agent.destination, agent.destination + Vector3.up);

                if ((boss.transform.position - transform.position).magnitude > runTreshold)
                {
                    toWalk();
                }
                
                break;
            case State.beingCarried:
                 
                //play being carried
                if (Input.GetMouseButtonUp(0))
                {
                    toThown();
                }
                
                break;
            case State.thrown:
                 
                //play thrown
                if (Time.time - thrownStart > recoverCooldown)
                {
                    if (Mathf.Abs(rb.velocity.y) < .1f)
                    {
                        reset();
                    }
                }
                
                break;
            
            case State.thrashed:
                if (Time.time - thrashedStart > thrashCooldown)
                {
                    gameObject.SetActive(false);
                }
                
                break;
            
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == boss.handTrigger)
        {
            boss.MeepleEnteredTrigger(this);
        }
        else
        {
            //thrown to abbys
            toThrashed();
        }
        
    }

    void pickANewDestination()
    {
        //agent.destination = Random.onUnitSphere * Random.Range(RandomInRadius, RandomOutRadius);
        
        //Vector3 randomDirection = Random.insideUnitSphere * 7f;
        //randomDirection += transform.position;
        //NavMeshHit hit;
        //Vector3 finalPosition = Vector3.zero;
        //if (NavMesh.SamplePosition(randomDirection, out hit, 7f, 1)) {
        //    finalPosition = hit.position;            
        //}

        //agent.destination = finalPosition;

        agent.destination = MeepleManager.getInstance().getRandomWaypoint();
    }
    

    public void toWalk()
    {
        state = State.walkingRandomly;
        anim.Play("Walking");
        agent.speed = 3.5f;
        thrownCollider.enabled = false;
        normalCollider.enabled = true;
        pickANewDestination();
    }

    public void toRun()
    {
        state = State.runningAway;
        anim.Play("Running");
        agent.speed = 7f;
        thrownCollider.enabled = false;
        normalCollider.enabled = true;
    }

    public void toCarried()
    {
        state = State.beingCarried;
        agent.enabled = false;
        rb.isKinematic = true;
        anim.Play("Caught");
        thrownCollider.enabled = false;
        normalCollider.enabled = false;
        
    }
    
    public void toThown()
    {   
        transform.SetParent(null);
        rb.isKinematic = false;
        rb.AddForce(Vector3.up * meepleThrowUpwardsForce, ForceMode.Impulse);
        rb.AddForce(boss.transform.forward * meepleThrowForwardForce, ForceMode.Impulse);
        thrownStart = Time.time;
        state = State.thrown;
        thrownCollider.enabled = true;
        normalCollider.enabled = false;
    }

    public void toThrashed()
    {
        thrashedStart = Time.time;
        state = State.thrashed;
        MeepleManager.getInstance().showParticle(transform.position);

    }
    
    public void reset()
    {
        state = State.runningAway;
        anim.Play("Running");
        agent.speed = 7f;
        transform.localEulerAngles = Vector3.zero;
        Vector3 pos = transform.position;
        pos.y = 0;
        transform.position = pos;                    
        agent.enabled = true;
        agent.SetDestination(this.transform.position);
        transform.position = agent.destination;
        
        rb.isKinematic = true;
        
    }
    
}
