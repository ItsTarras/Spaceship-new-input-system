using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;

public class SpaceshipControl : MonoBehaviour
{
    public float moveSpeed;
    public float rotateSpeed;
    public float burstSpeed;
    public int maxQueue;
    public GameObject laserBeam;
    public GameObject beamer_R;
    public GameObject beamer_L;
    public GameObject cameraPlacement;
    public GameObject furthestPlacement;
    public ParticleSystem booster;
    public GameObject explosion;
    public Slider healthbar;
    public int health;
    public GameObject gameOverText;

    private SimpleControls m_Controls;
    private bool m_Charging;
    private Vector2 m_Rotation;
    private Rigidbody shipRB;
    private Queue<Vector3> posFilter = new Queue<Vector3>();
    private Queue<Quaternion> rotFilter = new Queue<Quaternion>();
    private float shipSpeed;
    private Vector3 v1, v2, v3;
    private Vector3 contactPos;
    private Quaternion contactRot;
    private bool gameover = false;
    private float forwardThrust;
    private bool isThrusting = false;
    private bool isSide = false;
    private float sideThrust;
    private float deltaX;
    private float deltaY;

    private void Start()
    {
        shipRB = gameObject.GetComponent<Rigidbody>();
        booster.Stop();
    }

      public void OnGUI()
    {
        if (m_Charging)
            GUI.Label(new Rect(100, 100, 200, 100), "Charging...");
    }

    public void Update()
    {
        // If game is not over, continue to read input
        // You can add code here
        if(!gameover)
        {
            CheckHealth();
        }

        //Forward Movement;
        if (isThrusting)
        {
            if (forwardThrust != 0)
            {
                ForwardThrust();
            }
        }
        isThrusting = !isThrusting;

        //Side movement;
        if (isSide)
        {
            SideThrust();
        }

        if (forwardThrust < 1 && sideThrust == 0)
        {
            Stop();
        }


        isSide = false;
        isThrusting = false;

        


        LookAround();
    }

    // Ship has to come to a stop if there is no more input present
    private void Stop(){
        if(shipRB.velocity.magnitude > 0) {
            shipRB.velocity -= shipRB.velocity * Time.deltaTime;
        }
        booster.Stop();
        return;
    }

    public void OnForwardThrust(InputAction.CallbackContext context)
    {
        isThrusting = true;
        forwardThrust = context.ReadValue<float>();
    }



    // Ship moves forward towards where it's looking. This will activate that booster.
    private void ForwardThrust(){
        var scaledMoveSpeed = moveSpeed * Time.deltaTime;

        shipRB.AddForce(transform.forward * forwardThrust * scaledMoveSpeed, ForceMode.Acceleration);
        shipSpeed = shipRB.velocity.magnitude;
        shipSpeed = shipSpeed < moveSpeed ? shipSpeed : moveSpeed;
        Vector3 v1 = cameraPlacement.transform.position;
        Vector3 v2 = furthestPlacement.transform.position;
        float lerpVal = shipSpeed / moveSpeed;
        Camera.main.transform.position = new Vector3(Mathf.Lerp(v1.x, v2.x, lerpVal), Mathf.Lerp(v1.y, v2.y, lerpVal), Mathf.Lerp(v1.z, v2.z, lerpVal));

        booster.Play();
    }


    public void OnSideThrust(InputAction.CallbackContext context)
    {
        isSide = true;
        sideThrust = context.ReadValue<float>();
        Debug.Log(sideThrust);
    }


    // Ship moves sideways. No rotation needed.
    private void SideThrust(){
        var scaledMoveSpeed = moveSpeed * Time.deltaTime;

        shipRB.AddForce(transform.right * sideThrust * scaledMoveSpeed, ForceMode.Acceleration);
        booster.Play();
    }


    public void OnLookAround(InputAction.CallbackContext context)
    {
        deltaX = context.action.ReadValue<Vector2>().x;
        deltaY = context.action.ReadValue<Vector2>().y;
    }



    // Look around with your mouse. You will need to find the proper key binding for this.
    private void LookAround()
    {
        var scaledRotateSpeed = rotateSpeed * Time.deltaTime;

        m_Rotation.y += deltaX * scaledRotateSpeed;
        m_Rotation.x = Mathf.Clamp(m_Rotation.x - deltaY * scaledRotateSpeed, -89, 89);
        transform.localEulerAngles = m_Rotation / 2;
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        Fire();
    }


    private IEnumerator BurstFire(int burstAmount)
    {
        for (var i = 0; i < burstAmount; ++i)
        {
            Fire();
            yield return new WaitForSeconds(0.1f);
        }
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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "LaserEnemy")
        {
            health -= 1;
            healthbar.value = health;
            ContactPoint contact = collision.contacts[0];
            contactRot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            contactPos = contact.point;
            Destroy(collision.gameObject);
        }
    }

    private void CheckHealth()
    {
        if(health < 0)
        {
            booster.Stop();
            Instantiate(explosion, contactPos, contactRot);
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            healthbar.gameObject.SetActive(false);
            gameOverText.SetActive(true);
            StartCoroutine(StopGame(2f));
            gameover = true;
        }
    }


    IEnumerator StopGame(float time)
    {
        yield return new WaitForSeconds(time);
        Time.timeScale = 0;
    }
}
