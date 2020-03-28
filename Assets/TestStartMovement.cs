using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStartMovement : MonoBehaviour
{
    public int health;
    public Rigidbody thisBody;

    // Start is called before the first frame update
    void Start()
    {
        thisBody.AddForce(Vector3.back * 200);
        health = 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            health -= (int)other.gameObject.GetComponent<Projectile>().damage;
        }
        //Debug.Log("Health: " + health);
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
