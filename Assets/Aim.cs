using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour
{
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private float range = 100;

    [SerializeField]
    public GameObject laserBeam;
    [SerializeField]
    public GameObject beamer_R;
    [SerializeField] public GameObject beamer_L;

    public bool inRange = false;
    private bool isFiring = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Rotate the cannon
        rotate();

        //If the target is in range.
        if (inRange && !isFiring)
        {
            isFiring = true;
            StartCoroutine(BurstFire(5));
        }
    }


    private void rotate()
    {
        //Get the coordinates of the target.
        Vector3 relativePos = target.transform.position - transform.position;

        if (relativePos.x < range && relativePos.y < range && relativePos.z < range)
        {
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            transform.rotation = rotation;
            inRange = true;
        }
        else
        {
            inRange = false;
        }
    }

    private IEnumerator BurstFire(int burstAmount)
    {
        for (var i = 0; i < burstAmount; ++i)
        {
            Fire();
            yield return new WaitForSeconds(0.5f);
        }
        isFiring = false;
    }

    private void Fire()
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
