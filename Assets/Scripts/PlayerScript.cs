using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    Rigidbody2D rigid;
    bool onGround;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        onGround = true;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position = new Vector3(transform.position.x - .1f, transform.position.y, transform.position.z);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.position = new Vector3(transform.position.x + .1f, transform.position.y, transform.position.z);
        }

        if (Input.GetKeyDown(KeyCode.Space) && onGround)
        {
            rigid.velocity = Vector2.up * 6;
            onGround = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name.Contains("ground"))
        {
            onGround = true;
        }
    }
}
