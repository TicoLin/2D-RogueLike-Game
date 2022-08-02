using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_zombie : EnemyBase
{
    public GameManager gm;

    public int maxHealth = 100;
    private int curHealth;
    private bool isDead;

    public float moveSpeed = 4f;
    private Vector2 movement;
    public Rigidbody2D player;
    public Rigidbody2D zombie;
    public Collider2D playerDetectArea;
    private Vector2 playerPos;
    private Vector2 zombiePos;
    private Vector2 lookdir;
    public Animator animator;
    public Rigidbody2D hitBox;


    private int attackDamage = 5;
    private bool attack;
    public float attackRate;
    private float nextAttackTime = 0f;
    private bool attackMode;
    private bool inAttackRange;
    
    // Start is called before the first frame update
    void Start()
    {
        curHealth = maxHealth;
        isDead = false;
        attackMode = false;
        if (gm == null)
        {
            gm = FindObjectOfType<GameManager>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            playerPos = player.transform.position;
            zombiePos = zombie.transform.position;

            animator.SetFloat("Horizontal", lookdir.x);
            animator.SetFloat("Vertical", lookdir.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);
        }

        hitBox.GetComponent<HitBox>().ChangeADEnemy(attackDamage);

        if (isDead)
        {
            Die();
        }
        else if (inAttackRange && !animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            ZombieAttackLogic();//need timer for prepare attack
        }
        
 
    }

    private void FixedUpdate()
    {
        if(!isDead && !animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            Move();
        }
        
        float angle = Mathf.Atan2(lookdir.y, lookdir.x) * Mathf.Rad2Deg - 90f;
        hitBox.rotation = angle;
    }

    void ZombieAttackLogic()
    {
        if (Time.time >= nextAttackTime)
        {
            if(!animator.GetCurrentAnimatorStateInfo(0).IsName("hurt"))
            {
                Attack();
                gameObject.transform.GetChild(0).gameObject.SetActive(true);
                nextAttackTime = Time.time + 1f / attackRate;
            }
            else
            {
                nextAttackTime = Time.time + 0.6f / attackRate;
            }
        }
            
        if (!attack)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    private void Move()
    {
        lookdir = playerPos - zombiePos;
        if (attackMode && Time.time >= nextAttackTime)
        {
            movement = Vector2.MoveTowards(zombie.position, playerPos, moveSpeed * Time.deltaTime);
            zombie.position = movement;
        }
        else if( Time.time >= nextAttackTime - 1f / attackRate && Time.time < nextAttackTime - (1f / attackRate)/2)
        {
            movement = Vector2.MoveTowards(zombie.position, playerPos, -1*moveSpeed/2.8f * Time.deltaTime);
            zombie.position = movement;
        }
        else
        {
            movement = Vector2.zero;
            zombie.velocity = Vector2.zero;
        }
        

    }

    public void SetAttackMode(bool onOrOff)//playerSearchingArea
    {
        attackMode = onOrOff;
    }

    public void SetInAttackRange(bool insideOrNot)//attackRange
    {
        inAttackRange = insideOrNot;
    }

    public override void GetDamage(int damage)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("hurt"))
        {
            curHealth -= damage;
            animator.SetTrigger("Hurt");
            if (curHealth <= 0)
            {
                isDead = true;
            }
        }
        
        

    }

    void Attack()
    {
        attack = true;
        animator.SetTrigger("Attack");

    }

    public void FinishedAttack()
    {
        attack = false;
    }
    public override void Die()
    {
        animator.SetBool("Death", true);
        zombie.mass = 1000000;
    }

    public void Destroy()
    {
        gm.AddKill();
        DropCoins(5);
        Destroy(gameObject);
    }

    public void ChoosePlayer(Rigidbody2D player)
    {
        this.player = player;
    }
}
