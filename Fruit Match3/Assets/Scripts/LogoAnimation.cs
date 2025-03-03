using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine;


public class LogoAnimation : MonoBehaviour
{
    private Tween _logoTween;
    private bool _isFirstScene = false;
    private void Start()
    {
        StartLogoAnimation();
        _isFirstScene = true;
    }

    private void StartLogoAnimation()
    {
        _logoTween = transform.DOScale(1.2f, 0.5f).
            SetLoops(-1, LoopType.Yoyo).
            SetEase(Ease.InOutSine);
    }
    private void StopLogoAnimation()
    {
        if (_logoTween != null)
        {
            _logoTween.Kill();
        }
        Destroy(gameObject);
        Debug.Log("Destroy Logo");
    }

    private void OnActiveSceneChanged(Scene scene, Scene next)
    {
        if (_isFirstScene)
        {
            StopLogoAnimation();
        }
    }


    private void OnEnable()
    {
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }
    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
        StopLogoAnimation();
    }
}
