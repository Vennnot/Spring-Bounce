using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{ float levelLoadDelay = 2;
    private Animator animator;
    [SerializeField] private AudioClip levelEnd;
    [SerializeField] private int levelNumber = 1;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            SoundManager.Instance.PlaySound(levelEnd);
            animator.SetTrigger("isLevelEnding");
            if (GameController.Instance.levelUnlocked < levelNumber)
            {
                GameController.Instance.levelUnlocked = levelNumber;
            }

                StartCoroutine(LoadNextLevel());
        }
    }

    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }

        if (GameController.Instance.levelUnlocked < nextSceneIndex)
        {
            GameController.Instance.levelUnlocked = nextSceneIndex;
        }
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}