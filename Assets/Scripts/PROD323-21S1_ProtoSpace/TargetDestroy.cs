using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetDestroy : MonoBehaviour
{
    public GameObject explosion;
    public Slider healthbar;
    public int health;

    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Laser")
        {
            health -= 1;
            healthbar.value = health;
            Destroy(other.gameObject);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if(health < 0)
        {
            Instantiate(explosion, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject, 0.1f);
        }
            
    }
}
