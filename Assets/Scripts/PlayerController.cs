
using System.Collections; 
using System.Collections.Generic; 
using UnityEngine;
using Unity.Netcode;


public class PlayerController: NetworkBehaviour
    {
    private Vector3 inputSpeed;
    [SerializeField]private float moveSpeed = 5;
    private Vector3 otherPos;
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if (IsOwner)
        {
            inputSpeed.x = Input.GetAxis("Horizontal");
            inputSpeed.y = Input.GetAxis("Vertical");
            transform.position += inputSpeed * moveSpeed * Time.deltaTime;
            if (NetworkManager.Singleton.IsClient)
            {
                SyncPlayerPosServerRpc(transform.position);
            }
        }
        else { 
            transform.position =otherPos;
        }
        
    }
    [ServerRpc]
    void SyncPlayerPosServerRpc(Vector3 pos)
    {
        otherPos = pos;
    }
}