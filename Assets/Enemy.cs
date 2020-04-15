using System.Collections;
using System.Collections.Generic;
using UnityEngine.VFX;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health { get; set; }
    public VisualEffect deathFX;

    // Start is called before the first frame update
    void Start()
    {
        tag = "Enemy";
        health = 10;
    }

    // Update is called once per frame
    void Update()
    {
        
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
            VisualEffect deathEffect = Instantiate(deathFX, transform.position + Vector3.up*3, transform.rotation);
            deathEffect.Play();
            Destroy(deathEffect.gameObject, 3f);
            Destroy(this.gameObject);
        }
    }
}
