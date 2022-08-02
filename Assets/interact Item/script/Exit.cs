using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;


    /// <summary>
    /// Example implementation of an exit is activated by pressing E and loads the next level.
    /// </summary>
public class Exit : InteractableBase
{

    
    public override void BeginInteract()
    {
        ShowText("Press E to exit the level");
    }

    public override void Interact()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            var levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
            levelLoader.LoadNextScene();
        }
    }

    public override void EndInteract()
    {
        HideText();
    }
}

