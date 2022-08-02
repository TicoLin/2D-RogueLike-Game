using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
    public Collider2D coinCollider;
    public Rigidbody2D coinRig;
    public Collider2D collectArea;

    private float pasttime;
    private float delay;
    private float when = 1.0f;
    private Vector3 pop_dir;

    private GameObject player;
    private bool magnetize = false;
    private bool collected = false;
    private void Awake()
    {
        pop_dir = new Vector3(Random.Range(-3f, 3f), pop_dir.y, pop_dir.z);
        pop_dir = new Vector3(pop_dir.x, Random.Range(-3f, 3f), pop_dir.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (when >= delay)
        {
            pasttime = Time.deltaTime;
            this.gameObject.transform.position += pop_dir * Time.deltaTime;
            delay += pasttime;
        }

        if (magnetize)
        {
            Vector3 dir = Vector3.MoveTowards(transform.position, player.transform.position, 30 * Time.deltaTime);
            coinRig.MovePosition(dir);
        }

        if (collected)
        {
            Destroy(gameObject);
        }

    }

    private IEnumerator Magnet()
    {
        yield return new WaitForSeconds(1f);
        magnetize = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Character"))
        {
            player = GameObject.FindWithTag("Character");
            StartCoroutine(Magnet());
        }
    }

    public void Collected()
    {
        collected = true;
    }
}
