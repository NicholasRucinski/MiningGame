using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class FollowPlayer : NetworkBehaviour
{
    public GameObject camera;
    public Vector3 offset;
    private Vector3 mousePos;

    public override void OnNetworkSpawn()
    {
        camera.SetActive(IsOwner);
        base.OnNetworkSpawn();
    }
    void Update()
    {
        camera.transform.position = transform.position + offset;
    }
}
