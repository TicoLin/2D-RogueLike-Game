using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;


public class SaveLoad : MonoBehaviour
{
    public int money;
    public int maxHealth;
    public int attack;
    public float attack_speed;
    public GameManager gm;
    public PlayerMovement player;
    public static SaveLoad instance
    {
        get; private set;
    }

    private void Awake()
    {
        if (instance!= null & instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
        money = gm.GetMoney();
        maxHealth = player.GetMaxHp();
        attack = player.GetATK();
        attack_speed = player.GetATKRate();
        Load();
        gm.SetMoney(money);
        player.SetMaxHP(maxHealth);
        player.SetATKDMG(attack);
        player.SetATKRate(attack_speed);
    }

    public void Load()
    {
        if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData_Storage data = (PlayerData_Storage)bf.Deserialize(file);

            money = data.money;
            maxHealth = data.maxHealth;
            attack = data.attack;
            attack_speed = data.attack_speed;

            file.Close();

        }
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");
        PlayerData_Storage data = new PlayerData_Storage();

        data.money = money;
        data.maxHealth = maxHealth;
        data.attack=attack;
        data.attack_speed = attack_speed;

    bf.Serialize(file, data);
        file.Close();
    }
}

[Serializable]
class PlayerData_Storage
{
    public int money;
    public int maxHealth;
    public int attack;
    public float attack_speed;
}

