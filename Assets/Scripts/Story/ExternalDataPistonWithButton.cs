using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExternalDataPistonWithButton : MonoBehaviour
{
    [SerializeField] private GameObject _prefabUI;
    [SerializeField] private bool _externalDependency;

    [SerializeField] public UnityEvent Event_Complete = new UnityEvent();
    private Inventory _inventory;
    private FixedButton _button;
    private GameObject _instaledPrefab;
    private Transform _parentUI;

    private void Awake()
    {
        _inventory = GameObject.FindGameObjectWithTag("GlobalScript").GetComponent<Inventory>();

        if (_parentUI == null)
        {
            _parentUI = GameObject.FindGameObjectWithTag("TIS").transform;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (_instaledPrefab == null)
            {
                _instaledPrefab = Instantiate(_prefabUI, _parentUI);

                _button = _instaledPrefab.GetComponent<FixedButton>();
                var banner = _instaledPrefab.GetComponent<Banner>();
                banner.Title = "Ship repair";
                banner.enabled = true;

                _instaledPrefab.SetActive(_externalDependency);
            }
            else
            {
                _instaledPrefab.SetActive(_externalDependency);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_button != null && _button.Pressed)
        {
            var items = _inventory.InventoryItems;

            if (items[2].Amount >= 20 && items[3].Amount >= 5 && items[6].Amount >= 10)
            {
                _inventory.SubtractingAmountItem(2, out int _, 20);
                _inventory.SubtractingAmountItem(3, out int _, 5);
                _inventory.SubtractingAmountItem(6, out int _, 10);

                StartCoroutine(Execute());
                Destroy(_instaledPrefab);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (_instaledPrefab != null)
            {
                _instaledPrefab.SetActive(false);
            }
        }
    }

    private IEnumerator Execute()
    {
        var waitFading = true;
        Fader.instance.FadeIn(() => waitFading = false);

        while (waitFading)
            yield return null;

        Event_Complete.Invoke();

        waitFading = true;
        Fader.instance.FadeOut(() => waitFading = false);

        while (waitFading)
            yield return null;
    }

    public void DeleteObject(GameObject gameObject) => Destroy(gameObject);
    public void SetStateOfExternalDependency(bool value) => _externalDependency = value;

}
