using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAction : MonoBehaviour
{
    [SerializeField] private List<AudioSource> _environmentFXMoment = new List<AudioSource>();
    [SerializeField] private List<AudioSource> _environmentFXForest = new List<AudioSource>();

    public void ExitFromGame() => Application.Quit();


    public void SetAudioFXState(bool state)
    {
        _environmentFXMoment.ForEach(m => m.enabled = state);
        StartCoroutine(FXWillBe(_environmentFXForest, state));
    }


    private IEnumerator FXWillBe(List<AudioSource> audios, bool state)
    {
        foreach (var audio in audios)
        {
            audio.enabled = state;
            yield return new WaitForSeconds(Random.Range(1.5f, 3.2f));
        }

        yield break;
    }

}
