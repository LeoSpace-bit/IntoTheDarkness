using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Attack : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    
    private bool _active = false;

    public void Interact()
    {
        foreach (Collider collider in Physics.OverlapSphere(transform.position, 1))
        {
            if (collider.TryGetComponent<AdvancedTree>(out AdvancedTree tree))
            {
                if(!_active)
                {
                    _active = true;
                    StartCoroutine(PlayAttackAnimation(tree));
                }
                
            }

        }
    }

    private IEnumerator PlayAttackAnimation(AdvancedTree advancedTree)
    {
        _animator.Play("Attack");

        while(IsPlaying("Attack"))
        {
            yield return null;
        }
       
        advancedTree.Damage(25, transform.forward);
        _active = false;
    }

    private bool IsPlaying(string stateName)
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName(stateName) && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            return true;
        else
            return false;
    }

}
