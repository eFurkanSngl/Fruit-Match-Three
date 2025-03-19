using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelects : MonoBehaviour
{
    [SerializeField] private Button[] _levelSelectsButton;
    private void Awake()
    {
      
        int unLockedLevel = PlayerPrefs.GetInt("unLockedLevel", 1);

        for (int i = 0; i < _levelSelectsButton.Length; i++)
        {
            _levelSelectsButton[i].interactable = (i < unLockedLevel);
        }
    }
    public void OpenLevel(int levelId)
    {
        PlayerPrefs.SetInt("CurrentLevel",levelId);
        PlayerPrefs.Save();
        SceneManager.LoadScene("level-"+levelId);
    }

    public void UnLockNextLevel()
    {
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);

        int unLockedLevel = PlayerPrefs.GetInt("unLockedLevel", 1);
        if(currentLevel >= unLockedLevel)
        {
            PlayerPrefs.SetInt("unLockedLevel", currentLevel +1);
            PlayerPrefs.Save();
        }
        Debug.Log("UnlockedLevel" + PlayerPrefs.GetInt("UnlockedLevel", 1));
    }

    private void OnEnable()
    {
        GameLevelEvent.LevelEvents += UnLockNextLevel;
    }

    private void OnDisable()
    {
        GameLevelEvent.LevelEvents -= UnLockNextLevel;
    }
}
