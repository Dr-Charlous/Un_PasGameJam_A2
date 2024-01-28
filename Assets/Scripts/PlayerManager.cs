using System.Collections;
using System.Collections.Generic;
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
        player.transform.position = startingPoints.position;
        player.GetComponent<PlayerController>().playerManager = this;
        Instantiate(SpawnParticule, player.transform.position, Quaternion.identity);
        
        if (CanvaCamera.activeInHierarchy && Players.Count > 0)
            CanvaCamera.SetActive(false);
    }

    public void NextLevel()
    {
        Destroy(Ld);
        if (LdId < LdPrefab.Count-1)
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
