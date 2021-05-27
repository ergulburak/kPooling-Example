using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Kontrol Tuşları")]
    public KeyCode upKeyCode;
    public KeyCode downKeyCode;
    public KeyCode rightKeyCode;
    public KeyCode leftKeyCode;
    public KeyCode jumpKeyCode;
    public KeyCode fireKeyCode;
    [Header("Karakter Özellikleri")]
    public float moveSpeed = 5f;

    public float jumpPower = 50f;

    private Rigidbody _rb;
    private bool _isGrounded = false;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKey(upKeyCode))
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }

        if (Input.GetKey(downKeyCode))
        {
            transform.position += -transform.forward * moveSpeed * Time.deltaTime;
        }

        if (Input.GetKey(rightKeyCode))
        {
            transform.position += transform.right * moveSpeed * Time.deltaTime;
        }

        if (Input.GetKey(leftKeyCode))
        {
            transform.position += -transform.right * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKeyDown(jumpKeyCode)&&_isGrounded)
        {
            _rb.AddForce(Vector3.up*jumpPower,ForceMode.Impulse);
        }

        if (Input.GetKey(fireKeyCode))
        {
            WeaponSystem.instance.Shoot();
        }

        RaycastHit hit;
        if (Camera.main is { })
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 relativePos = hit.point - transform.position;
                Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
                transform.rotation = new Quaternion(0, rotation.y, 0, rotation.w);
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Floor")) _isGrounded = true;
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Floor")) _isGrounded = false;
    }
}
