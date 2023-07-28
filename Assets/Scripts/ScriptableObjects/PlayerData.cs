using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PlayerData", menuName = "BroBlock/PlayerData")]
public class PlayerData : ScriptableObject
{
    [SerializeField] public float healthBase = 100f;
    [SerializeField] public float throwPowerBase = 10f;
    [SerializeField] public float throwPowerIncreaseRate = 2.5f;

    // Health
    [SerializeField, ReadOnly] private float healthCurrent = 100f;
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

    // Throw power
    [SerializeField, ReadOnly] private float throwPowerCurrent;
    public float ThrowPowerCurrent
    {
        get { return throwPowerCurrent; }
        set
        {
            if (throwPowerCurrent != value)
            {
                throwPowerCurrent = Mathf.Max(value, 0);
                throwPowerCurrent = Mathf.Min(throwPowerCurrent, throwPowerBase);
                ThrowPowerChanged?.Invoke(throwPowerCurrent);
            }
        }
    }

    // Move state
    [SerializeField, ReadOnly] private PlayerMovement.PlayerMoveStates currentMoveState;
    public PlayerMovement.PlayerMoveStates CurrentMoveState
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

    public UnityAction<float> HealthChanged;
    public UnityAction<float> ThrowPowerChanged;
    public UnityAction<PlayerMovement.PlayerMoveStates> MoveStateChanged;

    public void InitDefaults()
    {
        healthCurrent = healthBase;
        throwPowerCurrent = throwPowerBase;
        currentMoveState = PlayerMovement.PlayerMoveStates.Idling;
    }
}
