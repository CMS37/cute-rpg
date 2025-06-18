using Game.Actors;
using Game.Interfaces;
using UnityEngine;

public class IdleState : IState
{
    private MonsterBase monster;

    public IdleState(MonsterBase monster) { this.monster = monster; }

    public void Enter()
    {
        monster.animator.SetFloat("Speed", 0);
    }

    public void Update()
    {
        if (monster.CanSeePlayer())
            monster.StateMachine.ChangeState(monster.MoveState);
    }

    public void Exit() { }
}