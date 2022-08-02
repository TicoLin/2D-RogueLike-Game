using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;


public class GameManager : MonoBehaviour
{
    private Text timeUsed;
    private Text coinGot;
    private Text noOfEnemyKilled;
    private Text vectory;
    private Text vectoryShadow;
    private Text coinInText;
    public Text hpValue;

    public static int money;
    private static bool added = false;
    public static int coin;
    public static float time;
    public static int enemyKilled;
    public static bool gameStarted;
    public static bool gameEnded;
    public static bool playerIsDead;
    public static bool bossIsDead;
    public GameObject menu;
    public GameObject setting;
    public Slider slider;
    public GameObject upG;
    private static int upCounter = 0;
    private static int menuCounter = 0;
    private static int settingCounter = 0;
    public AudioMixer audioMixer;
    public GameObject exit;
    public PlayerMovement player;
    [SerializeField]
    PlayerData playerData;


    private void Start()
    {
        slider.value = 0.5f;
        if (SceneManager.GetActiveScene().buildIndex == 5)
        {
            timeUsed = GameObject.Find("EndGameCanvas")?.transform.Find("hourglass")?.transform.Find("time")?.GetComponent<Text>();
            coinGot = GameObject.Find("EndGameCanvas")?.transform.Find("coin")?.transform.Find("coinNumber")?.GetComponent<Text>();
            noOfEnemyKilled = GameObject.Find("EndGameCanvas")?.transform.Find("enemy")?.transform.Find("enemyNumber")?.GetComponent<Text>();
            vectory = GameObject.Find("EndGameCanvas")?.transform.Find("GameOverText")?.GetComponent<Text>();
            vectoryShadow = GameObject.Find("EndGameCanvas")?.transform.Find("GameOverShadow")?.GetComponent<Text>();
        }
        else
        {
            coinInText = GameObject.Find("Canvas")?.transform.Find("CurrentCoins")?.Find("text")?.GetComponent<Text>();
            hpValue = GameObject.Find("Canvas")?.transform.Find("health_Bar")?.Find("text")?.GetComponent<Text>();
        }
        player = GameObject.Find("Character")?.transform.GetComponent<PlayerMovement>();

    }
    private void Update()
    {
        player = GameObject.Find("Character")?.transform.GetComponent<PlayerMovement>();
        if (SceneManager.GetActiveScene().buildIndex ==0)
        {
            playerData = new PlayerData();
        }
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            gameEnded = false;
            added = false;
            time = 0;
            enemyKilled = 0;
            coin = 0;
        }


        if (bossIsDead && exit != null)
        {
            StartCoroutine(ActivateExit());
        }

        if (SceneManager.GetActiveScene().buildIndex != 5)
        {
            //hpValue.text = player.GetCurHp() + "/" + player.GetMaxHp();
            if (gameStarted)
            {
                time += (float)(Time.deltaTime);

            }

            if (gameEnded || playerIsDead)
            {
                var levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
                levelLoader.LoadSpecificScene("EndSettle");

            }
        }
        else
        {
            if (added == false)
            {
                playerData = JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString("jsondata"));
                money = playerData.money;
                money += coin;
                added = true;
            }

