using UnityEngine;

public interface IGravity
{
    Rigidbody _rb { set; get; }
    void GetRigidBody();
    void EnableGravity();
    void DisableGravity();
}