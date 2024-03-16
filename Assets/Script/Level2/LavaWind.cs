using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaWind : MonoBehaviour
{
    [SerializeField] private float _windForce = 0.5f;
    private Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("LavaWind"))
        {
            _rb.gravityScale = 0f;
            _rb.AddForce(transform.up * _windForce, ForceMode2D.Force);
        }

        if (other.CompareTag("LavaWindDown"))
        {
            Debug.Log("down");
            _rb.AddForce(-1 * transform.up * _windForce, ForceMode2D.Force);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("LavaWind"))
        {
            _rb.gravityScale = 1f;
        }
    }
}