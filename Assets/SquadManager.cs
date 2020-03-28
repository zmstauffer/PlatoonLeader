﻿using System;
using System.Collections;
using System.Collections.Generic;
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
        transform.position = soldiers[0].transform.position;
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

        //calculate new positions for squad and tell them to move
        for (int i = 0; i < soldiers.Count; i++)
        {
            Vector3 stepOver = right.normalized * soldierOffset * i;
            soldiers[i].GetComponent<Soldier>().OnMove(destination + stepOver);
        }

    }

    #endregion

    #region Squad Load
    private void loadSquad()
    {
        //create 1 soldier for now
        for (int i = 0; i < 5; i++)
        {
            if (soldiers.Count <= 0)
            {
                GameObject newSoldier = Instantiate(Resources.Load("Soldier"), transform.position, transform.rotation) as GameObject;
                newSoldier.GetComponent<Soldier>().squad = this;
                soldiers.Add(newSoldier);
            }
            else
            {
                GameObject newSoldier = Instantiate(Resources.Load("Soldier"), soldiers[i - 1].transform.TransformPoint(Vector3.right * 2), transform.rotation) as GameObject;
                newSoldier.GetComponent<Soldier>().squad = this;
                soldiers.Add(newSoldier);
            }
        }
    }

    #endregion

    public void OnDeath()
    {

    }
}