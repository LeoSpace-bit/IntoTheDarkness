using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private GameRuler _rules;
    [SerializeField] private Daytime _time;

    [SerializeField] private AudioSource _breath;
    [SerializeField] public List<AudioSource> ForestSource = new List<AudioSource>();
    
    [SerializeField] private List<AudioSource> _monsterSource = new List<AudioSource>();
    [SerializeField] private List<AudioClip> _monsterClips = new List<AudioClip>();

    private bool _isMonsterNextGeneration = true;
    private float _audioVolume = 1f;
    private float[] _times = new float[3];

    private void Start() => GenerateTimeStamp();

    private void Update()
    {
        if(_rules.IsNight)
        {
            if(_audioVolume > 0f)
            {
                _audioVolume -= 0.1f * Time.deltaTime;
                
                if (_audioVolume < 0f)
                {
                    _audioVolume = 0f;
                }
            }

            foreach (var value in _times)
            {
                if(Mathf.Abs(value - _time.TimeProgress) < 0.075f)
                {
                    PlayMonsterScream();
                }
            }
        }
        else
        {
            if (_audioVolume < 1f)
            {
                _audioVolume += 0.1f * Time.deltaTime;

                if (_audioVolume > 1f)
                {
                    _audioVolume = 1f;
                }
            }
        }

        if(_audioVolume != 1 || _audioVolume != 0)
        {
            ForestSource.ForEach(x => x.volume = _audioVolume);
            _breath.volume = 1.1f - _audioVolume;

        }

    }


    public void GenerateTimeStamp()
    {
        _times[0] = Random.Range(0f, 0.15f);
        _times[1] = Random.Range(0.2f, 0.255f);
        _times[2] = Random.Range(0.78f, 1);
    }

    public void ResetMonsterScream() => GenerateTimeStamp();

    private AudioSource GetAudioSource() => _monsterSource[Random.Range(0, _monsterClips.Count - 1)];
    private AudioClip GetMonsterClip() => _monsterClips[Random.Range(0, _monsterClips.Count - 1)];

    public void PlayMonsterScream()
    {
        var source = GetAudioSource();
        source.clip = GetMonsterClip();
        source.Play();
    }

}
