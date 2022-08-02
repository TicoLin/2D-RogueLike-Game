using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapControl : MonoBehaviour
{
    public GameObject player;


    private void LateUpdate()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Character");
        }
        else
        {
            Vector3 newPos = player.transform.position;
            newPos.z = transform.position.z;
            transform.position = newPos;
        }
        

    }

}
