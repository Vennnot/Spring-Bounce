using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI sfxText;
    [SerializeField] private TextMeshProUGUI musicText;
    public bool isEnabled;

    public void ToggleSFX()
    {
        if (SoundManager.Instance.ToggleSFX())
        {
            sfxText.text = "Toggle SFX: On";
        }
        else
        {
            sfxText.text = "Toggle SFX: Off";
        }
    }

    public void ToggleMusic()
    {
        if (SoundManager.Instance.ToggleMusic())
        {
            musicText.text = "Toggle Music: On";
        }
        else
        {
            musicText.text = "Toggle Music: Off";
        }
    }

    public void ToggleEnabled()
    {
        isEnabled = !isEnabled;
    }
}
