using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscillateHeight : MonoBehaviour
{
    [SerializeField] private float _height;
    [SerializeField] private float _speed;
    private float _timer = 0f;

    private float _startHeight;

    private void Awake()
    {
        _startHeight = transform.position.y;
    }

    private void FixedUpdate()
    {
        _timer += Time.deltaTime;
        Vector3 position = transform.position;
        position[1] = _startHeight + Oscillate(_timer, _speed, _height);
        transform.position = position;
    }

    float Oscillate(float time, float speed, float scale)
    {
        return Mathf.Cos(time * speed / Mathf.PI) * scale;
    }
}
