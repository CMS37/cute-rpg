using UnityEngine;
using Game.Interfaces;
using Game.Actors;

public class AttackState : IState
{
    private MonsterBase monster;
    private float attackTimer;

    public AttackState(MonsterBase monster)
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

        if (monster.SkillState != null &&
            monster.SkillTimer >= monster.SkillCooldown &&
            monster.InAttackRange())
        {
            monster.SkillTimer = 0f;
            monster.StateMachine.ChangeState(monster.SkillState);
            return;
        }

        if (!monster.InAttackRange())
        {
            monster.StateMachine.ChangeState(monster.MoveState);
            return;
        }

        if (attackTimer >= monster.AttackCooldown)
        {
            monster.StateMachine.ChangeState(monster.AttackState);
        }
    }

    public void FixedUpdate() {}
    public void Exit() { }
}