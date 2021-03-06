﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Soldier : MonoBehaviour
{
    [SerializeField]
    private Material highlightMaterial;
    [SerializeField]
    private Material defaultMaterial;

    public NavMeshAgent agent;
    public SquadManager squad;
    public const bool isFriendly = true;

    //let's do some properties for our soldiers
    public string name { get; set; }
    public string rank { get; set; }

    public float reactionTime { get; set; }
    public float fireRate { get; set; }
    public float perception { get; set; }
    public float maxFireRange { get; set; }
    public float accuracy { get; set; }

    private const int bulletVelocity = 2500;            //this doesn't belong here!
    private const int bulletDamage = 5;
    private bool allowFire = false;
    private float timeToNextFire;
    private const float maxRange = 25;

    //private GameObject currentTarget = null;
    private GameObject lastTarget = null;               //who were we firing at before?

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<MouseManager>().OnMouseSelect += OnMouseSelect;
        FindObjectOfType<MouseManager>().OnMouseDeselect += OnMouseDeselect;
        FindObjectOfType<MouseManager>().OnMove += OnMove;
        timeToNextFire = Time.time + reactionTime;
        accuracy = Random.Range(.7f, .95f);
        fireRate = Random.Range(1.4f, 1.7f);
        reactionTime = Random.Range(.2f, .6f);
        maxFireRange = Random.Range(24f, 27f);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 3, Color.green);

        //check for targets
        if (squad.targets.Count > 0)
        {
            GameObject currentTarget = getClosestTarget();
            if (currentTarget != null)
            {
                if (allowFire)
                {
                    Vector3 fireDirection = currentTarget.transform.position +  - transform.position;       //currentTarget.GetComponent<Rigidbody>().velocity)/2 this needs to be redone (the rigidbody.velocity/2 is just a hack to get them to lead the target a bit)
                    fire(fireDirection);
                    allowFire = false;
                    timeToNextFire = Time.time + fireRate;
                }
                else
                {
                    if (lastTarget == currentTarget)
                    {
                        if (Time.time >= timeToNextFire)
                        {
                            allowFire = true;
                        }
                    }
                    else 
                    {
                        timeToNextFire = Time.time + reactionTime;
                        lastTarget = currentTarget;
                    }
                }
            }
        }
    }

    //gets closest target to fire at. This should be smartened up!
    public GameObject getClosestTarget()
    {
        GameObject currentTarget = null;
        foreach (GameObject potentialTarget in squad.targets)
        {
            if (potentialTarget != null)
            {
                float potentialRange = Vector3.Distance(potentialTarget.transform.position, transform.position);
                float currentRange = 9999.0f;
                if (currentTarget != null)
                {
                    currentRange = Vector3.Distance(currentTarget.transform.position, transform.position);
                }

                if (potentialRange < currentRange && potentialRange <= maxFireRange)
                {
                    currentTarget = potentialTarget;
                }
            }
        }
        return currentTarget;
    }

    public void fire(Vector3 direction)
    {
        //modify firing direction based on soldier's accuracy
        direction.x += calcAccuracyForShot()*10;
        direction.z += calcAccuracyForShot()*10;

        //rotate soldier towards target
        Quaternion newRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = newRotation;

        //instantiate bullet prefab slightly in front of soldier
        GameObject newBullet = Instantiate(Resources.Load("Bullet"), transform.position + direction.normalized, newRotation) as GameObject;

        //give bullet a force, adjust w/ accuracy
        newBullet.GetComponent<Rigidbody>().AddForce(newBullet.transform.forward * bulletVelocity);
        newBullet.GetComponent<Projectile>().damage = bulletDamage;

        //play a sound
        FindObjectOfType<AudioManager>().Play("shot");

        //bullet will self destruct after 3 secs
        Object.Destroy(newBullet, 3.0f);
    }

    private float calcAccuracyForShot()
    {
        int sign = utility.chooseRandomSign();
        float acc = Random.Range(accuracy, 1);

        return (1 - acc) * sign;
    }

    public void OnMouseSelect(Collider collider)
    {
        if (GetComponent<Collider>() == collider)
        {
            //change material
            GetComponent<Renderer>().material = highlightMaterial;

            //call squadmanager to select all the other soldiers
            squad.OnMouseSelect();
        }
    }

    public void OnMouseSelect()
    {
        GetComponent<Renderer>().material = highlightMaterial;
    }

    public void OnMouseDeselect(Collider collider)
    {
        if (GetComponent<Collider>() == collider)
        {
            GetComponent<Renderer>().material = defaultMaterial;
            squad.OnMouseDeselect();
        }
    }

    public void OnMouseDeselect()
    {
        GetComponent<Renderer>().material = defaultMaterial;
    }

    public void OnMove(Collider collider, Vector3 finalFacing, Vector3 destination)     //this is the function that is subscribed to the OnMove event and alerts the SquadManager that it needs to move the soldiers.
    {
        if (GetComponent<Collider>() == collider)
        {
            squad.OnMove(finalFacing, destination);
        }
    }

    public void OnMove(Vector3 destination)             //this move function is called by the SquadManager to move the agent
    {
        agent.SetDestination(destination);
    }

    public void OnDeath()
    {
        FindObjectOfType<MouseManager>().OnMouseSelect -= OnMouseSelect;
        FindObjectOfType<MouseManager>().OnMouseDeselect -= OnMouseDeselect;
        FindObjectOfType<MouseManager>().OnMove -= OnMove;
    }
}
