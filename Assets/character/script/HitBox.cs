using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    private int ADPlayer;
    private int ADEnemy;
    public Rigidbody2D item;

    private void Start()
    {

    }


    public void Deactivate()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }

    public List<Collider2D> enemies = new List<Collider2D>();
    public List<Collider2D> player = new List<Collider2D>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (item.CompareTag("Character"))
        {
            if (collision.CompareTag("Enemy") || collision.CompareTag("Boss"))
            {
                enemies.Add(collision);
            }
        }
        
        if (item.CompareTag("Enemy"))
        {
            if (collision.CompareTag("Character"))
            {
                player.Add(collision);
            }
        }
        
    }

    private void Update()
    {
        foreach(Collider2D enemy in enemies)
        {

            var eenemy = enemy.GetComponentInParent<Enemies>();
            eenemy.GetDamage(ADPlayer);

        }

        foreach (Collider2D character in player)
        {

            character.GetComponent<PlayerMovement>().GetHurt(ADEnemy);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
        enemies.Remove(collision);
        player.Remove(collision);
        
    }

    public void ChangeADPlayer(int damage)
    {
        ADPlayer = damage;
    }

    public void ChangeADEnemy(int damage)
    {
        ADEnemy = damage;
    }

}
