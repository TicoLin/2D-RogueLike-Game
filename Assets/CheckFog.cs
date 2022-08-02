using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CheckFog : MonoBehaviour
{
    RaycastHit2D[] hit;
    int frames = 0;
    
    void FixedUpdate()
    {
        frames++;
        if (frames == 4)
        {
            frames = 0;

            
            hit = Physics2D.CircleCastAll(transform.position, 2f, new Vector2(0, 0), 3f);

            foreach (var item in hit)
            {
                Vector3 dis = transform.position - item.collider.transform.position;
                if (dis.x == 2 || dis.x == -2 || dis.y == 2 || dis.y == -2)
                {
                    item.collider.transform.gameObject.GetComponent<SpriteRenderer>().color = new Color(.5f, .5f, .5f, 1.0f);
                }
                else
                {
                    item.collider.transform.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1.0f);

                }
            }
        }
    }
}
