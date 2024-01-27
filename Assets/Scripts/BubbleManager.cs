using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleManager : MonoBehaviour
{
    [SerializeField] GameObject BubblePrefab;
    [SerializeField] float spawnSpeed = 5;
    public Vector2 point1, point2, point3, point4;
    float time = 0;

    private void Update()
    {
        time++;

        if (time >= spawnSpeed * 60)
        {
            float rnd = Random.Range(point1.x, point2.x);

            var bubble = Instantiate(BubblePrefab, new Vector2(rnd, point1.y), Quaternion.identity);
            bubble.GetComponent<Bubbles>().persistant = false;
            bubble.GetComponent<Bubbles>().manager = this;

            time = 0;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(point1, point2);
        Gizmos.DrawLine(point3, point4);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(point1, 0.1f);
        Gizmos.DrawSphere(point2, 0.1f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(point3, 0.1f);
        Gizmos.DrawSphere(point4, 0.1f);
    }
}
