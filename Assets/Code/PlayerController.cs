using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int moveDir;

    public float speed;

    public LayerMask layer;

    private void Update()
    {
        if (moveDir == 1)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }

        if (moveDir == 2)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }

        if (moveDir == 3)
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        }

        if (moveDir == 4)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        //RaycastHit hit;

        //if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layer))
        //{
        //    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
        //    Debug.Log("Did Hit");
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "down")
        {
            moveDir = 3;
        }

        if (other.tag == "up")
        {
            moveDir = 1;
        }

        if (other.tag == "right")
        {
            moveDir = 2;
        }

        if (other.tag == "left")
        {
            moveDir = 4;
        }
    }
}
