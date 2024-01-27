using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] GameObject BombPrefab;
    [SerializeField] Transform SpawnBombPoint;
    [SerializeField] TextMeshProUGUI textBomb;
    public int BombNumberMax = 5;
    public int BombNumber = 0;

    public void PutBomb()
    {
        if (BombNumber > 0)
        {
            Instantiate(BombPrefab, SpawnBombPoint.position, Quaternion.identity);
            BombNumber--;
            BombText();
        }
    }

    public void BombText()
    {
        textBomb.text = $"Bombs : {BombNumber} / {BombNumberMax}";
    }
}
