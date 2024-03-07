using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Range(20f, 200f)]
    public float attackDist = 100f;
    [Range(0.1f, 2f)]
    public float fireInterval = 0.5f;
    public GameObject laserBeam;
    public GameObject beamer_R;
    public GameObject beamer_L;

    private GameObject player;
    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(timer > fireInterval) 
        {
            Attack();
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    void Attack()
    {
        Vector3 direction = player.transform.position - transform.position;

        float distance = direction.magnitude;
        if(distance < attackDist) {
            Fire(direction.normalized);
            return;
        }
    }

    void Fire(Vector3 v)
    {
        var transform = this.transform;
        var laserR = Instantiate(laserBeam, beamer_R.transform.position, Quaternion.identity);
        Quaternion rot = Quaternion.identity;
        rot.eulerAngles = new Vector3(90, 0, 0);
        laserR.transform.rotation = transform.rotation * rot;
        var laserL = Instantiate(laserBeam, beamer_L.transform.position, Quaternion.identity);
        laserL.transform.rotation = transform.rotation * rot;
    }
}
