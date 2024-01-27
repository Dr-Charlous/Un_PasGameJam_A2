using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubbles : MonoBehaviour
{
    [SerializeField] float power;
    [SerializeField] bool persistant;

    private void Update()
    {
        if (!persistant)
        {
            Vector3 position = transform.position;
            transform.position = new Vector3(position.x, position.y+(1*Time.deltaTime), position.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector3 direction = collision.transform.position - transform.position;
        collision.transform.GetComponentInParent<Rigidbody2D>().AddForce(direction * power, ForceMode2D.Impulse);

        if (!persistant)
            Destroy(this.gameObject);
    }
}
