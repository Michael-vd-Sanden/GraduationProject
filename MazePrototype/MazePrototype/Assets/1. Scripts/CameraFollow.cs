using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    [SerializeField] private Vector3 offset;

    private void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
