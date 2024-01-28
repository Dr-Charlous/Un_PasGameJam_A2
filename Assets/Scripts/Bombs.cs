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

    private void Update()
    {
        if (!Playerfriendly && GetComponent<CircleCollider2D>().radius != radius)
            GetComponent<CircleCollider2D>().radius = radius;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        var inventory = collision.GetComponent<PlayerInventory>();

        if (Playerfriendly && inventory != null && inventory.BombNumber < inventory.BombNumberMax)
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
            Collider2D[] obj = Physics2D.OverlapCircleAll(transform.position, radius);

            foreach (Collider2D obj2 in obj)
            {
                Explode(collision);

                if (obj2.GetComponent<Bombs>() != null)
                {
                    obj2.GetComponent<Bombs>().Playerfriendly = false;
                }
            }

            Instantiate(ExplosionFX, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    public void Explode(Collider2D collision)
    {
        Vector2 direction = collision.transform.position - transform.position;

        if (collision.GetComponent<PlayerInventory>())
        {
            collision.transform.GetComponentInParent<Rigidbody2D>().velocity = Vector3.zero;
            collision.transform.GetComponentInParent<Rigidbody2D>().AddForce(direction * power, ForceMode2D.Impulse);
            collision.GetComponentInParent<PlayerVisual>()._animator.SetBool("Explode", true);
            collision.transform.GetComponentInParent<PlayerController>().PlaySound(collision.transform.GetComponentInParent<PlayerController>()._explodeSound, collision.transform.GetComponentInParent<PlayerController>()._audioSource2);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
