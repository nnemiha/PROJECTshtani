using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour {
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private const string IS_RUNNING = "IsRunning";

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        animator.SetBool(IS_RUNNING, Player.Instance.IsRunning());
        AdjustPlayerFacingDirection();
    }

    private void AdjustPlayerFacingDirection()
    {
        Vector2 movementDirection = Player.Instance.GetLastMovementDirection();

        if (movementDirection.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (movementDirection.x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }
}