using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    private PlayerData playerData;
    private Animator animator;
    Dictionary<PlayerMovement.PlayerMoveStates, AnimatorTriggers> moveStateToTriggerDict = new();

    private void Awake()
    {
        playerData = GameManager.Instance.playerData;
        animator = GetComponent<Animator>();

        moveStateToTriggerDict.Add(PlayerMovement.PlayerMoveStates.Idling, AnimatorTriggers.triggerIdling);
        moveStateToTriggerDict.Add(PlayerMovement.PlayerMoveStates.Running, AnimatorTriggers.triggerRunning);
        moveStateToTriggerDict.Add(PlayerMovement.PlayerMoveStates.Jumping, AnimatorTriggers.triggerJumping);
    }

    private void OnEnable()
    {
        playerData.MoveStateChanged += ChangeMoveAnimation;
    }

    private void OnDisable()
    {
        playerData.MoveStateChanged -= ChangeMoveAnimation;
    }

    private void ChangeMoveAnimation(PlayerMovement.PlayerMoveStates newMovementState)
    {
        animator.SetTrigger(moveStateToTriggerDict[newMovementState].ToString());
    }

    private enum AnimatorTriggers
    {
        triggerIdling,
        triggerRunning,
        triggerJumping,
        triggerDying
    }
}
