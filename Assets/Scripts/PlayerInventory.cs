using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] GameObject BombPrefab;
    [SerializeField] Transform SpawnBombPoint;
    public int BombNumberMax = 5;
    public int BombNumber = 0;

    public void PutBomb()
    {
        if (BombNumber > 0)
        {
            Instantiate(BombPrefab, SpawnBombPoint.position, Quaternion.identity);
            BombNumber--;
        }
    }
}
