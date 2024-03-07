using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmartLaserAvoidance : MonoBehaviour
{
    public bool isSmart = true;
    public GameObject explosion;
    public Slider healthbar;
    public int health;

    private Rigidbody shipRB;
    private float avoidingSpeed = 2f;
    private Vector3 contactPos;
    private Quaternion contactRot;
    private void Start()
    {   
        shipRB = gameObject.GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(isSmart) 
        {
            if(other.tag == "Laser")
            {
                Vector3 laserPath = other.gameObject.transform.position - gameObject.transform.position;
                Vector3 avoidingDirection = Vector3.Cross(laserPath, gameObject.transform.up);
                shipRB.AddForce(avoidingDirection* avoidingSpeed, ForceMode.Impulse);
            }
        }

    }

    private void OnTriggerExit(Collider other) 
    {
        if(isSmart) 
        {        
            if(other.tag == "Laser")
            {
                shipRB.velocity = Vector3.zero;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Laser")
        {
            health -= 1;
            healthbar.value = health;
            ContactPoint contact = collision.contacts[0];
            contactRot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            contactPos = contact.point;
            Destroy(collision.gameObject);
        }
    }

    void Update()
    {
        if(health < 0)
        {
            Instantiate(explosion, contactPos, contactRot);
            Destroy(gameObject, 0.1f);
        }
            
    }
}
