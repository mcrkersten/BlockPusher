using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBlocker : MonoBehaviour
{
    public Collider blockCollider;
    public Collider objectCollider;

    // Start is called before the first frame update
    void Start()
    {
        Physics.IgnoreCollision(blockCollider, objectCollider);
    }
}
