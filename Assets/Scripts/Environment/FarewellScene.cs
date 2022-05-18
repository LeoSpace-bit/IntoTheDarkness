using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class FarewellScene : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private Toast _toast;
    [SerializeField] private Canvas _mainMenu;

    [SerializeField] private GameObject _ship;
    [SerializeField] private float _shipSpeed = 10f;

    public void ChangeCamere()
    {
        _virtualCamera.enabled = true;

        BeginEndScene();
    }

    public void BeginEndScene()
    {
        _ship.transform.position = new Vector3(359.3f, 1.2f, -313.2f);
        _ship.transform.rotation = Quaternion.Euler(0f, 45f, -90f);

        StartCoroutine(ShipSailing());

    }

    private IEnumerator ShipSailing()
    {
        while(_ship.transform.position.x < 540 && _ship.transform.position.z > -494)
        {
            _ship.transform.Translate(Vector3.up * _shipSpeed *  Time.deltaTime);
            yield return null;
        }

        _toast.ShowToast($"Time flows inexorably and there is no turning back.{Environment.NewLine}Reflect on your journey");
        yield return new WaitForSeconds(3);

        _mainMenu.enabled = true;

        yield break;
    }


}
