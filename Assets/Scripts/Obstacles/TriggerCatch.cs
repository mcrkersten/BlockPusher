using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sends triggered trigger from child to Obstacle
/// </summary>
public class TriggerCatch : MonoBehaviour
{
    [SerializeField] private Obstacle _obstacle;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("GravityTrigger"))
            _obstacle.OnTriggerEnter(collider);        
    }
}
