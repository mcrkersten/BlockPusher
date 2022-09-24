using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOverTime : MonoBehaviour
{
    [SerializeField] private bool _physics = true;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private angle _rotationAngle;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (_physics)
            AddRotationPhysics();
        else
            AddRotation();
    }

    private void AddRotationPhysics()
    {
        switch (_rotationAngle)
        {
            case angle.X:
                _rb.angularVelocity = new Vector3(_rotationSpeed, 0, 0);
                break;
            case angle.Y:
                _rb.angularVelocity = new Vector3(0, _rotationSpeed, 0);
                break;
            case angle.Z:
                _rb.angularVelocity = new Vector3(0, 0, _rotationSpeed);
                break;
        }
    }

    private void AddRotation()
    {
        switch (_rotationAngle)
        {
            case angle.X:
                transform.Rotate(Vector3.right, _rotationSpeed);
                break;
            case angle.Y:
                transform.Rotate(Vector3.up, _rotationSpeed);
                break;
            case angle.Z:
                transform.Rotate(Vector3.forward, _rotationSpeed);
                break;
        }
    }

    private enum angle
    {
        X, Y, Z
    }
}
