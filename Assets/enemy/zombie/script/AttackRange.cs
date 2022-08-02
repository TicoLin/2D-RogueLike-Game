using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    private bool inside = true;
    private bool outside = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Character"))
        {
            this.gameObject.GetComponentInParent<Enemy_zombie>().SetInAttackRange(inside);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Character"))
        {
            this.gameObject.GetComponentInParent<Enemy_zombie>().SetInAttackRange(outside);
        }
    }
}
