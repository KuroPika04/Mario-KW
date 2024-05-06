using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private BoxCollider2D BoxCollider;
    [SerializeField] private float speed;
    private bool hit;
    private float direction;
    private float life_time;
    private Rigidbody2D body;

    private void Awake()
    {
        BoxCollider = GetComponent<BoxCollider2D>();
        body = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        body.velocity = transform.right * speed;
        life_time = 0f;
    }
    private void Update()
    {
        // projectile lifetime
        life_time += Time.deltaTime;
        if (life_time > 1f)
        {
            gameObject.SetActive(false);
            life_time = 0f;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("Destroy objext");
            Destroy(gameObject);
        }
    }
/*    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Destroy objext");
            Destroy(gameObject);
        }
    }*/

}
