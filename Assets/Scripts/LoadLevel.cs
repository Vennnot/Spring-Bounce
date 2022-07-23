using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class LoadLevel : MonoBehaviour
{
    [SerializeField] private int level;
    [SerializeField] private bool levelUnlocked;
    private Image image;
    private Button button;
    void Start()
    {
        if (GameController.Instance.levelUnlocked >= level)
        {
             levelUnlocked = true;
        }

        if (!levelUnlocked)
        {
            image = GetComponent<Image>();
            button = GetComponent<Button>();
            image.color = Color.grey;
            button.enabled = false;
        }
    }

    public void LoadSelectedLevel()
    {
        if(levelUnlocked)
        {
            SceneManager.LoadScene(level);
        }
    }
}
