using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Inscribed")]
    public Transform[] waypoints;
    [Tooltip("The default pause time, which is overriden if a waypoint possesses a pause time not equal to 0")]
    public float defaultPauseTime = 0;
    [Tooltip("Whether or not the waypoints should restart from scratch, or loop backwards after a full cycle")]
    public bool reverseCycle = true;
    public float travelSpeed = 10;
    [Header("Dynamic")]
    [SerializeField] int waypointInt = -1;
    [SerializeField] float pauseRemaining = 0;
    [SerializeField] bool reversed = false;
    public bool contactFrozen = false;

    void Awake(){
        NewTarget(false);
    }

    // Called when a player dies and respawns
    public void ResetPosition()
    {
        transform.position = waypoints[0].transform.position;
        waypointInt = -1;
        reversed = false;
        pauseRemaining = 0;
        contactFrozen = false;
        NewTarget(false);
    }

    void FixedUpdate(){
        if (contactFrozen){
            return;
        }
        if (pauseRemaining > 0){
            pauseRemaining -= 0.02f;
        }

        else{
        transform.position = Vector3.MoveTowards(transform.position, waypoints[waypointInt].position, travelSpeed);

        if (transform.position == waypoints[waypointInt].position){
            NewTarget(true);
        }
        }
    }

    void NewTarget(bool usePause){
        if (usePause){
            WaypointAttributes wpInfo = waypoints[waypointInt].GetComponent<WaypointAttributes>();
            contactFrozen = wpInfo.contactPause;

            if (wpInfo.pauseDuration != 0){
                pauseRemaining = wpInfo.pauseDuration;
            }
            else{
            pauseRemaining = defaultPauseTime;
            }
        }

        if (reversed == false){
            waypointInt += 1;
            if (waypointInt == waypoints.Length){
                if (reverseCycle == false){
                waypointInt = 0;
                }
                else{
                    reversed = true;
                    waypointInt -= 1;
                }
            }
        }

        if (reversed == true){
            waypointInt -= 1;
            if (waypointInt == -1){
                reversed = false;
                waypointInt = 1;
            }
        }
               
            
            }
    }

