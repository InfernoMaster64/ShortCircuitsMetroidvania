using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyData enemyData;
    private int currentHealth;
    public GameObject player;
    private SpriteRenderer rend;
    private Animator anim;
    private BoxCollider2D leftAttack;
    private BoxCollider2D rightAttack;
    public LayerMask layerMask;

    private int timesDead = 0;

    #region States
    private bool isDead = false;
    private bool isFleeing = false;
    private bool isAttacking = false;
    private bool canAttack = true;
    private bool isHit = false;
    #endregion

    #region Boss
    public GameObject[] waypoints = new GameObject[3];
    private int phase = 1;
    private bool isActive = false;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = enemyData.maxHealth;
        Debug.Log("Enemy" + enemyData.enemyName + " spawned.");
        layerMask = LayerMask.NameToLayer("Ground");
        rend = gameObject.AddComponent<SpriteRenderer>();
        anim = gameObject.AddComponent<Animator>();
        //set the animator based on the enemyName
        anim.runtimeAnimatorController = enemyData.animControl;
        rend.sprite = enemyData.enemySprite;
        player = GameObject.FindGameObjectWithTag("Player");
        if(enemyData.enemyName == "Boss")
        {
            for(int i = 0; i < waypoints.Length; i++ )
            {
                waypoints[i] = GameObject.FindGameObjectWithTag("Waypoint_" + i);
            }
        }
        gameObject.AddComponent<BoxCollider2D>();
        

        /*leftAttack = gameObject.AddComponent<BoxCollider2D>();
        leftAttack.offset = new Vector2(1f, 0f);
        leftAttack.isTrigger = true;
        leftAttack.enabled = false;

        rightAttack = gameObject.AddComponent<BoxCollider2D>();
        rightAttack.offset = new Vector2(-1f, 0f);
        rightAttack.isTrigger = true;
        rightAttack.enabled = false;*/
    }

    private void Update()
    {
        if(!isDead) //all actions go in here
        {
            if ((player.transform.position.x <= this.transform.position.x + enemyData.attackRange || player.transform.position.x >= this.transform.position.x - enemyData.attackRange) && player.transform.position.y >= this.transform.position.y - 1f && player.transform.position.y <= this.transform.position.y + 1f && !isFleeing && !isAttacking && canAttack && !isHit)
            {
                Attack();
            }
            if (enemyData.enemyName == "Archer") //only actions for the archer
            {
                if ((player.transform.position.x <= this.transform.position.x + 2f || player.transform.position.x >= this.transform.position.x - 2f) && player.transform.position.y >= this.transform.position.y - 1f && player.transform.position.y <= this.transform.position.y + 1f && !isAttacking && !isHit)
                {
                    isFleeing = true;
                    ArcherFlee();
                }
                if ((player.transform.position.x >= this.transform.position.x + 2f || player.transform.position.x <= this.transform.position.x - 2f) && player.transform.position.y >= 1f && player.transform.position.y <= 1f)
                {
                    isFleeing = false;
                }
            }
            if(enemyData.enemyName != "Archer" && enemyData.enemyName != "Boss" && !isAttacking && !isHit)
            {
                Move();
            }




        }
        
        if(enemyData.enemyName == "Boss" && !isActive)
        {
            if(player.transform.position.x == waypoints[0].transform.position.x && player.transform.position.y >= waypoints[0].transform.position.y - 1f && player.transform.position.y <= waypoints[0].transform.position.y + 1f)
            {
                isActive = true;
            }
        }



        if(Input.GetKeyDown(KeyCode.P))
        {
            Attack();
        }
        if(Input.GetKeyDown(KeyCode.O))
        {
            TakeDamage(10);
        }





    }

    private void Move()
    {
        bool canWalk = CheckGroundBeneath();
        if(canWalk)
        {
            if(rend.flipX)
            {
                //move right
                //play animation
                this.transform.Translate(new Vector3(-enemyData.speed, 0, 0));
            }
            else
            {
                //move left
                //play animation
                this.transform.Translate(new Vector3(enemyData.speed, 0, 0));
            }
        }
        else
        {
            if (rend.flipX)
            {
                rend.flipX = false;
                //move in left
                //play animation
                this.transform.Translate(new Vector3(enemyData.speed, 0, 0));
            }
            else
            {
                rend.flipX = true;
                //move in right
                //play animation
                this.transform.Translate(new Vector3(-enemyData.speed, 0, 0));
            }
            
        }

    }

    bool CheckGroundBeneath()
    {
        bool isGround;
        if(rend.flipX == true)
        {
            
            if(Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y), Vector2.down, .1f, layerMask))
            {
                isGround = true;
            }
            else
            {
                isGround = false;
            }
            return isGround;
        }
        else if(rend.flipX == false)
        {
            if (Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y), Vector2.down, .1f, layerMask))
            {
                isGround = true;
            }
            else
            {
                isGround = false;
            }
            return isGround;
        }
        Debug.Log("Did not check for the ground.");
        return false;
    }

    void ArcherFlee()
    {
        if (player.transform.position.x <= this.transform.position.x + 2f && player.transform.position.x != this.transform.position.x)
        {
            bool canWalk = CheckGroundBeneath();
            rend.flipX = true;
            if (canWalk)
            {
                //move away to the right
                this.transform.Translate(new Vector3(-enemyData.speed, 0, 0));
            }
        }
        if (player.transform.position.x >= this.transform.position.x - 2f && player.transform.position.x != this.transform.position.x)
        {
            bool canWalk = CheckGroundBeneath();
            rend.flipX = false;
            if (canWalk)
            {
                //move away to the left
                this.transform.Translate(new Vector3(enemyData.speed, 0, 0));
            }

        }
    }

    public void TakeDamage(int damage)
    {
        if(!isHit && enemyData.enemyName != "Boss")
        {
            isHit = true;
            //play 'hit' animation
            anim.SetTrigger("IsHit");
            currentHealth -= damage;
            if (currentHealth <= 0 && enemyData.enemyName != "Boss")
            {
                Death();
            }
            else if(currentHealth > 0 && enemyData.enemyName != "Boss")
            {
                StartCoroutine(HitDuration());
            }
        }
        if(enemyData.enemyName == "Boss")
        {
            if(currentHealth <= 66 && phase == 1)
            {
                phase = 2;
                //teleport to second location
                this.transform.position = waypoints[1].transform.position;
            }
            else if(currentHealth <= 33 && phase == 2)
            {
                phase = 3;
                //teleport to third location
                this.transform.position = waypoints[2].transform.position;
            }
            else if(currentHealth <= 0)
            {
                Death();
            }
        }

    }

    void Attack()
    {
        isAttacking = true;
        canAttack = false;
        //play animation
        //if player gets hit with the trigger created, then deal damage
        if(enemyData.enemyName == "Archer")
        {
            //do the archer attack
            //instantiate the arrow to shoot
            StartCoroutine(AttackDuration());
            StartCoroutine(AttackCooldown());
        }
        else if(enemyData.enemyName != "Archer" && enemyData.enemyName != "Boss")
        {
            int random = Random.Range(0, 2);
            if(random == 0)
            {
                if(rend.flipX)
                {
                    //do attack 1 animation
                    anim.SetTrigger("IsAttacking1");
                    //animation should create a trigger prefab
                    //all of this to the right
                    StartCoroutine(HitBoxTrigger());
                    StartCoroutine(AttackDuration());
                    StartCoroutine(AttackCooldown());
                }
                else
                {
                    //do attack 1 animation
                    anim.SetTrigger("IsAttacking1");
                    //animation should create a trigger prefab
                    //all of this to the left
                    StartCoroutine(HitBoxTrigger());
                    StartCoroutine(AttackDuration());
                    StartCoroutine(AttackCooldown());
                }

            }
            else if(random == 1)
            {
                if (rend.flipX)
                {
                    //do attack 2 animation
                    anim.SetTrigger("IsAttacking2");
                    //animation should create a trigger prefab
                    //all of this to the right
                    StartCoroutine(HitBoxTrigger());
                    StartCoroutine(AttackDuration());
                    StartCoroutine(AttackCooldown());
                }
                else
                {
                    //do attack 2 animation
                    anim.SetTrigger("IsAttacking2");
                    //animation should create a trigger prefab
                    //all of this to the left
                    StartCoroutine(HitBoxTrigger());
                    StartCoroutine(AttackDuration());
                    StartCoroutine(AttackCooldown());
                }
            }
        }
        else if(enemyData.enemyName == "Boss")
        {
            int random = Random.Range(0, 2);
            if(random == 0)
            {
                //tentacle attack
            }
            else
            {
                //fire rain attack
            }
        }

    }

    void Death()
    {
        isDead = true;
        if(enemyData.enemyName != "Boss")
        {
            if (enemyData.enemyName == "Skeleton" && timesDead < 1)
            {
                StartCoroutine(SkeletonRevive());
            }
            else
            {
                StartCoroutine(DeathTimer());
            }
        }
        else if(enemyData.enemyName == "Boss")
        {
            //do the boss death
        }
        
    }


    public void HitBox()
    {
        if(rend.flipX)
        {
            //rightAttack.enabled = true;
            Debug.Log("Attack Right");
            RaycastHit2D hit = Physics2D.BoxCast(new Vector2(this.transform.position.x - 1f, this.transform.position.y), new Vector2(1, 1), 0f, Vector2.right, 9);
            if(hit.transform.gameObject.tag == "Player")
            {
                //player.GetComponent<PlayerScript>().TakeDamage(enemyData.damage);
                Debug.Log("Hit Player");
            }
        }
        else
        {
            //leftAttack.enabled = true;
            Debug.Log("Attack Left");
            RaycastHit2D hit = Physics2D.BoxCast(new Vector2(this.transform.position.x + 1f, this.transform.position.y), new Vector2(1, 1), 0f, Vector2.left, 9);
            if (hit.transform.gameObject.tag == "Player")
            {
                //player.GetComponent<PlayerScript>().TakeDamage(enemyData.damage);
                Debug.Log("Hit Player");
            }
        }
    }



    #region Enumerators

    IEnumerator SkeletonRevive()
    {
        //play the death animation for skeleton
        anim.SetBool("IsDead", true);
        yield return new WaitForSeconds(5);
        //play the revive animaiton
        anim.SetBool("IsDead", false);
        anim.SetTrigger("revive");
        timesDead += 1;
        isDead = false;
        anim.ResetTrigger("revive");
        
    }

    IEnumerator DeathTimer()
    {
        //play death animation
        anim.SetBool("IsDead", true);
        yield return new WaitForSeconds(5);
        Destroy(this);
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(enemyData.attackCooldown);
        canAttack = true;

    }

    IEnumerator HitBoxTrigger()
    {
        yield return new WaitForSeconds(.2f);
        HitBox();
    }

    IEnumerator AttackDuration()
    {
        yield return new WaitForSeconds(.5f);
        isAttacking = false;
        anim.ResetTrigger("IsAttacking1");
        anim.ResetTrigger("IsAttacking2");
        //leftAttack.enabled = false;
        //rightAttack.enabled = false;
    }

    IEnumerator HitDuration()
    {
        yield return new WaitForSeconds(1);
        isHit = false;
        anim.ResetTrigger("IsHit");
    }

    #endregion
}
