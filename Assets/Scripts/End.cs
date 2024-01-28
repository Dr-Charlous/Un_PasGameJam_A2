using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class End : MonoBehaviour
{
    public PlayerManager playerManager;
    public bool isLevelFinished = false;
    public bool canChange = false;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() && collision.transform.position.y > transform.position.y)
        {
            for (int i = 0; i < playerManager.Players.Count; i++)
            {
                if (playerManager.Players[i] != collision.GetComponent<PlayerInput>())
                {
                    playerManager.Players[i].GetComponent<PlayerController>().LoseUi.SetActive(true);
                }
                else
                {
                    playerManager.Players[i].GetComponent<PlayerController>().WinUi.SetActive(true);
                }
            }
            isLevelFinished = true;

            GetComponent<BoxCollider2D>().isTrigger = false;
            StartCoroutine(wait());
        }
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(2);
        canChange = true;
    }

    public void ChangeLd()
    {
        playerManager.NextLevel();
        GetComponent<BoxCollider2D>().isTrigger = true;

        for (int i = 0; i < playerManager.Players.Count; i++)
        {
            playerManager.Players[i].GetComponent<PlayerController>().LoseUi.SetActive(false);
            playerManager.Players[i].GetComponent<PlayerController>().WinUi.SetActive(false);
        }

        canChange = false;
    }
}
