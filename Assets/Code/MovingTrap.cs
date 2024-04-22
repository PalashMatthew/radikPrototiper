using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTrap : MonoBehaviour
{
    public GameObject trapObj;
    public Transform target1, target2;

    public float speed;

    private void Start()
    {
        trapObj.transform.DOMove(target1.position, speed);
    }

    private void Update()
    {
        if (trapObj.transform.position == target1.position)
        {
            trapObj.transform.DOMove(target2.position, speed);
        }

        if (trapObj.transform.position == target2.position)
        {
            trapObj.transform.DOMove(target1.position, speed);
        }
    }
}
