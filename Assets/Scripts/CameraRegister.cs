using UnityEngine;
using Unity.Cinemachine;
using System.Collections.Generic;
public class CameraRegister : MonoBehaviour
{
    private void OnEnable()
    {
        CameraManager.Register(GetComponent<CinemachineCamera>());
    }
    private void OnDisable()
    {
        CameraManager.Unregister(GetComponent<CinemachineCamera>());
    }
}
