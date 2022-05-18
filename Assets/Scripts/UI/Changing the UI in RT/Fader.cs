using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : MonoBehaviour
{
    private const string FADER_PATH = "Prefabs/Fader";
    [SerializeField] private Animator _animator;

    private static Fader _instance;
    public static Fader instance
    {
        get
        {
            if (_instance == null)
            {
                var prefab = Resources.Load<Fader>(FADER_PATH);
                _instance = Instantiate(prefab);
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    private Action _fadedInCallback;
    private Action _fadedOutCallback;

    public bool isFading { get; private set; }

    public void FadeIn(Action fadedInCallback)
    {
        if (isFading) return;

        isFading = true;
        _fadedInCallback = fadedInCallback;

        _animator.SetBool("Faded", true);

    }

    public void FadeOut(Action fadedOutCallback)
    {
        if (isFading) return;

        isFading = true;
        _fadedOutCallback = fadedOutCallback;

        _animator.SetBool("Faded", false);
    }

    private void Handle_FadeInAnimationOver()
    {
        _fadedInCallback?.Invoke();
        _fadedInCallback = null;
        isFading = false;
    }

    private void Handle_FadeOutAnimationOver()
    {
        _fadedOutCallback?.Invoke();
        _fadedOutCallback = null;
        isFading = false;
    }


}
