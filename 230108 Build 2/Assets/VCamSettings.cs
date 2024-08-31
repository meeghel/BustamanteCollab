using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VCamSettings : MonoBehaviour
{
    CinemachineVirtualCamera vcam;
    GameObject player;
    Transform followTarget;

    private void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        followTarget = player.transform;
        vcam.LookAt = followTarget;
        vcam.Follow = followTarget;
    }
}