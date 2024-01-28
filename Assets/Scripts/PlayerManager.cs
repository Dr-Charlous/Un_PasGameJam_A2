using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] GameObject CanvaCamera;
    [SerializeField] GameObject SpawnParticule;
    public Transform startingPoints;
    PlayerInputManager playerInputManager;
    public List<PlayerInput> Players = new List<PlayerInput>();
    public List<GameObject> LdPrefab;
    public GameObject Ld;
    public int LdId = 0;
    [SerializeField] Color[] colors;

    private void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
        CanvaCamera.SetActive(true);

        Ld = Instantiate(LdPrefab[LdId]);
        Ld.GetComponentInChildren<End>().playerManager = this;
        startingPoints = Ld.GetComponentInChildren<SpawnPoint>().transform;
    }

    private void OnEnable()
    {
        playerInputManager.onPlayerJoined += AddPlayer;
    }

    private void OnDisable()
    {
        playerInputManager.onPlayerJoined -= AddPlayer;
    }

    public void AddPlayer(PlayerInput player)
    {
        Players.Add(player);

        RandomColorPlayer(player);

        player.transform.position = startingPoints.position;
        player.GetComponent<PlayerController>().playerManager = this;

        Instantiate(SpawnParticule, player.transform.position, Quaternion.identity);

        if (CanvaCamera.activeInHierarchy && Players.Count > 0)
            CanvaCamera.SetActive(false);
    }

    void RandomColorPlayer(PlayerInput player)
    {
        int numbLoop = 0;
        int rnd = Boucle(Mathf.Abs(Random.Range(0, colors.Length)), numbLoop);

        player.gameObject.GetComponent<PlayerVisual>().ColorInt = rnd;
        player.gameObject.GetComponent<PlayerVisual>().ColorPlayer = colors[rnd];
        player.gameObject.GetComponent<PlayerVisual>().PlayerRenderer.color = colors[rnd];
    }

    private int Boucle(int rnd, int numbLoop)
    {
        if (numbLoop < colors.Length)
        {
            bool isUsed = false;

            for (int i = 0; i < Players.Count; i++)
            {
                if (Players[i].GetComponent<PlayerVisual>().ColorInt == rnd)
                    isUsed = true;
            }

            if (!isUsed)
                return rnd;
            else
                return Boucle(Mathf.Abs(Random.Range(0, colors.Length)), numbLoop++);
        }
        else
        {
            return rnd;
        }
    }

    public void NextLevel()
    {
        Destroy(Ld);
        if (LdId < LdPrefab.Count - 1)
            LdId++;
        else
            LdId = 0;

        Ld = Instantiate(LdPrefab[LdId]);
        Ld.GetComponentInChildren<End>().playerManager = this;
        startingPoints = Ld.GetComponentInChildren<SpawnPoint>().transform;

        for (int i = 0; i < Players.Count; i++)
        {
            Players[i].transform.position = startingPoints.position;
        }
    }
}
