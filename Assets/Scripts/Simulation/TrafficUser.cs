﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrafficUser : MonoBehaviour
{
    [SerializeField]
    public GameObject lane;

    public WayPoint currentWayPoint;

    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        speed = 7.0f;
        currentWayPoint = lane.GetComponent<Lane>().wayPoints.FirstOrDefault();
        transform.position = currentWayPoint.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateY();

        if (currentWayPoint)
        {
            MoveTowardsWayPoint();
        }
        if (PassedWayPoint())
        {
            SetWayPointToNext();
        }
        CheckEnd();
    }

    private void UpdateY()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 0.2f))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.01f, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.05f, transform.position.z);
        }
    }

    private void MoveTowardsWayPoint()
    {
        float step = speed * Time.deltaTime;

        var _direction = (currentWayPoint.transform.position - transform.position).normalized;

        //create the rotation we need to be in to look at the target
        var _lookRotation = Quaternion.LookRotation(_direction) * Quaternion.Euler(0, -90, 0);

        //rotate us over time according to speed until we are in the required rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, step);
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        if (HasToStop())
        {
            return;
        }

        transform.position = currentWayPoint ? Vector3.MoveTowards(transform.position, currentWayPoint.transform.position, step) : transform.position;
    }

    private bool HasToStop()
    {
        if (Vector3.Distance(transform.position, currentWayPoint.transform.position) < 2)
        {
            if (currentWayPoint.CheckLight == true && lane.GetComponent<Lane>().trafficLight.color == LightColor.Red)
            {
                return true;
            }
        }

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hit, 2.0f))
        {
            return true;
        }
        return false;
    }

    private bool PassedWayPoint()
    {
        if (Vector3.Distance(transform.position, currentWayPoint.transform.position) < 0.2)
        {
            return true;
        }
        return false;
    }

    private void SetWayPointToNext()
    {
        currentWayPoint = currentWayPoint.Next;
    }

    private void CheckEnd()
    {
        if (currentWayPoint.name == "End") // todo Make this work with tags?
        {
            Despawn();
        }
    }

    private void Despawn()
    {
        Destroy(gameObject);
        lane.GetComponent<Lane>().cars.Remove(gameObject);
    }
}
