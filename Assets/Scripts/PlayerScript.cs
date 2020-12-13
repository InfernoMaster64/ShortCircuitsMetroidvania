using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    Rigidbody2D rigid;
    float speed;

    bool onGround, isJumping;
    float jumpForce, jumpCounter, jumpTime; //power of jumping, current jump time, max jump time
    bool canAttack = true;

    string triggerObject;
    bool nearSomething;

    Text interactText;
    Slider healthSlider;

    StatsScript stats;
    CameraScript camera;
    
    Animator anim;
    SpriteRenderer rend;
    AudioSource source;

    public GameObject[] mirrors;
    public GameObject[] mirrorNewPositions;
    int mirrorNum;

    Vector2 respawnPos;

    public AudioClip[] sfx;
    bool footstepsActive;

    public GameObject PuzzleController; //This will allow me to call to the Puzzle Code for literlly one line of this script.

    public GameObject bulletPrefab;

    void Start()
    {
        stats = GameObject.Find("playerStats").GetComponent<StatsScript>();
        camera = GameObject.Find("Main Camera").GetComponent<CameraScript>();

        interactText = GameObject.Find("interactText").GetComponent<Text>();
        healthSlider = GameObject.Find("healthSlider").GetComponent<Slider>();

        UpdateStats(); //immediately set starting stats, or keep them updated for scene changes

        rigid = GetComponent<Rigidbody2D>();

        onGround = true;
        isJumping = false;
        jumpForce = 10;
        jumpTime = .35f;

        nearSomething = false;

        interactText.gameObject.SetActive(false);

        healthSlider.maxValue = stats.Health;

        speed = stats.MoveSpeed;

        anim = gameObject.GetComponent<Animator>();
        rend = gameObject.GetComponent<SpriteRenderer>();
        source = gameObject.GetComponent<AudioSource>();

        transform.position = stats.respawn;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) //moving
        {
            //transform.localScale = new Vector2(-3, 3);
            rend.flipX = true;
            Move(-speed);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            //transform.localScale = new Vector2(3, 3);
            rend.flipX = false;
            Move(speed);
        }
        else if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.LeftArrow) //stop moving
            && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.RightArrow))
        {
            anim.SetBool("Movement", false);

            if (footstepsActive)
            {
                footstepsActive = false;
                CancelInvoke(); //stop footsteps when player stops moving
                source.Stop();
            }
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) //crouching
        {
            anim.SetBool("Crouching", true);
        }
        else if (!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.DownArrow)) //stop crouching
        {
            anim.SetBool("Crouching", false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && onGround) //cannot jump if not touching ground
        {
            rigid.velocity = Vector2.up * (jumpForce / 2);
            onGround = false;
            isJumping = true;
            anim.SetBool("Grounded", false);
            anim.SetBool("Jumping", true);
            jumpCounter = jumpTime;
            
            source.clip = sfx[2]; //The footstep sound is connected to jumping so they can only be heard when onGround = true
            source.Play();
            footstepsActive = true;
            CancelInvoke();
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
                anim.SetBool("Jumping", false);
                anim.SetBool("Falling", true);
                isJumping = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.Space) && isJumping) //if space is released before jumpCounter reaches 0
        {
            anim.SetBool("Jumping", false);
            anim.SetBool("Falling", true);
            isJumping = false;
        }

        if (Input.GetKeyDown(KeyCode.E) && nearSomething)
        {
            Interact(triggerObject); //near a door? Press E to enter. Near an NPC? Press E to talk
        }

        if (Input.GetKeyDown(KeyCode.Q) && canAttack) //shooting
        {
            anim.SetBool("Shooting", true);
            Shoot();
        }
        else if (!Input.GetKeyDown(KeyCode.Q)) //not shooting
        {
            anim.SetBool("Shooting", false);
        }

        if (Input.GetKeyDown(KeyCode.Backspace)) //temp code start
        {
            TakeDamage(20);
        }

        if (healthSlider.value <= 0) //death animation override
        {
            Invoke("ResetPlayer", 1.5f);
            InvokeRepeating("DeathAnimation", 0, .1f);
            rigid.isKinematic = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D col) //OnCollision deals with physics, OnTrigger deals with "isTrigger"
    {
        if (col.gameObject.name.Contains("ground"))
        {
            anim.SetBool("Falling", false);
            anim.SetBool("Grounded", true);
            onGround = true;
            footstepsActive = false;
            StopTakingDamage();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) //make sure TriggerEnter2D, Collider2D other, and objectCollider2D are all the same type
    {
        
        if (other.gameObject.tag == "Statue") //This will check to see if the player is at one of the first floor statue - William
        {
            interactText.text = "Press 'E' to activate";
            interactText.gameObject.SetActive(true); //Would you beleive me if I told you it took me 15 minutes to realize I forgot this line of code? - William
            triggerObject = other.gameObject.tag;
            nearSomething = true;
        }
        else if (other.gameObject.tag == "Statue_Two") //This will check to see if the player is at one of the first floor statue - William
        {
            interactText.text = "Press 'E' to activate";
            interactText.gameObject.SetActive(true); //Would you beleive me if I told you it took me 15 minutes to realize I forgot this line of code? - William
            triggerObject = other.gameObject.tag;
            nearSomething = true;
        }
        else if (other.gameObject.tag == "Statue_Three") //This will check to see if the player is at one of the first floor statue - William
        {
            interactText.text = "Press 'E' to activate";
            interactText.gameObject.SetActive(true); //Would you beleive me if I told you it took me 15 minutes to realize I forgot this line of code? - William
            triggerObject = other.gameObject.tag;
            nearSomething = true;
        }
        else if (other.gameObject.tag == "Dummy") //This will check to see if the player is at one of the dummy statue - William
        {
            interactText.text = "Press 'E' to activate";
            interactText.gameObject.SetActive(true); //Would you beleive me if I told you it took me 15 minutes to realize I forgot this line of code? - William
        }
        else if (other.gameObject.tag == "Door") //This will check to see if the player is at the statue on the second floor - William
        {
            interactText.text = "Press 'E' to activate";
            interactText.gameObject.SetActive(true);
            //... So i forgot the next two lines when adding this code in. - WIlliam
            triggerObject = other.gameObject.tag;
            nearSomething = true;
        }
        else if (other.gameObject.tag == "Respawn") //This will check to see if the player is at one of the respawn statues - William
        {
            interactText.text = "Press 'E' to set spawn";
            interactText.gameObject.SetActive(true); //Would you beleive me if I told you it took me 15 minutes to realize I forgot this line of code? - William

            triggerObject = other.gameObject.tag;
            nearSomething = true;

            respawnPos = other.gameObject.transform.position;
        }
        else if (other.gameObject.tag == "MirrorPuzzle") //keep camera in fixed positions within the second puzzle room
        {
            Debug.Log("Now in puzzle room");
            camera.lockPosition = new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y, -9);
            camera.lockCamera = true;
            Debug.Log("X = " + other.transform.position.x);
            Debug.Log("Y = " + other.transform.position.y);
        }
        else if (other.gameObject.tag == "MirrorDoor")
        {
            triggerObject = other.gameObject.tag;
            nearSomething = true;

            interactText.text = "Press 'E' to enter";
            interactText.gameObject.SetActive(true);

            for (int x = 0; x < mirrors.Length; x++)
            {
                if (other.gameObject == mirrors[x])
                {
                    mirrorNum = x;
                    break;
                }
            }
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
    //Note From William - I might want to edit some code here so the interactText is different for when you get to statues. I'll try this on a little later.
    //Keep this in mind for the other puzzles as well.

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag != "Damage") 
        {
            triggerObject = "";
            nearSomething = false;
            interactText.gameObject.SetActive(false);
        }

        if (other.gameObject.tag == "MirrorPuzzle")
        {
            camera.lockCamera = false;
        }
        else if (other.gameObject.tag == "Respawn")
        {
            respawnPos = stats.respawn;
            triggerObject = "";
            nearSomething = false;
        }

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "MirrorPuzzle")
        {
            camera.lockCamera = true;
            camera.lockPosition = new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y, -9);
        }

        if (other.gameObject.tag == "MirrorDoor")
        {
            triggerObject = other.gameObject.tag;
            nearSomething = true;

            interactText.text = "Press 'E' to enter";
            interactText.gameObject.SetActive(true);
            for (int x = 0; x < mirrors.Length; x++)
            {
                if (other.gameObject == mirrors[x])
                {
                    mirrorNum = x;
                    break;
                }
            }
        }
    }

    void Move(float direction)
    {
        transform.position = new Vector3(transform.position.x + (direction * Time.deltaTime), transform.position.y, transform.position.z);
        anim.SetBool("Movement", true);

        if (!footstepsActive && onGround)
        {
            footstepsActive = true;
            source.clip = sfx[0];
            InvokeRepeating("PlayFootstepSound", 0, source.clip.length - .925f); //since the clip barely doesn't loop perfectly, use invoke to make it perfect
            source.Play();
        }

    }

    void Shoot()
    {
        source.PlayOneShot(sfx[1]);
        canAttack = false;
        /*if (stats.Ammo > 0)
        {
            stats.Ammo -= 1;
            Debug.Log("Pew pew pew! Ammo remaining: " + stats.Ammo);
        }
        else
        {
            Debug.Log("Out of ammo!");
        }*/
        if(rend.flipX)
        {
            Instantiate(bulletPrefab, new Vector2(this.transform.position.x - 1f, this.transform.position.y), Quaternion.Euler(1, 0, 0));
            StartCoroutine(AttackCooldown());
        }
        else
        {
            Instantiate(bulletPrefab, new Vector2(this.transform.position.x + 1f, this.transform.position.y), Quaternion.Euler(0, 0, 0));
            StartCoroutine(AttackCooldown());
        }
        
    }

    public void TakeDamage(int damage)
    {
        source.PlayOneShot(sfx[3]);
        healthSlider.value -= damage;
        anim.SetBool("TakingDamage", true);
        gameObject.GetComponent<Renderer>().material.color = Color.red;
        speed = 0;
        anim.SetBool("Movement", false);

        if (healthSlider.value <= 0) //something is causing this IF statement to not call sometimes, and I want to know what it is
        {
            Invoke("ResetPlayer", 1.5f);
            InvokeRepeating("DeathAnimation", 0, .1f);
            rigid.isKinematic = true;
        }
        else
        {
            Invoke("StopTakingDamage", .2f);
        }

    }

    void Interact(string type) //near a door? Tell the player to press "E" to go through it
    {
        Debug.Log("Interact");
        switch (type)
        {
            case "Castle":
                Debug.Log("You entered the creepy " + type);
                break;
            case "NPC": //temporary term
                Debug.Log("Dab");
                break;
            case "MirrorDoor":
                Debug.Log("Used the mirror!");
                Teleport();
                break;
                //Will - Need this for the Statues to be activated.
            case "Statue":
                Debug.Log("Let's Activtion!");
                PuzzleController.GetComponent<PuzzleScript>().Activation();
                break;
            case "Statue_Two":
                Debug.Log("Let's Activtion Two!");
                PuzzleController.GetComponent<PuzzleScript>().ActivationTwo();
                break;
            case "Statue_Three":
                Debug.Log("Let's Activtion Tri!");
                PuzzleController.GetComponent<PuzzleScript>().ActivationThree();
                break;
            case "Respawn":
                Debug.Log("So... How was the dying?");
                stats.respawn = respawnPos; //permanently set spawn, until permanently set again
                break;
            case "Door":
                Debug.Log("Let's Activtion 2nd Floor!");
                PuzzleController.GetComponent<PuzzleScript>().ActivateMirror();
                break;
        }
    }

    void UpdateStats()
    {
        if (stats.Level == 0) //if just starting the game
        {
            stats.Level = 1;

            stats.expLvRequired = 200;
        }
        else
        {
            //update UI with gold, current exp / exp needed, current health...
        }
        healthSlider.maxValue = stats.Health;
    }

    void StopTakingDamage()
    {
        anim.SetBool("TakingDamage", false);
        speed = stats.MoveSpeed;
        if (!onGround)
        {
            anim.SetBool("Jumping", false);
            anim.SetBool("Falling", true);
            anim.SetBool("Grounded", false);
        }
        else
        {

        }
        gameObject.GetComponent<Renderer>().material.color = Color.white;
    }

    void ResetPlayer()
    {
        StopTakingDamage();

        Debug.Log("Become dead lol");
        SceneManager.LoadScene("Castle");
    }

    void DeathAnimation()
    {
        transform.Rotate(0, 0, 30);
        transform.localScale = new Vector2(transform.localScale.x - .1f, transform.localScale.y - .1f);
    }

    public void Teleport()
    {
        transform.position = mirrorNewPositions[mirrorNum].transform.position;
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(.7f);
        canAttack = true;
    }

    void PlayFootstepSound()
    {
        source.Play();
    }
}
