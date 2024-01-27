using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] GameObject CanvaCamera;
    List<PlayerInput> players = new List<PlayerInput>();
    [SerializeField] Transform startingPoints;
    [SerializeField] List<LayerMask> playerLayers;
    PlayerInputManager playerInputManager;

    private void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
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
        players.Add(player);
        player.transform.position = startingPoints.position;
        
        if (CanvaCamera.activeInHierarchy && players.Count > 0)
            CanvaCamera.SetActive(false);
    }
}
