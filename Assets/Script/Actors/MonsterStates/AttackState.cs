using UnityEngine;
using Game.Interfaces;
using Game.Actors;

public class AttackState : IState
{
    private MonsterController monster;
    private float attackTimer;

    public AttackState(MonsterController monster)
    {
        this.monster = monster;
    }

    public void Enter()
    {
        attackTimer = 0f;
        // monster.animator.SetTrigger("Attack");
        monster.Attack();
    }

    public void Update()
    {
        attackTimer += Time.deltaTime;

        // 공격 사거리 밖이면 즉시 MoveState로 전환
        if (!monster.InAttackRange())
        {
            monster.StateMachine.ChangeState(monster.MoveState);
            return;
        }

        // 쿨타임이 끝나면 다시 공격
        if (attackTimer >= monster.AttackCooldown)
        {
            monster.StateMachine.ChangeState(monster.AttackState);
        }
    }

    public void Exit() { }
}