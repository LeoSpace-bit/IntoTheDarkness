using Assets.Scripts.Story;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    [SerializeField] private Text _title;
    [SerializeField] private Text _description;

    [SerializeField] private Text _bookTitle;
    [SerializeField] private Text _bookDescription;

    [SerializeField] private Animator _animator;

    public List<Story> Stories = new List<Story>();

    private int _historyProgress;

    public int HistoryProgress
    {
        get => _historyProgress; set
        {
            if (value > Stories.Count) Debug.LogError("The story is over");
            StartCoroutine(SetValue(value));
        }
    }

    public void SetStoryID(int index) => HistoryProgress = index;
    public void SetNextStoryID() => SetStoryID(1 + _historyProgress);

    private IEnumerator SetValue(int value)
    {
        _historyProgress = value;

        _bookTitle.text = Stories[_historyProgress].Title;
        _bookDescription.text =  Stories[_historyProgress].FullDescription;

        _title.text = Stories[_historyProgress].Title;
        _description.text = Stories[_historyProgress].Description;

        _animator.SetBool("Faded", true);

        yield return new WaitForSeconds(5);

        _animator.SetBool("Faded", false);
    }
}