using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, Enemies
{

    public GameObject coin;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void DropCoins(int number)
    {
        for (int i = 0;i<number;i++)
        {
            GameObject temp = Instantiate(coin);
            temp.transform.position = transform.position;
        }
        
    }


    public virtual void GetDamage(int x)
    {

    }

    public virtual void Die()
    {

    }

}
