using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Sharkey, Logan
/// 5/7/2024
/// Handles the movement of the guards, and the detection of the player as well
/// </summary>
public class Enemy_Roamer: MonoBehaviour
{
    public Transform waypointHolder;
    public float speed = 4f;
    public float waitTime = 3f;
    public float turnSpeed = 90f;

    private void Awake()
    {
        //fills and array with all the patrol points created in the editor
        Vector3[] waypoints = new Vector3[waypointHolder.childCount];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = waypointHolder.GetChild(i).position;
            waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z);
        }
        StartCoroutine(Patrol(waypoints));
    }
  
    /// <summary>
    /// Using gizmos to help set up guard patrolling paths and adjust detection range
    /// </summary>
    private void OnDrawGizmos()
    {
        Vector3 startPosition = waypointHolder.GetChild(0).position;
        Vector3 previousPosition = startPosition;

        foreach (Transform waypoint in waypointHolder)
        {
            Gizmos.DrawSphere(waypoint.position, 0.3f);
            Gizmos.DrawLine(previousPosition, waypoint.position);
            previousPosition = waypoint.position;
        }
        Gizmos.DrawLine(previousPosition, startPosition);

        //Gizmos.color = Color.red;
        //Gizmos.DrawRay(transform.position, transform.forward * 10);
    }

    /// <summary>
    /// Looping Logic for the roamer to run to patrol points
    /// </summary>
    /// <param name="waypoints">Array of all the patrol points made in the editor</param>
    /// <returns></returns>
    IEnumerator Patrol(Vector3[] waypoints)
    {
        transform.position = waypoints[0];

        //we start at the first waypoint so we want our target to be the next in line
        int targetWaypointNum = 1;
        Vector3 targetWaypointLoc = waypoints[targetWaypointNum];

        transform.LookAt(targetWaypointLoc);

        while (true)
        {
            yield return StartCoroutine(Turn(targetWaypointLoc));
            transform.position = Vector3.MoveTowards(transform.position, targetWaypointLoc, speed * Time.deltaTime);

            //once waypoint is reached
            if (transform.position == targetWaypointLoc)
            {
                //once the waypoint array length is reached, targetWaypointNum will be set to 0 using the modulus operator
                targetWaypointNum = (targetWaypointNum + 1) % waypoints.Length;

                //set new target waypoint to move towards
                targetWaypointLoc = waypoints[targetWaypointNum];

                //wait before moving to next location, makes sense but is also necessary to avoid an infinite loop crash even though it technically is an infinite loop
                yield return new WaitForSeconds(waitTime);
                yield return StartCoroutine(Turn(targetWaypointLoc));
            }
            yield return null;
        }
    }

    /// <summary>
    /// Used to determine where the guard should look before moving to the next patrol point
    /// </summary>
    /// <param name="lookTarget">Next patrol point</param>
    /// <returns></returns>
    IEnumerator Turn(Vector3 lookTarget)
    {
        //gets the direction the guard should rotate towrds before moving to the next waypoint
        Vector3 dirToLook = (lookTarget - transform.position).normalized;
        float targetAngle = 90 - Mathf.Atan2(dirToLook.z, dirToLook.x) * Mathf.Rad2Deg;

        //if the guard is not turned towards the correct angle, turn towards next waypoint
        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f)
        {
            float angle;
            
            angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
        
            transform.eulerAngles = Vector3.up * angle;
            yield return null;
        }
    }

    
}
