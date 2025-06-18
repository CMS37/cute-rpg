using Game.Actors;
using Game.Interfaces;
using UnityEngine;

public class MoveState : IState
{
    private MonsterController monster;

    public MoveState(MonsterController monster) { this.monster = monster; }

    public void Enter()
    {
        monster.animator.SetFloat("Speed", monster.MoveSpeed);
    }

    public void Update()
    {
        monster.ChasePlayer();

        if (!monster.CanSeePlayer())
            monster.StateMachine.ChangeState(monster.IdleState);

        if (monster.InAttackRange())
            monster.StateMachine.ChangeState(monster.AttackState);
    }

    public void Exit() { }
}
