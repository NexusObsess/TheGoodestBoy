using UnityEngine;
using System.Collections.Generic;
using Unity.Cinemachine;
public class CameraManager : MonoBehaviour
{
    static List<CinemachineCamera> cameras = new List<CinemachineCamera>();

    public static CinemachineCamera activeCamera = null;

    public static bool IsActiveCamera(CinemachineCamera cam)
    {
        return cam == activeCamera;
    }

    public static void SwitchCamera(CinemachineCamera newcam)
    {
        Debug.Log("Switching to camera: " + newcam.name);
        newcam.Priority = 10;
        activeCamera = newcam;

        foreach (CinemachineCamera cam in cameras)
        {
            if (cam != newcam)
            {
                cam.Priority = 0;
            }
        }
    }

    public static void Register(CinemachineCamera cam)
    {
        cameras.Add(cam);
    }

    public static void Unregister(CinemachineCamera cam)
    {
        cameras.Remove(cam);
    }
}
