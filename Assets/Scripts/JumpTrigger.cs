using System.Collections.Generic;
using UnityEngine;

public class JumpTrigger : MonoBehaviour
{
    private List<PlayerJump> playersJump = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnTriggerEnter(Collider other)
    {
        var playerEntering = other.GetComponent<PlayerJump>();
        playersJump.Add(playerEntering);
        playerEntering.OnJumped += AutoJumpOnPlayerJump;
    }

    private void AutoJumpOnPlayerJump(PlayerJump player)
    {
        player.Jump();
        playersJump.Remove(player);
        player.OnJumped -= AutoJumpOnPlayerJump;
    }

    private void OnTriggerExit(Collider other)
    {
        var playerExiting = other.GetComponent<PlayerJump>();
        if (playersJump.Contains(playerExiting))
        {
            playerExiting.Jump();
            playersJump.Remove(playerExiting);
            playerExiting.OnJumped -= AutoJumpOnPlayerJump;
        }
    }
}
