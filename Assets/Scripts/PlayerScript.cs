using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    Rigidbody2D rigid;
    bool onGround;
    float moveSpeed;
    float jumpForce;

    string triggerObject;
    bool nearSomething;

    Text interactText;
    Slider healthSlider;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        onGround = true;
        moveSpeed = .1f;
        jumpForce = 10;

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
            transform.position = new Vector3(transform.position.x - moveSpeed, transform.position.y, transform.position.z);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.position = new Vector3(transform.position.x + moveSpeed, transform.position.y, transform.position.z);
        }

        if (Input.GetKeyDown(KeyCode.Space) && onGround) //cannot jump if not touching ground
        {
            rigid.velocity = Vector2.up * jumpForce;
            onGround = false;
        }

        if (Input.GetKeyDown(KeyCode.E) && triggerObject != "")
        {
            Interact(triggerObject); //near a door? Press E to enter. Near an NPC? Press E to talk
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
            Debug.Log("String is " + triggerObject);
            interactText.text = "Press 'E' to enter " + triggerObject;
            interactText.gameObject.SetActive(true);
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

    void Shoot()
    {

    }

    void TakeDamage()
    {

    }

    void Interact(string type) //near a door? Tell the player to press "E" to go through it
    {
        Debug.Log("String is " + type);
    }
}
