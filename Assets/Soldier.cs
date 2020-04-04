using System.Collections;
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

    private const int bulletVelocity = 2000;            //this doesn't belong here!
    private const int bulletDamage = 5;
    private bool allowFire = true;
    private float timeToNextFire;
    private const float maxRange = 25;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<MouseManager>().OnMouseSelect += OnMouseSelect;
        FindObjectOfType<MouseManager>().OnMouseDeselect += OnMouseDeselect;
        FindObjectOfType<MouseManager>().OnMove += OnMove;
        timeToNextFire = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 3, Color.green);

        //check for targets
        if (squad.targets.Count > 0)
        {
            GameObject currentTarget = squad.targets[0];
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

                    if (potentialRange < currentRange)
                    {
                        currentTarget = potentialTarget; 
                    }
                }
                else
                {
                    currentTarget = potentialTarget;
                }
            }

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
                    if (Time.time >= timeToNextFire)
                    {
                        allowFire = true;
                    }
                }
            }
        }
    }

    public void fire(Vector3 direction)
    {
        //rotate soldier towards target
        Quaternion newRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = newRotation;

        //instantiate bullet prefab slightly in front of soldier
        GameObject newBullet = Instantiate(Resources.Load("Bullet"), transform.position + direction.normalized, newRotation) as GameObject;

        //give bullet a force
        newBullet.GetComponent<Rigidbody>().AddForce(newBullet.transform.forward * bulletVelocity);
        newBullet.GetComponent<Projectile>().damage = bulletDamage;

        //play a sound
        FindObjectOfType<AudioManager>().Play("shot");

        //bullet will self destruct after 3 secs
        Object.Destroy(newBullet, 3.0f);
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
