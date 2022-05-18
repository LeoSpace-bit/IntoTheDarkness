using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crutch : MonoBehaviour
{
    [SerializeField] private AudioSource _attack;
    public void PlayAudio() => _attack.Play();

}
