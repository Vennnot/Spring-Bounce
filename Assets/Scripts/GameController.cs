using System.Collections;
using System.Collections.Generic;
using Microsoft.Win32.SafeHandles;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour, IDataPersistence
{
    public static GameController Instance;

    public int playerDeaths = 0;

    public int playerKills = 0;

    public int levelUnlocked = 1;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void QuitGame(){
        Application.Quit();
    }

    public void LoadData(GameData data)
    {
        this.playerDeaths = data.deathCount;
        this.playerKills = data.killCount;
        this.levelUnlocked = data.levelUnlocked;
    }

    public void SaveData(GameData data)
    {
        //Saved Data
        data.deathCount = playerDeaths;
        data.killCount = playerKills;
        data.levelUnlocked = levelUnlocked;
    }

    public void IncrementDeaths()
    {
        playerDeaths++;
    }
    public void IncrementKills()
    {
        playerKills++;
    }
    
}
