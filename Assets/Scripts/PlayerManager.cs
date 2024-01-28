using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] GameObject CanvaCamera;
    [SerializeField] GameObject SpawnParticule;
    [SerializeField] Transform startingPoints;
    PlayerInputManager playerInputManager;
    public List<PlayerInput> Players = new List<PlayerInput>();
    public List<Transform> LdSpawnPoint;
    public List<GameObject> Ld;
    public int LdId = 0;

    private void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
    }

    private void Start()
    {
        for (int i = 1; i < Ld.Count; i++)
        {
            Ld[i].SetActive(false);
        }
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
        startingPoints = LdSpawnPoint[LdId];

        Players.Add(player);
        player.transform.position = startingPoints.position;
        player.GetComponent<PlayerController>().playerManager = this;
        Instantiate(SpawnParticule, player.transform.position, Quaternion.identity);
        
        if (CanvaCamera.activeInHierarchy && Players.Count > 0)
            CanvaCamera.SetActive(false);
    }

    public void NextLevel()
    {
        Ld[LdId].SetActive(false);
        if (LdId < LdSpawnPoint.Count-1)
            LdId++;
        else 
            LdId = 0;
        Ld[LdId].SetActive(true);

        for (int i = 0; i < Players.Count; i++)
        {
            Players[i].transform.position = LdSpawnPoint[LdId].position;
        }
    }
}
