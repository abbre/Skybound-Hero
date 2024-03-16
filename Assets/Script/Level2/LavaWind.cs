using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaWind : MonoBehaviour
{
    [SerializeField] private float inWindDeltaSpeed = 0.5f;
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
            transform.Translate(Vector2.up * inWindDeltaSpeed * Time.deltaTime);
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
