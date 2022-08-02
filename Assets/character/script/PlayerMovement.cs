using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    #region Data Setting
    private GameManager gm;
    #endregion

    private AudioSource gunFire;

    #region HP Setting
    public static int maxHP = 100;
    private static int curHP;
    private static bool isDead;  
    public HealthBar healthBar;
    
    #endregion

    public float moveSpeed = 4f;
    public Rigidbody2D rigidbody2;
    public Rigidbody2D hitBox;
    private Vector2 movement;
    private Vector2 mosPos;
    private Vector2 lookdir;
    public Animator animator;
    public Rigidbody2D gun;
    public GameObject bullet;
    public GameObject stPT;
    

    #region Camera Setting
    public Camera cam;
    public CameraShake cameraShake;
    #endregion

    #region Attack Setting
    public static int attackDamage = 2;
    private bool attack;
    public static float attackRate= 4;
    private float nextAttackTime = 0f;
    #endregion

    #region InteractItem
    private IInteractable interactableInFocus;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        #region Search Component
        if (cam == null)
        {
            cam = Camera.main;
        }
        if (healthBar == null)
        {
            healthBar = FindObjectOfType<HealthBar>();
        }
        if (cameraShake == null)
        {
            cameraShake = FindObjectOfType<CameraShake>();
        }
        if (gm == null)
        {
            gm = FindObjectOfType<GameManager>();
        }

        if (gunFire == null)
        {
            gunFire = GetComponent<AudioSource>();
        }
        #endregion

        if (SceneManager.GetActiveScene().buildIndex <= 1)
        {
            curHP = maxHP;
        }

        isDead = false;
 
        healthBar.SetMaxHealth(maxHP);

    }

    // Update is called once per frame
    void Update()
    {
        #region Player Stat
        healthBar.SetHealth(curHP);
        #endregion

   
        #region Animator
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        mosPos = cam.ScreenToWorldPoint(Input.mousePosition);
        
        animator.SetFloat("Horizontal", lookdir.x);
        animator.SetFloat("Vertical", lookdir.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
        #endregion

        #region interaction
        if (interactableInFocus != null)
        {
            if (interactableInFocus.IsInteractionAllowed())
            {
                interactableInFocus.Interact();
            }
            else
            {
                interactableInFocus.EndInteract();
                interactableInFocus = null;
            }
        }
        #endregion

        hitBox.GetComponent<HitBox>().ChangeADPlayer(attackDamage);

        #region Attack Logic
        if (Time.time >= nextAttackTime && !animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
                
                //gameObject.transform.GetChild(0).gameObject.SetActive(true);
                nextAttackTime = Time.time + 1f / attackRate;
            }
            
        }
        if (!attack)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
        #endregion 

        Death(isDead);
    }
    private void FixedUpdate()
    {
        #region Movement and Look Direction
        lookdir = mosPos - rigidbody2.position;
        float angle = Mathf.Atan2(lookdir.y, lookdir.x) * Mathf.Rad2Deg - 90f;
        hitBox.rotation = angle;
        
        if (attack)
        {
            rigidbody2.MovePosition(rigidbody2.position + movement * moveSpeed * Time.fixedDeltaTime*0.8f);
        }
        else
        {
            rigidbody2.MovePosition(rigidbody2.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
        
        #endregion
    }

    #region function for Attack
    void Attack()
    {
        attack = true;
        gunFire.Play();
        StartCoroutine(Fire());
        //animator.SetTrigger("Attack");
    
    }
    public void FinishedAttack()
    {
        attack = false;
    }
    #endregion

    #region function for interaction
    public void OnTriggerEnter2D(Collider2D collider)
    {
        var interactable = collider.GetComponentInParent<IInteractable>();

        if (interactable == null || !interactable.IsInteractionAllowed())
        {
            return;
        }

        interactableInFocus?.EndInteract();
        interactableInFocus = interactable;
        interactableInFocus.BeginInteract();
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        var interactable = collider.GetComponentInParent<IInteractable>();

        if (interactable == interactableInFocus)
        {
            interactableInFocus?.EndInteract();
            interactableInFocus = null;
        }
    }
    #endregion

    #region function for GetDamage and Death
    public void GetHurt(int damage)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("hurt"))
        {
            curHP -= damage;
            healthBar.SetHealth(curHP);
            animator.SetTrigger("Hurt");
            if (cameraShake != null)
            {
                StartCoroutine(cameraShake.Shake(0.15f, 0.15f));
            }
            
            if (curHP <= 0)
            {
                isDead = true;
            }
        }
            
    }

    private void Death(bool dead)
    {
        var gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (dead)
        {
            animator.SetBool("Death", true);
            gameManager.PlayerIsDead();
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
    #endregion

    public int GetCurHp()
    {
        return curHP;
    }

    public int GetMaxHp()
    {
        return maxHP;
    }

    public void SetMaxHP(int hp)
    {
        maxHP = hp;
    }

    public int GetAttackDMG()
    {
        return attackDamage;
    }

    public void SetATKDMG(int dmg)
    {
        attackDamage = dmg;
    }

    public float GetATKRate()
    {
        return attackRate;
    }

    public void SetATKRate(float rate)
    {
        attackRate = rate;
    }
    public void AddCoins()
    {
        gm.AddCoins();
    }

    IEnumerator Fire()
    {
        Vector3 fireDirection = gun.transform.right;
        GameObject temp = Instantiate(bullet, stPT.transform.position, gun.transform.rotation) as GameObject;
        temp.GetComponent<Rigidbody2D>().AddForce(temp.transform.right*1000f);
        yield return null;
    }

    public void HpUp()
    {
        maxHP = (int)(maxHP * 1.2);
        healthBar.SetMaxHealth(maxHP);
        curHP = maxHP;
        healthBar.SetHealth(curHP);

    }

    public void AtkUp()
    {
        var atk = attackDamage * 1.2;
        if (atk % attackDamage > 0)
        {
            attackDamage = attackDamage + 1 + (int)(atk / attackDamage);
        }
    }
    public int GetATK()
    {
        return attackDamage;
    }
    public void SpeedUp()
    {
        attackRate *= 1.2f;
    }

}
