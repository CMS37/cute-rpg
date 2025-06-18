using Game.Actors;
using Game.Interfaces;

public class HitState : IState
{
    private MonsterController monster;
    private float hitTimer;
    private float hitDuration = 0.3f;

    public HitState(MonsterController monster)
    {
        this.monster = monster;
    }

    public void Enter()
    {
        hitTimer = 0f;
        monster.animator.SetTrigger("Hit");
    }

    public void Update()
    {
        hitTimer += UnityEngine.Time.deltaTime;
        if (hitTimer >= hitDuration)
        {
            if (monster.isDead)
            {
                monster.StateMachine.ChangeState(monster.DeadState);
            }
            else if (monster.InAttackRange())
            {
                monster.StateMachine.ChangeState(monster.AttackState);
            }
            else if (monster.CanSeePlayer())
            {
                monster.StateMachine.ChangeState(monster.MoveState);
            }
            else
            {
                monster.StateMachine.ChangeState(monster.IdleState);
            }
        }
    }

    public void Exit() { }
}