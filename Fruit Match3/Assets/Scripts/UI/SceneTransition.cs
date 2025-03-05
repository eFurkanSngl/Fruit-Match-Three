using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private CanvasGroup _fadePanel;
    private WaitForSeconds _openTime = new WaitForSeconds(1f);
    private WaitForSeconds _closeTime = new WaitForSeconds(0.5f);


    private void Start()
    {
        _fadePanel.alpha = 1f;
        _fadePanel.DOFade(0, 1f);  // Scene open fade-out
    }

    private void HandleSceneChange(string sceneName)
    {
        StartCoroutine(Transition(sceneName));
    }
    private IEnumerator Transition(string SceneName)
    {
        _fadePanel.DOFade(1, 1f);
        yield return _openTime;
        SceneManager.LoadScene(SceneName);
        yield return _closeTime;
        _fadePanel.DOFade(0, 1f);
    }
    private void OnEnable()
    {
        RegisterEvents();
    }
    private void OnDisable()
    {
        UnRegisterEvents();
    }

    private void RegisterEvents() => ScenesEvets.OnSceneChanged += HandleSceneChange;
    private void UnRegisterEvents() => ScenesEvets.OnSceneChanged -= HandleSceneChange;
}
