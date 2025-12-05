using UnityEngine;

public class JumpTrigger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerExit(Collider other)
    {
        {
            var player_jump = other.GetComponent<PlayerJump>();
            if (player_jump != null)
            {
                player_jump.Jump();
            }
        }
    }
}
