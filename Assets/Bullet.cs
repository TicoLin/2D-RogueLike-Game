using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    public static int bulletDamage = 3;
    private int MDamge = 3;
    private PlayerMovement player;
    // Update is called once per frame
    private void Start()
    {
        player = GameObject.Find("Character")?.GetComponent<PlayerMovement>();
    }
    void Update()
    {
        transform.Translate(new Vector3(transform.rotation.x, transform.rotation.y, 0) * 400f * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.gameObject.CompareTag("EBullet"))
        {
            if(collision.CompareTag("Character")|| collision.CompareTag("Wall"))
            {
                if (collision.CompareTag("Character"))
                {
                    collision.GetComponent<PlayerMovement>().GetHurt(MDamge);
                }
                Destroy(this.gameObject);
            }
            
        }

        if (this.gameObject.CompareTag("Bullet"))
        {
            
            if(collision.CompareTag("Boss") || collision.CompareTag("Enemy") || collision.CompareTag("Wall"))
            {
                if (collision.CompareTag("Boss") || collision.CompareTag("Enemy"))
                {
                    collision.GetComponent<Enemies>().GetDamage(bulletDamage + player.GetATK());
                }
                Destroy(this.gameObject);
            }
            
        }
    }
}

