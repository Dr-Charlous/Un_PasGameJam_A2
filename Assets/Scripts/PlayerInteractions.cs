using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() != null)
        {
            Vector3 direction = collision.transform.position - transform.position;
            collision.transform.GetComponent<Rigidbody2D>().AddForce(direction*GetComponentInParent<Rigidbody2D>().velocity.magnitude, ForceMode2D.Impulse);
        }
    }
}
