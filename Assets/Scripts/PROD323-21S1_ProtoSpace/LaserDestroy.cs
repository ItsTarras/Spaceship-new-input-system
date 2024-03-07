using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    private float speed = 100f;

    void Start()
    {
        Destroy(gameObject, 3f);
    }

    private void Update()
    {
        transform.position = transform.position + transform.up * speed * Time.deltaTime;
    }
}
