using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AllyAI : MonoBehaviour
{
    // Start is called before the first frame update
    private enum state
    {
        following,
        pursuing,
        seeking,
        attacking
    }

    private state currentState;

    private bool isFiring = false;
    [SerializeField]
    private GameObject laserBeam;
    [SerializeField]
    private GameObject beamer_L;
    [SerializeField]
    private GameObject beamer_R;


    [Range(1, 100)]
    [SerializeField]
    private int attackRange;

    [Range(1, 100)]
    [SerializeField]
    private int prediction;

    [Range(1, 100)]
    [SerializeField]
    private int maxAcceleration;

    [Range(1, 100)]
    [SerializeField] 
    private int maxTargetTime;

    private GameObject currentTarget;

    [SerializeField]
    private GameObject playerObject;

    [SerializeField]
    private GameObject[] targetEnemies;
    void Start()
    {
        currentState = state.following;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case state.attacking:
                if (currentTarget != null)
                {
                    //Get the coordinates of the target.
                    Vector3 enemyPosition = currentTarget.transform.position - transform.position;

                    //Rotate to the player
                    Quaternion currentRotation = Quaternion.LookRotation(enemyPosition, Vector3.up);
                    transform.rotation = currentRotation;

                    //Move towards the target position of the enemy.
                    float distanceToEnemy = enemyPosition.magnitude;

                    if (distanceToEnemy > 30)
                    {
                        Debug.Log("Heading for enemy!");
                        transform.position += transform.forward * maxAcceleration / 100;
                    }



                    if (!isFiring)
                    {
                        isFiring = true;
                        StartCoroutine(BurstFire(1));
                    }
                }

                else
                {
                    currentState = state.following;
                }
                Debug.Log(currentState);

                break;

            case state.following:
                //Get the coordinates of the target.
                Vector3 relativePos = playerObject.transform.position - transform.position;

                //Rotate to the player
                Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
                transform.rotation = rotation;

                // Calculate the distance to the player
                float distanceToPlayer = relativePos.magnitude;

                if(distanceToPlayer > 30)
                {
                    transform.position += transform.forward * maxAcceleration / 100;
                }

                

                //Move towards the target position of the player.
                if (Mathf.Abs(relativePos.x) > 10 && Mathf.Abs(relativePos.y) > 10 && Mathf.Abs(relativePos.z) > 10)
                {
                    Debug.Log("Moving");
                    
                }

                for (int i = 0; i < targetEnemies.Length; i++)
                {
                    if (targetEnemies[i] != null)
                    {
                        Vector3 enemyPosition = targetEnemies[i].transform.position - transform.position;
                        if (enemyPosition.x < attackRange && enemyPosition.y < attackRange && enemyPosition.z < attackRange)
                        {
                            currentState = state.attacking;
                            currentTarget = targetEnemies[i];
                            break;
                        }
                    }
                }

                break;

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
