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
    //private BoxCollider2D leftAttack;
    //private BoxCollider2D rightAttack;
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
    public GameObject[] flameballSpawns = new GameObject[24];
    private int phase = 1;
    private bool isActive = false;
    private int count = 0;
    private string lastAttack = "Tentacle";
    #endregion

    public string tagName = "Enemy";
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = enemyData.maxHealth;
        Debug.Log("Enemy" + enemyData.enemyName + " spawned.");
        layerMask = LayerMask.NameToLayer("Player");
        rend = gameObject.AddComponent<SpriteRenderer>();
        anim = gameObject.AddComponent<Animator>();
        //set the animator based on the enemyName
        anim.runtimeAnimatorController = enemyData.animControl;
        rend.sprite = enemyData.enemySprite;
        player = GameObject.FindGameObjectWithTag("Player");
        //this.gameObject.layer = 10;
        if(enemyData.enemyName == "Boss")
        {
            for(int i = 0; i < waypoints.Length; i++ )
            {
                waypoints[i] = GameObject.FindGameObjectWithTag("Waypoint_" + i);
            }
            for(int i = 0; i < flameballSpawns.Length; i++)
            {
                flameballSpawns = GameObject.FindGameObjectsWithTag("FlameballSpawn");
            }
        }
        BoxCollider2D collide = gameObject.AddComponent<BoxCollider2D>();
        collide.size = new Vector2(1, 2);
        if(enemyData.enemyName == "Archer")
        {
            collide.offset = new Vector2(0.31f, -0.65f);
        }
        rend.sortingOrder = 3;
        
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
            float distance = player.transform.position.x - this.transform.position.x;
            
            if (Mathf.Abs(distance) <= enemyData.attackRange && enemyData.enemyName != "Boss"/*(player.transform.position.x <= this.transform.position.x + enemyData.attackRange || player.transform.position.x >= this.transform.position.x - enemyData.attackRange)*/ && player.transform.position.y >= this.transform.position.y - 1.5f && player.transform.position.y <= this.transform.position.y + 1.5f && !isFleeing && !isAttacking && canAttack && !isHit)
            {
                Attack();
            }
            if (enemyData.enemyName == "Archer") //only actions for the archer
            {

                if(player.transform.position.x < this.transform.position.x)
                {
                    rend.flipX = true;
                }
                else
                {
                    rend.flipX = false;
                }


                float fleeDistance;
                if(!rend.flipX)
                {
                    fleeDistance = player.transform.position.x - this.transform.position.x;
                }
                else
                {
                    fleeDistance = player.transform.position.x + this.transform.position.x;
                }
                if (Mathf.Abs(fleeDistance) <= 2f/*(player.transform.position.x <= this.transform.position.x + 2f || player.transform.position.x >= this.transform.position.x - 2f)*/ && player.transform.position.y >= this.transform.position.y - 1f && player.transform.position.y <= this.transform.position.y + 1f && !isAttacking && !isHit)
                {
                    /*isFleeing = true;
                    anim.SetBool("IsFleeing", true);
                    ArcherFlee();*/
                }
                if (Mathf.Abs(fleeDistance) > 2f/*(player.transform.position.x >= this.transform.position.x + 2f || player.transform.position.x <= this.transform.position.x - 2f)*//* && player.transform.position.y >= 1f && player.transform.position.y <= 1f*/ && player.transform.position.y >= this.transform.position.y - 1f && player.transform.position.y <= this.transform.position.y + 1f && !isAttacking && !isHit)
                {
                    /*isFleeing = false;
                    anim.SetBool("IsFleeing", false);*/
                }
            }
            if(enemyData.enemyName != "Archer" && enemyData.enemyName != "Boss" && !isAttacking && !isHit)
            {
                Move();
            }




        }
        
        if(enemyData.enemyName == "Boss" && !isActive)
        {
            if(waypoints[0].gameObject.GetComponent<BossSpawn>().spawned)
            {
                isActive = true;
                Debug.Log("Boss is active");
            }
        }

        if(enemyData.enemyName == "Boss" && isActive && !isAttacking)
        {
            StartCoroutine(BossAttackCycle());
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Bullet")
        {
            TakeDamage(10);
            Debug.Log("Enemy hit from enemy");
            Destroy(collision.gameObject);
        }
    }

   


    private void Move()
    {
        bool canWalk = CheckGroundBeneath();
        if(canWalk)
        {
            if(!rend.flipX)
            {
                //move right
                //play animation
                this.transform.Translate(new Vector3(enemyData.speed, 0, 0));
            }
            else
            {
                //move left
                //play animation
                this.transform.Translate(new Vector3(-enemyData.speed, 0, 0));
            }
        }
        else
        {
            if (!rend.flipX)
            {
                rend.flipX = true;
                //move in left
                //play animation
                this.transform.Translate(new Vector3(-enemyData.speed, 0, 0));
            }
            else
            {
                rend.flipX = false;
                //move in right
                //play animation
                this.transform.Translate(new Vector3(enemyData.speed, 0, 0));
            }
            
        }

    }

    bool CheckGroundBeneath()
    {
        bool isGround;
        if(!rend.flipX)
        {
            
            if(Physics2D.Raycast(new Vector2(this.transform.position.x + .1f, this.transform.position.y), Vector2.down, .1f, layerMask))
            {
                isGround = true;
            }
            else
            {
                isGround = false;
            }
            return isGround;
        }
        else if(rend.flipX)
        {
            if (Physics2D.Raycast(new Vector2(this.transform.position.x - .1f, this.transform.position.y), Vector2.down, .1f, layerMask))
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
        float playerDistance;
        if(!rend.flipX)
        {
            playerDistance = player.transform.position.x - this.transform.position.x;
        }
        else
        {
            playerDistance = player.transform.position.x + this.transform.position.x;
        }
        if (Mathf.Abs(playerDistance) <= 2f && rend.flipX)
        {
            bool canWalk = CheckGroundBeneath();
            rend.flipX = false;
            if (canWalk)
            {
                //move away to the right
                this.transform.Translate(new Vector3(enemyData.speed, 0, 0));
            }
        }

        if (Mathf.Abs(playerDistance) <= 2f && !rend.flipX)
        {
            bool canWalk = CheckGroundBeneath();
            rend.flipX = true;
            if (canWalk)
            {
                //move away to the left
                this.transform.Translate(new Vector3(-enemyData.speed, 0, 0));
            }

        }
    }

    public void TakeDamage(int damage)
    {
        if(!isHit && enemyData.enemyName != "Boss")
        {
            isHit = true;
            Debug.Log("TakeDamage was called");
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
            else if(currentHealth <= 0 && !isDead)
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
        if(enemyData.enemyName == "Archer" && enemyData.enemyName != "Boss")
        {
            if(rend.flipX)
            {
                //do the archer attack
                anim.SetTrigger("IsAttacking1");
                //instantiate the arrow to shoot
                //going to do a raycast forwards for right now
                StartCoroutine(ArcherAttack());
                StartCoroutine(AttackDuration());
                StartCoroutine(AttackCooldown());
            }
            else
            {
                //do the archer attack
                anim.SetTrigger("IsAttacking1");
                //instantiate the arrow to shoot
                //going to do a raycast forwards for right now
                Debug.Log("Before Archer Attack");
                StartCoroutine(ArcherAttack());
                Debug.Log("Before Attack duration");
                StartCoroutine(AttackDuration());
                Debug.Log("Before Attack cooldown");
                StartCoroutine(AttackCooldown());
            }
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
                Instantiate(enemyData.tentacle, new Vector2(player.transform.position.x, player.transform.position.y - 1f), Quaternion.Euler(0, 0, 0));
            }
            else
            {
                //fire rain attack
                StartCoroutine(FireballSpawning());
                
            }
        }

    }

    void BossAttack()
    {
        int random = Random.Range(0, 2);
        if (random == 0)
        {
            //tentacle attack
            Instantiate(enemyData.tentacle, new Vector2(player.transform.position.x, player.transform.position.y - 1f), Quaternion.Euler(0, 0, 0));
            lastAttack = "Tentacle";
        }
        else if(random == 1 && lastAttack == "Tentacle")
        {
            //fire rain attack
            lastAttack = "Fireblast";
            StartCoroutine(FireballSpawning());

        }
    }

    void Death()
    {
        isDead = true;
        Debug.Log("Death was called");
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
            StartCoroutine(Explosion1());
        }
        
    }


    public void HitBox()
    {
        if(!rend.flipX)
        {
            //rightAttack.enabled = true;
            Debug.Log("Attack Right");
            RaycastHit2D hit = Physics2D.BoxCast(new Vector2(this.transform.position.x, this.transform.position.y), new Vector2(3, 1), 0f, Vector2.right, 1);
            Debug.DrawRay(this.transform.position, Vector2.right, Color.red, 1);
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
            RaycastHit2D hit = Physics2D.BoxCast(new Vector2(this.transform.position.x - 1f, this.transform.position.y), new Vector2(3, 1), 0f, Vector2.left, 1);
            Debug.DrawRay(this.transform.position, Vector2.left, Color.red, 1);
            if (hit.transform.gameObject.tag == "Player")
            {
                //player.GetComponent<PlayerScript>().TakeDamage(enemyData.damage);
                Debug.Log("Hit Player");
            }
        }
    }



    #region Enumerators

    IEnumerator FireballSpawning()
    {
        yield return new WaitForSeconds(.1f);
        int random = Random.Range(0, 24);
        count += 1;
        Instantiate(enemyData.flameball, flameballSpawns[random].transform.position, Quaternion.Euler(0, 0, 0));
        if(count < 101)
        {
            StartCoroutine(FireballSpawning());
        }
        else
        {
            count = 0;
        }
    }

    IEnumerator BossAttackCycle()
    {
        isAttacking = true;
        yield return new WaitForSeconds(enemyData.attackCooldown);
        BossAttack();
        if(!isDead)
        {
            StartCoroutine(BossAttackCycle());
        }
        
    }

    IEnumerator SkeletonRevive()
    {
        //play the death animation for skeleton
        anim.SetBool("IsDead", true);
        yield return new WaitForSeconds(5);
        //play the revive animaiton
        anim.SetTrigger("revive");
        anim.SetBool("IsDead", false);
        timesDead += 1;
        isDead = false;
        anim.ResetTrigger("revive");
        
    }

    IEnumerator DeathTimer()
    {
        //play death animation
        //anim.SetBool("IsDead", true);
        Debug.Log("DeathTimer was called");
        anim.SetTrigger("DeadTrigger");
        yield return new WaitForSeconds(5);
        Destroy(this.gameObject);
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(enemyData.attackCooldown);
        canAttack = true;

    }

    IEnumerator ArcherAttack()
    {
        yield return new WaitForSeconds(.2f);
        if(!rend.flipX)
        {
            Instantiate(enemyData.arrowPrefab, new Vector2(this.transform.position.x + 1f, this.transform.position.y - .5f), Quaternion.Euler(0, 0, 0));

            
            /*Debug.Log("Before Raycast right");
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y - .2f), Vector2.right, 10f);
            Debug.Log(hit.transform.gameObject.tag);
            Debug.DrawRay(this.transform.position, Vector2.right, Color.red, 1);
            Debug.Log("After Raycast right");
            if (hit.transform.tag == "Player")
            {
                Debug.Log("Hit Player");
                player.GetComponent<PlayerScript>().TakeDamage(enemyData.damage);
            }*/
        }
        else
        {
            Instantiate(enemyData.arrowPrefab, new Vector2(this.transform.position.x - 1f, this.transform.position.y - .5f), Quaternion.Euler(1, 0, 0));
            /*Debug.Log("Before Raycast left");
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y - .2f), Vector2.left, 10f);
            Debug.Log(hit.transform.gameObject.tag);
            Debug.DrawRay(this.transform.position, Vector2.left, Color.red, 1);
            Debug.Log("After Raycast left");
            if (hit.transform.tag == "Player")
            {
                Debug.Log("Hit Player");
                player.GetComponent<PlayerScript>().TakeDamage(enemyData.damage);
            }*/
        }
        
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
        if(enemyData.enemyName != "Archer")
        {
            anim.ResetTrigger("IsAttacking2");
        }
        
        //leftAttack.enabled = false;
        //rightAttack.enabled = false;
    }

    IEnumerator HitDuration()
    {
        yield return new WaitForSeconds(1f);
        isHit = false;
        anim.ResetTrigger("IsHit");
    }

    IEnumerator Explosion1()
    {
        Instantiate(enemyData.ExplosionHandler, new Vector2(this.transform.position.x + 1, this.transform.position.y + 3), Quaternion.Euler(0,0,0));
        yield return new WaitForSeconds(1.1f);

        StartCoroutine(Explosion2());
    }
    IEnumerator Explosion2()
    {
        Instantiate(enemyData.ExplosionHandler1, new Vector2(this.transform.position.x + .5f, this.transform.position.y), Quaternion.Euler(0, 0, 0));
        yield return new WaitForSeconds(1.6f);


        StartCoroutine(Explosion3());
    }
    IEnumerator Explosion3()
    {
        Instantiate(enemyData.ExplosionHandler2, new Vector2(this.transform.position.x, this.transform.position.y + 1), Quaternion.Euler(0, 0, 0));
        yield return new WaitForSeconds(1.6f);


        StartCoroutine(ExplosionFinal());
    }
    IEnumerator ExplosionFinal()
    {
        Instantiate(enemyData.ExplosionHandler3, new Vector2(this.transform.position.x + .7f, this.transform.position.y + .7f), Quaternion.Euler(0, 0, 0));
        yield return new WaitForSeconds(11f);
        

        //call the new scene
    }

    #endregion
}