            gameEnded = true;
            gameStarted = false;
            float minutes = Mathf.FloorToInt(time / 60);
            float seconds = Mathf.FloorToInt(time % 60);
            timeUsed.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            coinGot.text = coin.ToString();
            noOfEnemyKilled.text = enemyKilled.ToString();
            if (!playerIsDead)
            {
                vectory.text = "Vectory";
                vectoryShadow.text = "Vectory";
            }
            playerIsDead = false;
            playerData.money = money;
            PlayerPrefs.SetString("jsondata", JsonUtility.ToJson(playerData));

        }

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            
            playerData = JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString("jsondata"));
            money = playerData.money;
            coinInText.text = money.ToString();

        }
        else if (coinInText != null)
        {
            coinInText.text = coin.ToString();
        }

    }
    IEnumerator ActivateExit()
    {
        yield return new WaitForSeconds(3);
        exit.SetActive(true);
    }

    #region game state 
    public void BossIsDead()
    {
        bossIsDead = true;
    }

    public void StartGame()
    {
        gameStarted = true;
    }
    public void EndGame()
    {
        gameEnded = true;
    }

    public void PlayerIsDead()
    {
        playerIsDead = true;
    }
    #endregion

    #region stat function
    public void AddCoins()
    {
        coin += 1;
    }

    public void AddKill()
    {
        enemyKilled += 1;
    }
    #endregion

    #region menu
    public void ActivateMenu()
    {
        if (menuCounter == 0)
        {
            StartCoroutine(Menu());
            menuCounter += 1;
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
            StartCoroutine(DeMenu());
            menuCounter = 0;
        }

    }
    IEnumerator Menu()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        RectTransform menuPos = menu.GetComponent<RectTransform>();
        menuPos.anchoredPosition = new Vector3(0, 0, 0);

    }

    IEnumerator DeMenu()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        RectTransform menuPos = menu.GetComponent<RectTransform>();
        menuPos.anchoredPosition = new Vector3(0, 500, 0);

    }

    public void BackToHomePage()
    {
        gameStarted = false;
        gameEnded = false;
        var levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
        levelLoader.LoadSpecificScene("HomeBase");
        time = 0;
        coin = 0;
        enemyKilled = 0;


    }

    public void AdjustSetting()
    {
        if (settingCounter == 0)
        {
            //Time.timeScale = 1;
            StartCoroutine(Setting());
            settingCounter += 1;
        }
        else
        {
            // Time.timeScale = 1;
            StartCoroutine(DeSetting());
            settingCounter = 0;
            // Time.timeScale = 0;
        }

    }

    public void SetVolume()
    {

        float sliderValue = slider.value;
        audioMixer.SetFloat("volume", sliderValue);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void QuitGame()
    {
        StartCoroutine(QuitG());
    }

    IEnumerator QuitG()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        Application.Quit();
    }

    IEnumerator Setting()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        StartCoroutine(DeMenu());
        RectTransform settingPos = setting.GetComponent<RectTransform>();
        settingPos.anchoredPosition = new Vector3(0, 0, 0);
    }

    IEnumerator DeSetting()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        RectTransform settingPos = setting.GetComponent<RectTransform>();
        settingPos.anchoredPosition = new Vector3(0, 500, 0);
        StartCoroutine(Menu());

    }
    #endregion

    #region UpGrade
    public void UpGradeHP()
    {
        if (money >= 100)
        {
            player.HpUp();
            money -= 100;
        }

    }

    public void UpGradeATK()
    {
        if (money >= 50)
        {
            player.AtkUp();
            money -= 50;
        }
    }

    public void UpGradeSpeed()
    {
        if (money >= 100)
        {
            player.SpeedUp();
            money -= 100;
        }

    }

    public void ActivateUpGrade()
    {
        if (upCounter == 0)
        {
            StartCoroutine(UpGrade());
            upCounter += 1;
        }
        else
        {
            StartCoroutine(DeUpGrade());
            upCounter = 0;
        }

    }
    IEnumerator UpGrade()
    {
        yield return new WaitForSeconds(0.1f);
        RectTransform upPos = upG.GetComponent<RectTransform>();
        upPos.anchoredPosition = new Vector3(0, 0, 0);
    }

    IEnumerator DeUpGrade()
    {
        yield return new WaitForSeconds(0.1f);
        RectTransform upPos = upG.GetComponent<RectTransform>();
        upPos.anchoredPosition = new Vector3(0, 700, 0);

    }

    #endregion

    #region Save Load

    public int GetMoney()
    {
        return money;
    }

    public void SetMoney(int m)
    {
        money = m;
    }
    #endregion
    [System.Serializable]
    public class PlayerData
    {
        public int money;
    }
}

