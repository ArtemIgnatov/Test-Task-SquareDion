using UnityEngine;

[RequireComponent (typeof(Animator), typeof(Rigidbody))]
public class PlayerAnimationController : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody _rb;

    private void OnEnable()
    {
        ShootingController.OnShoot += HandleShoot;
    }

    private void OnDisable()
    {
        ShootingController.OnShoot -= HandleShoot;
    }

    void Start()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        _animator.SetFloat("Velocity", _rb.velocity.magnitude);  
    }

    void HandleShoot(bool shoot)
    {
        _animator.SetBool("Shooting", shoot);
    }
}
