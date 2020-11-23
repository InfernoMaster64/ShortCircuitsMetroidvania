﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    Rigidbody2D rigid;
    float moveSpeed;

    bool onGround, isJumping;
    float jumpForce, jumpCounter, jumpTime; //power of jumping, current jump time, max jump time

    string triggerObject;
    bool nearSomething;

    Text interactText;
    Slider healthSlider;
    int currentHealth;

    StatsScript stats;

    void Start()
    {
        stats = GameObject.Find("playerStats").GetComponent<StatsScript>();

        interactText = GameObject.Find("interactText").GetComponent<Text>();
        healthSlider = GameObject.Find("healthSlider").GetComponent<Slider>();

        UpdateStats(); //immediately set starting stats, or keep them updated for scene changes

        rigid = GetComponent<Rigidbody2D>();
        moveSpeed = .1f;

        onGround = true;
        isJumping = false;
        jumpForce = 10;
        jumpTime = .35f;

        nearSomething = false;

        interactText.gameObject.SetActive(false);

        healthSlider.maxValue = stats.Health;

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
            jumpCounter = jumpTime;
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

        if (Input.GetKey(KeyCode.B))
        {
            Shoot();
        }

        if (Input.GetKey(KeyCode.D))
        {
            Debug.Log("Become dead lol");
            SceneManager.LoadScene("Castle");
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
        if (stats.Ammo > 0)
        {
            stats.Ammo -= 1;
            Debug.Log("Pew pew pew! Ammo remaining: " + stats.Ammo);
        }
        else
        {
            Debug.Log("Out of ammo!");
        }
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

    void UpdateStats()
    {
        if (stats.Level == 0) //if just starting the game
        {
            DontDestroyOnLoad(stats.gameObject);
            stats.Level = 1;
            stats.Health = 100;
            stats.Damage = 10;
            stats.Defense = 10;

            stats.Ammo = 20;
            stats.expLvRequired = 200;


        }
        else
        {
            healthSlider.maxValue = stats.Health;
        }

    }
}
