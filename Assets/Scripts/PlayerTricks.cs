using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class PlayerTricks : MonoBehaviour {

    [Header("Inputs")]
    [SerializeField] private InputActionReference lauchTrick;

    private Animator animator;
    private PlayerJump playerJump;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerJump = GetComponent<PlayerJump>();
    }

    private void Update() {
        if (!playerJump.IsGrounded())
        {
            if (lauchTrick.action.WasPressedThisFrame())
            {
                animator.Play("Kickflip");
            }
        }
        else { 
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Kickflip"))
            {
                Debug.Log("Looser");
            }
        }
    }

}
