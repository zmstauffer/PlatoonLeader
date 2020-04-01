using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{

    public float delay = 3;
    private float timeRemaining;
    // Start is called before the first frame update
    void Start()
    {
        timeRemaining = delay;
    }

    // Update is called once per frame
    void Update()
    {
        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0)
        {
            explode();
            Object.Destroy(gameObject);
        }
    }

    private void explode()
    {
        //show effect

        //add forces to stuff
    }
}

