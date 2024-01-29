using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Bombsmanager : MonoBehaviour
{
    [SerializeField] float time = 0;
    [SerializeField] float timeRespawn = 0;
    [SerializeField] GameObject BombPrefab;
    [SerializeField] Collider2D collision;

    private void Start()
    {
        if (collision == null)
        {
            var obj = Instantiate(BombPrefab, transform.position, Quaternion.identity, transform);
            obj.GetComponent<Bombs>().Playerfriendly = true;
        }
    }

    private void Update()
    {
        if (collision == null || collision.GetComponent<Bombs>() == null)
        {
            time += Time.deltaTime;
            if (time >= timeRespawn)
            {
                var obj = Instantiate(BombPrefab, transform.position, Quaternion.identity, transform);
                obj.GetComponent<Bombs>().Playerfriendly = true;
                time = 0;
            }
        }
        else
        {
            time = 0;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        this.collision = collision;
    }
}
