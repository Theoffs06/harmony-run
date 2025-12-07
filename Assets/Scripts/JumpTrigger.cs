using System.Collections.Generic;
using Player;
using UnityEngine;

public class JumpTrigger : MonoBehaviour {
    private readonly List<PlayerJump> _players = new();

    private void OnTriggerEnter(Collider other) {
        if (!other.TryGetComponent<PlayerJump>(out var playerEntering)) return;
        
        _players.Add(playerEntering);
        playerEntering.OnJumped += AutoJumpOnPlayerJump;
    }
    
    private void OnTriggerExit(Collider other) {
        if (!other.TryGetComponent<PlayerJump>(out var playerExiting)) return;
        if (!_players.Contains(playerExiting)) return;
        playerExiting.Jump();
            
        playerExiting.OnJumped -= AutoJumpOnPlayerJump;
        _players.Remove(playerExiting);
    }
    
    private void AutoJumpOnPlayerJump(PlayerJump player) {
        player.Jump();
        
        player.OnJumped -= AutoJumpOnPlayerJump;
        _players.Remove(player);
    }
}
