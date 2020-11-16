using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    Rigidbody2D rigid;
    bool onGround;
    float moveSpeed;
    float jumpForce;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        onGround = true;

        moveSpeed = .1f;
        jumpForce = 10;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position = new Vector3(transform.position.x - moveSpeed, transform.position.y, transform.position.z);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.position = new Vector3(transform.position.x + moveSpeed, transform.position.y, transform.position.z);
        }

        if (Input.GetKeyDown(KeyCode.Space) && onGround)
        {
            rigid.velocity = Vector2.up * jumpForce;
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
