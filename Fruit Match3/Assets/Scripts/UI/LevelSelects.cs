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
        int UnLockedLevel = PlayerPrefs.GetInt("UnLockedLevel", 4);

        for (int i = 0; i < _levelSelectsButton.Length; i++)
        {
            _levelSelectsButton[i].interactable = false;
        }
        for (int i = 0; i < UnLockedLevel; i++)
        {
            _levelSelectsButton[i].interactable = true;
        }

    }
    public void OpenLevel(int levelId)
    {
        string levelName = "Level-" + levelId;
        SceneManager.LoadScene(levelName);
    }
}
