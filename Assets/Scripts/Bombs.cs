using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bombs : MonoBehaviour
{
    public bool Playerfriendly = false;
    [SerializeField] float power;
    [SerializeField] float radius = 2;
    [SerializeField] CircleCollider2D triggerCollider;
    [SerializeField] GameObject ExplosionFX;
    [SerializeField] GameObject EatFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var inventory = collision.GetComponent<PlayerInventory>();

        if (inventory != null)
        {
            if (Playerfriendly && inventory.BombNumber < inventory.BombNumberMax)
            {
                inventory.BombNumber++;
                inventory.BombText();
                collision.transform.GetComponentInParent<PlayerController>().AnimEat();
                Instantiate(EatFX, transform.position, Quaternion.identity);
                collision.transform.GetComponentInParent<PlayerVisual>()._animator.runtimeAnimatorController = collision.transform.GetComponentInParent<PlayerVisual>()._animatorControllerFat;
                Destroy(this.gameObject);
            }
            else if (!Playerfriendly)
            {
                Explode(collision);
            }
        }
    }

    private void Explode(Collider2D collision)
    {
        triggerCollider.radius = radius;

        Vector3 direction = collision.transform.position - transform.position;
        collision.transform.GetComponentInParent<Rigidbody2D>().velocity = Vector3.zero;
        collision.transform.GetComponentInParent<Rigidbody2D>().AddForce(direction * power, ForceMode2D.Impulse);
        Instantiate(ExplosionFX, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
