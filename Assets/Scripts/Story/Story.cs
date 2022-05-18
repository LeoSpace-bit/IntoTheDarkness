using UnityEngine;

namespace Assets.Scripts.Story
{
    [System.Serializable]
    public class Story
    {
        [SerializeField] public string Title;
        [SerializeField] public string Description;
        [SerializeField, Multiline] public string FullDescription;
    }
}
