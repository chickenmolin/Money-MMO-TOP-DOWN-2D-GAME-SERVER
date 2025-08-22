using Unity.Netcode;
using UnityEngine;
using Cinemachine;

public class PlayerCameraSetup : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            var vCam = FindObjectOfType<CinemachineVirtualCamera>();
            if (vCam != null)
            {
                vCam.Follow = transform;
                // Không cần LookAt cho 2D
            }
        }
    }
}
