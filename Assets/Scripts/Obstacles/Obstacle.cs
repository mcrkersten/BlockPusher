using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;

[ExecuteInEditMode]
public class Obstacle : MonoBehaviour, IGravity
{
    IGravity _Igravity;

    [SerializeField] private Obstacle_SO _obstacleSettings;
    private ObstacleType _originalType;
    public ObstacleType ObstacleType { 
        set { 
            _obstacleType = value; 
            _obstacleSettings.BuildObject(this); 
        } 
        get {
            return _obstacleType; 
        }
    }
    [SerializeField] private ObstacleType _obstacleType;

    public Collider _collisionBlocker;
    public Collider _collisionBox;
    public Renderer _renderer;
    public Rigidbody _rb { set; get; }

    /// <summary>
    /// In editor force update of Obstacle
    /// </summary>
    [ContextMenu("Update type")]
    public void ForceUpdate()
    {
        ObstacleType = _obstacleType;
    }

    private void Awake()
    {
        Player._powerUp += OnPowerUp;

        _Igravity = gameObject.GetComponent<IGravity>();
        _Igravity.GetRigidBody();
        _obstacleSettings.BuildObject(this);
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("GravityTrigger"))
            _Igravity.EnableGravity();
    }

    private void OnPowerUp()
    {
        if (IsVisibleToCamera(this.transform))
            ChangeToObstacleType(ObstacleType.Positive);
    }

    private void ChangeToObstacleType(ObstacleType obstacleType)
    {
        _originalType = ObstacleType;
        ObstacleType = obstacleType;
    }

    private void ChangeToOriginal()
    {
        ObstacleType = _originalType;
    }

    private void OnDestroy()
    {
        Player._powerUp -= OnPowerUp;
    }

    private static bool IsVisibleToCamera(Transform transform)
    {
        Vector3 visTest = Camera.main.WorldToViewportPoint(transform.position);
        return (visTest.x >= 0 && visTest.y >= 0) && (visTest.x <= 1 && visTest.y <= 1) && visTest.z >= 0;
    }

#region Interface
    void IGravity.GetRigidBody()
    {
        Rigidbody rb;
        if (!TryGetComponent(out rb))
            rb = GetComponentInChildren<Rigidbody>();
        _rb = rb;
    }

    void IGravity.EnableGravity()
    {
        _rb.useGravity = true;
        _rb.constraints = RigidbodyConstraints.None;
        _rb.drag = 0;
    }

    void IGravity.DisableGravity()
    {
        _rb.constraints = RigidbodyConstraints.FreezePositionY | 
            RigidbodyConstraints.FreezeRotationX | 
            RigidbodyConstraints.FreezeRotationZ;

        _rb.useGravity = false;
        _rb.drag = 1;
    }
#endregion

}