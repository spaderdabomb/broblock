using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimation : MonoBehaviour
{
    private SpawnedEnemy spawnedEnemy;
    private Animator animator;

    private void Awake()
    {
        spawnedEnemy = GetComponent<SpawnedEnemy>();
        animator = GetComponent<Animator>();
    }


    private void OnEnable()
    {
        spawnedEnemy.EnemyInstance.CombatStateChanged += ChangeCombatAnimation;
    }

    private void OnDisable()
    {
        spawnedEnemy.EnemyInstance.CombatStateChanged -= ChangeCombatAnimation;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    // private void GetAnimationController

    private void ChangeCombatAnimation(EnemyCombatState newCombatState)
    {
        if (newCombatState == EnemyCombatState.Patrolling)
        {
            animator.SetTrigger(EnemyAnimationNames.Patrolling);
        }
        else if (newCombatState == EnemyCombatState.Idling)
        {
            animator.SetTrigger(EnemyAnimationNames.Idling);
        }
    }

    public static class EnemyAnimationNames
    {
        public static string Idling = "transitionToIdling";
        public static string Patrolling = "transitionToPatrolling";
        public static string PlayerFollowing = "transitionToPlayerFollowing";

    }
}
