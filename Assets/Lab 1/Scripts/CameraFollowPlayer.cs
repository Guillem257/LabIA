using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public Transform player;
    public void Update()
    {
        Vector3 playerPos = player.position;
        playerPos.y = transform.position.y;
        transform.position = playerPos;
    }
}
