using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class SquadManager : MonoBehaviour
{
    public List<GameObject> soldiers = new List<GameObject>();         //the soldiers that make up this squad

    public const int soldierOffset = 2;         //how far apart the soldiers stand from each other

    public List<GameObject> targets = new List<GameObject>();          //list of targets

    private void Awake()
    {
        loadSquad();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //want to keep transform equal w/ soldier that represents squad leader
        Vector3 newPosition = Vector3.zero;

        newPosition.x = soldiers.Average(c => c.transform.position.x);
        newPosition.y = soldiers.Average(c => c.transform.position.y);
        newPosition.z = soldiers.Average(c => c.transform.position.z);

        transform.position = newPosition;
        transform.rotation = soldiers[0].transform.rotation;

        //update target list
        for (int i = targets.Count -1; i >= 0; i--)
        {
            if (targets[i] == null)
            {
                targets.RemoveAt(i);
            }
        }
    }

    #region Collider Handling
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Projectile")
        {
            targets.Add(other.gameObject);
        }

        Debug.Log(other.tag);
    }

    private void OnCollisionEnter(Collision collision)
    {
        targets.Add(collision.gameObject);
    }

    //When the Primitive exits the collision, it will change Color
    private void OnTriggerExit(Collider other)
    {
        targets.Remove(other.gameObject);
    }
    #endregion

    #region Mouse Stuff
    public void OnMouseSelect()
    {
        foreach (GameObject man in soldiers)
        {
            man.GetComponent<Soldier>().OnMouseSelect();
        }
    }

    public void OnMouseDeselect()
    {
        foreach (GameObject man in soldiers)
        {
            man.GetComponent<Soldier>().OnMouseDeselect();
        }
    }
    public void OnMove(Vector3 finalFacing, Vector3 destination)
    {
        Vector3 right;
        Vector3 back;

        //finalFacing will be zero if the user just right clicked on the terrain instead of drawing an arrow to set unit facing.
        if (finalFacing == Vector3.zero)
        {
            Vector3 currentDirection = destination - soldiers[0].transform.position;                //calculates direction between current squad position and desired position so that squad will line up perpendicular to line of travel
            right = Quaternion.Euler(0, 90f, 0) * currentDirection;
        }
        else
        {
            right = Quaternion.Euler(0, 90f, 0) * finalFacing;                                      //squad will line up in desired facing
        }

        back = Quaternion.Euler(0, 90f, 0) * right;

        //calculate new positions for squad and tell them to move. going for a 'V' formation w/ squad leader in center.
        soldiers[0].GetComponent<Soldier>().OnMove(destination);         //squad leader in front. Not accurate but good enough for now
        soldiers[1].GetComponent<Soldier>().OnMove(destination + (right.normalized + back.normalized) * soldierOffset);
        soldiers[2].GetComponent<Soldier>().OnMove(destination + (right.normalized + back.normalized) * soldierOffset * 2);
        soldiers[3].GetComponent<Soldier>().OnMove(destination + (-right.normalized + back.normalized) * soldierOffset);
        soldiers[4].GetComponent<Soldier>().OnMove(destination + (-right.normalized + back.normalized) * soldierOffset * 2);
    }

    #endregion

    #region Squad Load
    private void loadSquad()
    {
        //create 1 soldier for now. eventually will have read/write and load for this
        for (int i = 0; i < 5; i++)
        {
            GameObject newSoldier;
            if (soldiers.Count <= 0)
            {
                newSoldier = Instantiate(Resources.Load("Soldier"), transform.position, transform.rotation) as GameObject;
            }
            else
            {
                newSoldier = Instantiate(Resources.Load("Soldier"), soldiers[i - 1].transform.TransformPoint(Vector3.right * 2), transform.rotation) as GameObject;
            }

            //set soldier properties
            Soldier soldierScript = newSoldier.GetComponent<Soldier>();
            soldierScript.squad = this;
            soldiers.Add(newSoldier);
        }
    }

    #endregion

    public void OnDeath()
    {

    }
}
