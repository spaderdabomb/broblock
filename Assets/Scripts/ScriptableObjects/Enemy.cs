using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Enemy", menuName = "BroBlock/Enemy", order = 1)]
public class Enemy : ScriptableObject
{
    public string type;
    public float health;
    public float damageForceThreshold;
    public bool canWalk;
    public bool canSprint;
    public bool canJump;
    public float walkSpeed;
    public float sprintSpeed;
    public float jumpSpeed;

    public float stateChangeTimerMin = 5f;
    public float stateChangeTimerMax = 10f;
    public EnemyCombatState[] enemyCombatStates;

    // Health
    [SerializeField, ReadOnly] private float healthCurrent;
    public float HealthCurrent
    {
        get { return healthCurrent; }
        set
        {
            if (healthCurrent != value)
            {
                healthCurrent = Mathf.Max(value, 0);
                HealthChanged?.Invoke(healthCurrent);
            }
        }
    }

    // Move state
    [SerializeField, ReadOnly] private EnemyMoveState currentMoveState;
    public EnemyMoveState CurrentMoveState
    {
        get { return currentMoveState; }
        set
        {
            if (currentMoveState != value)
            {
                currentMoveState = value;
                MoveStateChanged?.Invoke(currentMoveState);
            }
        }
    }

    // Combat state
    [SerializeField, ReadOnly] private EnemyCombatState currentCombatState;
    public EnemyCombatState CurrentCombatState
    {
        get { return currentCombatState; }
        set
        {
            if (currentCombatState != value)
            {
                currentCombatState = value;
                CombatStateChanged?.Invoke(currentCombatState);
            }
        }
    }

    public UnityAction<float> HealthChanged;
    public UnityAction<EnemyMoveState> MoveStateChanged;
    public UnityAction<EnemyCombatState> CombatStateChanged;

    public void InitDefaults()
    {
        healthCurrent = health;
        CurrentMoveState = EnemyMoveState.None;
        CurrentCombatState = EnemyCombatState.None;
    }
}


[Flags]
public enum EnemyMoveState
{
    None,
    Idling,
    Walking,
    Sprinting,
    Jumping,
    Attacking
}

public enum EnemyCombatState
{
    None,
    Idling,
    Patrolling,
    PlayerFollow
}