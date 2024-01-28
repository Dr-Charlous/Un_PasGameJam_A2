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

    private void Start()
    {
        GetComponentInParent<PlayerController>()._animator.runtimeAnimatorController = GetComponentInParent<PlayerController>()._animatorControllerSkinny;
        textBomb.text = $"Bombs : {BombNumber} / {BombNumberMax}";
    }

    public void PutBomb()
    {
        if (BombNumber > 0)
        {
            Instantiate(BombPrefab, SpawnBombPoint.position, Quaternion.identity);
            BombNumber--;
            BombText();

            if (BombNumber <= 0)
                GetComponentInParent<PlayerController>()._animator.runtimeAnimatorController = GetComponentInParent<PlayerController>()._animatorControllerSkinny;
        }
    }

    public void BombText()
    {
        textBomb.text = $"Bombs : {BombNumber} / {BombNumberMax}";
    }
}
