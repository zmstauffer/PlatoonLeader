﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public bool isFriendly;
    public float damage;

    // Start is called before the first frame update
    void Start()
    {
        tag = "Projectile";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Object.Destroy(this.gameObject);
    }
}
