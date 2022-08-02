using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelLoader : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator transition;
    private bool enter = false;
    public float transistionTime = 3f;

    // Update is called once per frame
    void Update()
    {
        if (enter)
        {
            LoadNextScene();
        }
        enter = false;
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                LoadNextScene();
            }
        }

        if (SceneManager.GetActiveScene().buildIndex == 5)
        {
            if (Input.GetKeyDown("space"))
            {
                LoadSpecificScene("HomeBase");
            }
        }
        else if (SceneManager.GetActiveScene().buildIndex >= 2)
        {
            if (Input.GetKeyDown("space"))
            {
                LoadNextScene();
            }
        }


        
    }

    public void LoadNextScene()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));      
    }

    public void LoadSpecificScene(string name)
    {
        StartCoroutine(LoadLevel(name));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Load");
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadSceneAsync(levelIndex);
        Time.timeScale = 1;

    }
    IEnumerator LoadLevel(string levelName)
    {
        yield return new WaitForSecondsRealtime(1.5f);
        transition.SetTrigger("Load");
        yield return new WaitForSecondsRealtime(0.1f); 
        SceneManager.LoadSceneAsync(levelName);
        Time.timeScale = 1;

    }

    public void PlayerEnteredTheNextStage()
    {
        enter = true;
    }
}
