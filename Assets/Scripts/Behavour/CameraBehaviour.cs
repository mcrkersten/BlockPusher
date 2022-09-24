using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _offset;
    private void FixedUpdate()
    {
        Vector3 newPos = this.transform.position;
        newPos[2] = Mathf.Lerp(this.transform.position.z, _player.transform.position.z + _offset, Time.deltaTime);
        this.transform.position = newPos;
    }
}
