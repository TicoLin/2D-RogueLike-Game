using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectArea : MonoBehaviour
{
    // Start is called before the first frame update
    private bool on = true;
    private bool off = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.gameObject.CompareTag("BossSubClass"))
        {
            if (collision.CompareTag("Character"))
            {
                this.gameObject.GetComponentInParent<Boss>().ChoosePlayer(collision.GetComponent<Rigidbody2D>());

            }
        }
        else if(this.gameObject.CompareTag("EnemySubClass"))
        {
            if (collision.CompareTag("Character"))
            {
                this.gameObject.GetComponentInParent<Enemy_zombie>().ChoosePlayer(collision.GetComponent<Rigidbody2D>());
                this.gameObject.GetComponentInParent<Enemy_zombie>().SetAttackMode(on);

            }
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (this.gameObject.CompareTag("Boss"))
        {
            if (collision.CompareTag("Character"))
            {
               

            }
        }
        else if(this.gameObject.CompareTag("Enemy"))
        {
            if (collision.CompareTag("Character"))
            {
                this.gameObject.GetComponentInParent<Enemy_zombie>().SetAttackMode(off);

            }
        }
        
    }
}
