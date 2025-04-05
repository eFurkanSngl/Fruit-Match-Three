using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextLevel : UIBTN
{
    [SerializeField] private Slider _timeSlider;
    protected override void OnClick()
    {
        GoToLevelSelect();
    }
    private void GoToLevelSelect()
    {
        SceneManager.LoadScene("Level-Select");
        _timeSlider.animator.speed = 1.0f;
    }
}
