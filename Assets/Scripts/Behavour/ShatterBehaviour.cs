using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatterBehaviour : MonoBehaviour
{
    [SerializeField] private float _randomTorque;
    [SerializeField] private float _explosionForce;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private Transform _shardParent;

    private List<Rigidbody> _rigidbodies = new List<Rigidbody>();


    private void Start()
    {
        Player._onGameOver += OnGameOver;
        _rigidbodies = GetAllRigidbodies();
    }

    private List<Rigidbody> GetAllRigidbodies()
    {
        List<Rigidbody> rigidbodies = new List<Rigidbody>();
        foreach (Transform item in _shardParent)
        {
            Rigidbody rb;
            if (item.TryGetComponent(out rb))
                rigidbodies.Add(rb);
        }
        return rigidbodies;
    }

    private void OnGameOver(bool playerFinished)
    {
        if(!playerFinished)
            ActivateShards();
    }

    private void ActivateShards()
    {
        Transform root = _rigidbodies[0].transform.root;
        Vector3 velocity = root.GetComponent<Rigidbody>().velocity;
        _rigidbodies[0].transform.parent.gameObject.SetActive(true);

        foreach (Rigidbody rb in _rigidbodies)
            AddForcesToShard(rb, velocity);

        root.gameObject.SetActive(false);
    }

    private void AddForcesToShard(Rigidbody rb, Vector3 parentVelocity)
    {
        rb.isKinematic = false;
        rb.velocity = parentVelocity;
        rb.AddExplosionForce(_explosionForce, rb.transform.parent.transform.position, _explosionRadius);

        Vector3 torque;
        torque.x = Random.Range(-_randomTorque, _randomTorque);
        torque.y = Random.Range(-_randomTorque, _randomTorque);
        torque.z = Random.Range(-_randomTorque, _randomTorque);
        rb.AddTorque(torque);

        rb.gameObject.transform.parent = null;
    }

    private void OnDestroy()
    {
        Player._onGameOver -= OnGameOver;
    }
}
