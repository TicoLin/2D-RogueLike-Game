using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector2 mosPos;
    private Vector2 lookdir;
    public Rigidbody2D guns;
    public Camera cam;
    public SpriteRenderer gunSR;
    public GameObject stPT;
    private int change=0;
    

    private void Start()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }
        
        guns = this.GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        
        mosPos = cam.ScreenToWorldPoint(Input.mousePosition);
        lookdir = mosPos - guns.position;
        float angle= Mathf.Atan2(lookdir.y, lookdir.x) * Mathf.Rad2Deg;
        //gunSR.flipY = (angle> -90f && angle < 90f) ? false : true;
        if (change == 0)
        {
           
        }
        
        if (angle > -90f && angle < 90f)
        {
            if (change == 2)
            {
                stPT.transform.Rotate(180, 0, 0);
            }
            change = 1;

        }
        else
        {
            if (change == 1 || change ==0)
            {
                stPT.transform.Rotate(180, 0, 0);
            }

            change = 2;
            
        }
        

        guns.transform.eulerAngles = new Vector3(0, 0, angle);
        
    }
}
