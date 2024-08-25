using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cobweb : MonoBehaviour
{
    public Vector3 direction;
    [SerializeField] private float moveSpeed = 35f;
    private Collider2D hitbox;
    void Update()
    {
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            hitbox = GetComponent<Collider2D>();
            collider.GetComponent<Stats>().TakeDamage(7, false, hitbox);
        }
        if (collider.CompareTag("Terrain"))
        {
            Destroy(gameObject);
        }
    }
}
