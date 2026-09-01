using System;
using NUnit.Framework.Constraints;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class WorldRotator : MonoBehaviour
{
    [Header("-------------- Required Objects")]
    [SerializeField] private InputActionReference deltaAction;
    [SerializeField] private GameObject boat;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var rb = gameObject.GetComponent<Rigidbody>();
        var dir = rb.angularVelocity.normalized;
        float ang = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        var boatRotation = Quaternion.Lerp(boat.transform.rotation, Quaternion.Euler(0, (ang*1)-0, 0), Time.deltaTime * 5);
        boat.transform.rotation = boatRotation;
    }

    private void RotateGlobe(Vector2 dir, float speed)
    {
        var rb = gameObject.GetComponent<Rigidbody>();
        var norm = dir.normalized;
        rb.angularVelocity += new Vector3(norm.y, norm.x, 0) * speed;
        float ang = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }

    private void Move(InputAction.CallbackContext obj)
    {
        float sensitivity = 1.0f;
        Vector2 delta = obj.ReadValue<Vector2>() * sensitivity * Time.deltaTime;
        delta.x = Mathf.Clamp(delta.x, -1, 1);
        delta.y = Mathf.Clamp(delta.y, -1, 1);
        Vector2 dir = delta.normalized;
        float speed = delta.magnitude;
        RotateGlobe(obj.ReadValue<Vector2>(), speed);
    }

    private void OnEnable()
    {
        deltaAction.action.performed += Move;
    }

    private void OnDisable()
    {
        deltaAction.action.performed -= Move;
    }
}
