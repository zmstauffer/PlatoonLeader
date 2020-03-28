using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public SquadManager squadMan;
    //awake called before everything else
    private void Awake()
    {
        //create a squad and place it in the scene
        //squadMan = new SquadManager();
        //squadMan.squadCentroid = transform;          //put the squad where it is supposed to spawn, this should change to some sort of spawn point
        //squadMan.squadRotation = this.transform.rotation;

        //loadSquad(squadMan);

        //setup the events for the squad, although we aren't currently using these
        //FindObjectOfType<MouseManager>().OnMouseSelect += squadMan.OnMouseSelect;
        //FindObjectOfType<MouseManager>().OnMouseDeselect += squadMan.OnMouseDeselect;
        //FindObjectOfType<MouseManager>().OnMove += squadMan.OnMove;
    }

    //private void loadSquad(SquadManager squad)
    //{
    //    //create 1 soldier for now
    //    for (int i = 0; i < 5; i++)
    //    {
    //        if (squad.soldiers.Count <= 0)
    //        {
    //            GameObject newSoldier = Instantiate(Resources.Load("Soldier"), squad.squadCentroid.position, squad.squadRotation) as GameObject;
    //            newSoldier.GetComponent<Soldier>().squad = squad;
    //            squad.soldiers.Add(newSoldier);
    //        }
    //        else
    //        {
    //            GameObject newSoldier = Instantiate(Resources.Load("Soldier"), squad.soldiers[i - 1].transform.TransformPoint(Vector3.right * 2), squad.squadRotation) as GameObject;
    //            newSoldier.GetComponent<Soldier>().squad = squad;
    //            squad.soldiers.Add(newSoldier);
    //        }
    //    }
    //}
}
