using System.Collections;
using System.Collections.Generic;
using Microsoft.Win32.SafeHandles;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour, IDataPersistence
{
    public static GameController Instance;

    public int playerDeaths = 0;

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
    }

    public void SaveData(GameData data)
    {
        data.deathCount = playerDeaths;
    }

    public void IncrementDeaths()
    {
        Debug.Log("Increment deaths was called");
        playerDeaths++;
    }
}
