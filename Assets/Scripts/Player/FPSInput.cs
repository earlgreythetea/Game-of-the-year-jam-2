using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Control Script/FPS Input")]
public class FPSInput: MonoBehaviour
{
    [SerializeField] Animator _animator;
    private CharacterController _charController;

    //Стандартная скорость игрока
    public float moveSpeedInitial = 1f;
    //Во сколько раз увеличится скорость при ускорении
    public float speedAccelerationRate = 1.5f;
    //Сила прыжка игрока
    public float jumpSpeed = 15.0f; 
    public float gravity = -9.8f; 
    public float terminalVelocity = -10.0f; 
    public float minFall = -1.5f;

    private float _vertSpeed;
    private float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        _charController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        moveSpeed = moveSpeedInitial;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = moveSpeedInitial * speedAccelerationRate;
        }
        if (_charController.isGrounded) 
        {
            _animator.SetBool("NotGrounded", false);
            if (Input.GetButtonDown("Jump")) 
            { 
                _vertSpeed = jumpSpeed; 
            } 
            else 
            { 
                _vertSpeed = minFall; 
            } 
        } 
        else 
        {
            _animator.SetBool("NotGrounded", true);
            _vertSpeed += gravity * 5 * Time.deltaTime; 
            if (_vertSpeed < terminalVelocity) 
            { 
                _vertSpeed = terminalVelocity; 
            } 
        }

        if (!_animator.GetBool("IsActionPerforming"))
        {
            float deltaX = Input.GetAxis("Horizontal") * moveSpeed;
            float deltaZ = Input.GetAxis("Vertical") * moveSpeed;

            Vector3 movement = new Vector3(deltaX, 0, deltaZ);
            movement.x = deltaX * moveSpeed;
            movement.z = deltaZ * moveSpeed;
            _animator.SetFloat("Speed", movement.sqrMagnitude);
            movement = Vector3.ClampMagnitude(movement, moveSpeed);
            //movement.y = gravity;
            movement.y = _vertSpeed;

            movement *= Time.deltaTime;
            movement = transform.TransformDirection(movement);
            _charController.Move(movement);
        }
    }
}
