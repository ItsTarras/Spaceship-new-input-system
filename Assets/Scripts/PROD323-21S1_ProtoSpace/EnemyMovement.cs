using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
public bool isPursuing = false;
    //public GameObject target; 

    [Range(20f, 200f)]
    public float maxDist = 100f;

    [Range(0.1f, 4f)]
    public float maxPrediction = 2f;

    [Range(0f, 100f)]
    public float maxAcceleration = 0.2f;

    [Range(1f, 20f)]    
    public float timetoTarget = 10f;

    private GameObject player;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        GetSteeringPursueOrEvade();
    }


    void GetSteeringPursueOrEvade()
    {
        Vector3 direction;
        float prediction;
        //1. Calculate the target to delegate to seek & Work out the distance to target
        direction = player.transform.position - transform.position;

        float distance = direction.magnitude;

        if(isPursuing)
        {
            if(distance < maxDist) {
                rb.velocity = Vector3.zero;
                LookWhereGoing();
                return;
            }  
        }
        else
        {
            if(distance > maxDist) {
                rb.velocity = Vector3.zero;
                return;
            }  
        }
     
        //Work out our current speed
        float speed = rb.velocity.magnitude;

        //Check if speed gives a reasonable prediction time
        if (speed <= distance / maxPrediction)
            prediction = maxPrediction;
        else //Otherwise calculate the prediction time
            prediction = distance / speed;

        //Put the target together
        Vector3 targetPos = player.transform.position;
        targetPos += player.GetComponent<Rigidbody>().velocity * prediction;

        //2. Delegate to seek
        SeekOrFlee(targetPos, isPursuing);
            
        //3. Face
        if(isPursuing)
        {
            LookWhereGoing();
        }
        else
        {
            Face(targetPos);
        }
  
    }

    void SeekOrFlee(Vector3 targetPos, bool pursue)
    {
        Vector3 v;
        if (pursue)
            v = targetPos - transform.position; /*Seek*/
        else
            v = transform.position - targetPos; /*Flee*/     

        rb.AddForce(v.normalized * maxAcceleration, ForceMode.Acceleration);
    }

    void Face(Vector3 targetPos)
    {
        // Delegate to align.
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(transform.position - targetPos, Vector3.up), Time.deltaTime/timetoTarget);
             
    }

    void LookWhereGoing()
    {
        if(rb.velocity.magnitude > 0)
            transform.rotation = Quaternion.Slerp(Quaternion.LookRotation(rb.velocity.normalized, Vector3.up), transform.rotation, Time.deltaTime/timetoTarget);
        else
            transform.rotation = Quaternion.Slerp(Quaternion.LookRotation(player.transform.position - transform.position, Vector3.up), transform.rotation, Time.deltaTime/timetoTarget);
    }
}
