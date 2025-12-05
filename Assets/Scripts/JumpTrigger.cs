using System.Collections;
using System.Threading;
using UnityEngine;

public class JumpTrigger : MonoBehaviour
{
    private PlayerJump playerJump;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnTriggerExit(Collider other)
    {
        {
            playerJump = other.GetComponent<PlayerJump>();
            if (playerJump != null)
            {
                playerJump.Jump();
            }
        }
    }
}
