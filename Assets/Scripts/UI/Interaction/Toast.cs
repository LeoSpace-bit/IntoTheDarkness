using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Toast : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Text _text;

    private Action _fadedInCallback;
    private Action _fadedOutCallback;

    private bool _isAnimating = false;

    public bool isFading { get; private set; }

    public void FadeIn(Action fadedInCallback)
    {
        if (isFading) return;
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


    public void ShowToast(string line)
    {
        _text.text = line;

        if(_isAnimating == false)
        {
            StartCoroutine(BeginToast());
        }
        
    }

    private IEnumerator BeginToast()
    {
        //if (_isAnimating) yield return null;

        _isAnimating = true;

        var waitFading = true;
        FadeIn(() => waitFading = false);
        while(waitFading) yield return null;


        yield return new WaitForSeconds(2);


        waitFading = true;
        FadeOut(() => waitFading = false);
        while (waitFading) yield return null;

        _isAnimating = false;
    }


}
