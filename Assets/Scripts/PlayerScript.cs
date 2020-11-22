using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    Rigidbody2D rigid;
    float moveSpeed;

    bool onGround;
    bool isJumping;
    float jumpForce;
    float jumpCounter; //count air time
    float jumpTimer; //max air time

    string triggerObject;
    bool nearSomething;





    Text interactText;
    Slider healthSlider;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        moveSpeed = .05f;

        onGround = true;
        isJumping = false;
        jumpForce = 10;

        jumpTimer = .35f;

        nearSomething = false;

        interactText = GameObject.Find("interactText").GetComponent<Text>();
        interactText.gameObject.SetActive(false);

        healthSlider = GameObject.Find("healthSlider").GetComponent<Slider>();
        healthSlider.value = 100; //to be updated later
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            Move(-moveSpeed);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            Move(moveSpeed);
        }

        if (Input.GetKeyDown(KeyCode.Space) && onGround) //cannot jump if not touching ground
        {
            rigid.velocity = Vector2.up * (jumpForce / 2);
            onGround = false;
            isJumping = true;
            jumpCounter = jumpTimer;
        }
        if (Input.GetKey(KeyCode.Space) && isJumping) //tap space to jump, hold space to jump higher
        {
            if (jumpCounter > 0)
            {
                rigid.velocity = Vector2.up * jumpForce;
                jumpCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.Space) && isJumping) //if space is released before jumpCounter reaches 0,
        {
            isJumping = false;
        }

        if (Input.GetKeyDown(KeyCode.E) && nearSomething)
        {
            Interact(triggerObject); //near a door? Press E to enter. Near an NPC? Press E to talk
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            Shoot();
        }
    }

    private void OnCollisionEnter2D(Collision2D col) //OnCollision deals with physics, OnTrigger deals with "isTrigger"
    {
        if (col.gameObject.name.Contains("ground"))
        {
            onGround = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) //make sure TriggerEnter2D, Collider2D other, and objectCollider2D are all the same type
    {
        if (other.gameObject.tag == "Damage") //This is purely a bool for weapon damage. TakeDamage() checks the type and modifies health 
        {
            TakeDamage();
        }
        else
        {
            triggerObject = other.gameObject.tag;
            nearSomething = true;
            
            interactText.text = "Press 'E' to enter " + triggerObject;
            interactText.gameObject.SetActive(true);

            Debug.Log("Interacting with: " + triggerObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag != "Damage") 
        {
            triggerObject = "";
            nearSomething = false;
            interactText.gameObject.SetActive(false);
        }

    }

    void Move(float direction)
    {
        transform.position = new Vector3(transform.position.x + direction, transform.position.y, transform.position.z);
    }

    void Shoot()
    {

    }

    void TakeDamage()
    {

    }

    void Interact(string type) //near a door? Tell the player to press "E" to go through it
    {
        switch (type)
        {
            case "Castle":
                Debug.Log("You entered the creepy " + type);
                break;
            case "Crypt":
                Debug.Log("You entered the haunted " + type);
                break;
            case "Swamp":
                Debug.Log("You entered the spooky " + type);
                break;
            case "Town":
                Debug.Log("You entered the boring " + type);
                break;
            case "NPC": //temporary term
                Debug.Log("Dab");
                break;
        }
    }
}
