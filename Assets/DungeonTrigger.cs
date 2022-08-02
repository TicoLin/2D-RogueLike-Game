using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonTrigger : MonoBehaviour
{
    public LevelLoader levelLoader;
    public GameManager gm;
    // Start is called before the first frame update

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Character"))
        {
            gm.StartGame();
            levelLoader.PlayerEnteredTheNextStage();  
        }

    }


}
