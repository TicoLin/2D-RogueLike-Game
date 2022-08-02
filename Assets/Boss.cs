using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : EnemyBase
{
    #region HP setting
    public int maxHealth = 600;
    private int curHealth;
    public bool isDead=false;
    #endregion

    #region Random Movement
    private float latestDirectionChangeTime;
    private readonly float directionChangeTime = 1f;
    private float characterVelocity = 2f;
    private Vector2 movementDirection;
    private Vector2 movementPerSecond;
    #endregion

    public Animator animator;
    public Collider2D playerDetectArea;

    #region Look Direction
    private Rigidbody2D player;
    private Rigidbody2D boss;
    private Vector2 lookdir;
    private Vector2 playerPos;
    private Vector2 bossPos;
    #endregion

    public GameObject bullet;
    public GameManager gm;

    void Start()
    {
        #region HP
        curHealth = maxHealth;
        isDead = false;
        #endregion

        if (gm == null)
        {
            gm = FindObjectOfType<GameManager>();
        }

        boss = gameObject.GetComponent<Rigidbody2D>();
        

        #region Random Movement
        latestDirectionChangeTime = 0f;
        calcuateNewMovementVector();
        #endregion
    }

    

    void Update()
    {
        #region Look direction
        lookdir = playerPos - bossPos;
        if (player != null)
        {
            playerPos = player.transform.position;
            bossPos = boss.transform.position;

            animator.SetFloat("Horizontal", lookdir.x);
            animator.SetFloat("Vertical", lookdir.y);
            animator.SetFloat("Speed", 1);//not in charge mode
        }
        #endregion

        #region Attack
        if (Time.time - latestDirectionChangeTime > directionChangeTime && !isDead)
        {
            StartCoroutine(Fire());
        }
        #endregion

        #region Random Movement
        //if the changeTime was reached, calculate a new movement vector
        if (!isDead)
        {
            if (Time.time - latestDirectionChangeTime > directionChangeTime)
            {
                latestDirectionChangeTime = Time.time;
                calcuateNewMovementVector();

            }

            //move enemy: 
            transform.position = new Vector2(transform.position.x + (2 * movementPerSecond.x * Time.deltaTime),
            transform.position.y + (2 * movementPerSecond.y * Time.deltaTime));
        }
        #endregion

        

        #region Death
        if (isDead)
        {
            Die();
        }
        #endregion

    }

    void calcuateNewMovementVector()
    {
        //create a random direction vector with the magnitude of 1, later multiply it with the velocity of the enemy
        movementDirection = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
        movementPerSecond = movementDirection * characterVelocity;
    }

    public void ChoosePlayer(Rigidbody2D player)
    {
        this.player = player;
    }

    public void FinishedAttack()
    {
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
                if (gm != null)
                {
                    gm.BossIsDead();
                }
                
            }
        }
    }

    public override void  Die()
    {
        animator.SetBool("Death", true);
        boss.mass = 1000000;
    }
    public void Destroy()
    {
        gm.AddKill();
        DropCoins(15);
        Destroy(gameObject);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        latestDirectionChangeTime = Time.time;
        calcuateNewMovementVector();
    }

    IEnumerator Fire()
    {
        Vector3 fireDirection = this.transform.up;
        Quaternion startQuaternion = Quaternion.AngleAxis(30,Vector3.forward);
        for (int i = 0; i < 3; i++)
        {
            for(int j= 0; j < 12; j++)
            {
                GameObject temp = Instantiate(bullet);
                temp.transform.position = transform.position;
                temp.transform.rotation = Quaternion.Euler(fireDirection);
                fireDirection = startQuaternion * fireDirection;
            }
            yield return new WaitForSeconds(0.5f);
        }
        yield return null;
    }

}
