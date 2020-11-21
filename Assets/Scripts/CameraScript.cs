using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {
        if (player.transform.position.x > transform.position.x + 1) //follow the player when player is near the edge of the screen
        {
            transform.position = new Vector3(player.transform.position.x - 1, 0, -10);
        }
        else if (player.transform.position.x < transform.position.x - 1)
        {
            transform.position = new Vector3(player.transform.position.x + 1, 0, -10);
        }
    }
}
