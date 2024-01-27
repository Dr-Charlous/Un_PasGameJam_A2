using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bombs : MonoBehaviour
{
    public bool Playerfriendly = false;
    [SerializeField] float power;
    [SerializeField] CircleCollider2D triggerCollider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var inventory = collision.GetComponent<PlayerInventory>();

        if (inventory != null)
        {
            if (Playerfriendly && inventory.BombNumber < inventory.BombNumberMax)
            {
                inventory.BombNumber++;
                Destroy(this.gameObject);
            }
            else if (!Playerfriendly)
            {
                triggerCollider.radius = 2;
                Vector3 direction = collision.transform.position - transform.position;
                collision.transform.GetComponentInParent<Rigidbody2D>().AddForce(direction * power, ForceMode2D.Impulse);
                Destroy(this.gameObject);
            }
        }
    }
}
