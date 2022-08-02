using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float ZoomOutSize = 70;

    private new Camera camera;
    private GameObject player;

    private bool isZoomedOut;
    private float previousOrthographicSize;
    private Vector3 previousPosition;

    public void Start()
    {
        camera = GetComponent<Camera>();
    }

   

    public void LateUpdate()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Character");
        }

        if (player != null)
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
        }
    }
}

