using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    GameObject player;

    public bool lockCamera;
    public Vector3 lockPosition;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {
        if (!lockCamera)
        {
            if (player.transform.position.x > transform.position.x + 1) //follow the player when player is near the edge of the screen
            {
                transform.position = new Vector3(player.transform.position.x - 1, transform.position.y, -10);
            }
            else if (player.transform.position.x < transform.position.x - 1)
            {
                transform.position = new Vector3(player.transform.position.x + 1, transform.position.y, -10);
            }

            if (player.transform.position.y > transform.position.y) //follow, but vertically
            {
                transform.position = new Vector3(transform.position.x, player.transform.position.y + .25f, -10);
            }
            else if (player.transform.position.y < transform.position.y - 3)
            {
                transform.position = new Vector3(transform.position.x, player.transform.position.y + 2.98f, -10);
            }
        }
        else //used for second floor puzzle room
        {
            transform.position = lockPosition;
        }
    }
}
