using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpaceshipMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private float forwardThrust;
    private bool isThrusting = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isThrusting)
        {
            if (forwardThrust != 0)
            {
                transform.position += transform.forward * forwardThrust;
            }
        }

        isThrusting = !isThrusting;
    }

    public void OnForwardThrust(InputAction.CallbackContext context)
    {
        isThrusting = !isThrusting;
        forwardThrust = context.ReadValue<float>();
        Debug.Log(forwardThrust);
    }
}
