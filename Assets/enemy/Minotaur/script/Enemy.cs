using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    private int curHealth;
    private bool isDead;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        curHealth = maxHealth;
        isDead = false;
    }

    // Update is called once per frame

    void Update()
    {
        if (isDead && !animator.GetCurrentAnimatorStateInfo(0).IsName("minotaur_death"))
        {
            Die();
        }
    }
    
    

    public void GetDamaged(int damage)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("minotaur_hurt"))
        {
            curHealth -= damage;
            animator.SetTrigger("Hurt");
            if (curHealth <= 0)
            {
                isDead = true;
            }
        }
        
    }

    private void Die()
    {
        animator.SetBool("Death",true);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
