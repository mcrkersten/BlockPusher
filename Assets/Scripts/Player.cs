using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class Player : MonoBehaviour, IGravity
{
    IGravity _Igravity;

    private bool _gameOver;
    public float _acceleration;

    public Rigidbody _rb { set; get; }

    /// <summary>
    /// Called when game ends, false = died, true = finished 
    /// </summary>
    public static event OnGameOver _onGameOver;
    public delegate void OnGameOver(bool finished);

    /// <summary>
    /// Called when powerop-gameObject collides with player
    /// </summary>
    public static event OnPowerUp _powerUp;
    public delegate void OnPowerUp();

    /// <summary>
    /// Called when collectable-gameObject collides with player
    /// </summary>
    public static event OnCollectable _onCollectable;
    public delegate void OnCollectable(float amount);

    private ControlActions _controlActions;
    private InputAction _movementInputAction;

    private void Awake()
    {
        _Igravity = gameObject.GetComponent<IGravity>();
        _Igravity.GetRigidBody();

        InitControls();
    }

    private void InitControls()
    {
        _controlActions = new ControlActions();
        _controlActions.Player.Movement.Enable();
        _movementInputAction = _controlActions.Player.Movement;
    }

    private void FixedUpdate()
    {
        if (_gameOver) return;
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        Vector2 movementInput = _movementInputAction.ReadValue<Vector2>();
        Vector3 force = Vector3.zero;
        force[0] = movementInput.x;
        force[2] = movementInput.y;
        _rb.AddForce(force * _acceleration);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PowerUp"))
            OnPowerUpHit(collision.gameObject);

        if (collision.gameObject.CompareTag("Collectable"))
            OnCollectableHit(collision.gameObject);

        if (collision.gameObject.CompareTag("Hazardous"))
            OnHazardousHit();
    }

    private void OnPowerUpHit(GameObject hitObject)
    {
        _powerUp?.Invoke();
        Destroy(hitObject);
    }

    private void OnCollectableHit(GameObject hitObject)
    {
        float amount = hitObject.GetComponent<Collectable>().amount;
        _onCollectable?.Invoke(amount);
        Destroy(hitObject);
    }

    private void OnHazardousHit()
    {
        SetGameOver(false);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("GravityTrigger"))
            _Igravity.EnableGravity();

        if (collider.CompareTag("Hazardous"))
            SetGameOver(false);

        if (collider.CompareTag("Finishline"))
            SetGameOver(true);
    }

    private void SetGameOver(bool finished)
    {
        if(!finished)
            _Igravity.EnableGravity();

        _gameOver = true;
        _onGameOver?.Invoke(finished);
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
        _rb.constraints = RigidbodyConstraints.FreezeRotation |
            RigidbodyConstraints.FreezePositionY;

        _rb.useGravity = false;
        _rb.drag = 1;
    }
#endregion

}
