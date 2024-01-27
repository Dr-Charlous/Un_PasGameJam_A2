using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubbles : MonoBehaviour
{
    [SerializeField] float power;
    public BubbleManager manager;
    public bool persistant;

    private void Update()
    {
        if (!persistant)
        {
            Vector3 position = transform.position;
            transform.position = new Vector3(position.x, position.y + (1 * Time.deltaTime), position.z);

            if (transform.position.y >= manager.point3.y || transform.position.y >= manager.point4.y)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() != null)
        {
            Vector3 direction = collision.transform.position - transform.position;
            collision.transform.GetComponentInParent<Rigidbody2D>().velocity = Vector3.zero;
            collision.transform.GetComponentInParent<Rigidbody2D>().AddForce(direction * power, ForceMode2D.Impulse);

            if (!persistant)
                Destroy(this.gameObject);
        }
    }
}
