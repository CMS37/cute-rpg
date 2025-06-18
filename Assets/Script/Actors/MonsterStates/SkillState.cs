using UnityEngine;
using Game.Interfaces;
using Game.Actors;

public class SkillState : IState
{
    private MonsterBase monster;
    private float skillAnimTime = 1.0f;
    private float timer = 0f;

    public SkillState(MonsterBase monster) { this.monster = monster; }

    public void Enter()
    {
		monster.animator.SetTrigger("UseSkill");
        monster.UseSkill();
        timer = 0f;
    }

    public void Update()
    {
        timer += Time.deltaTime;
        if (timer >= skillAnimTime)
        {
            monster.StateMachine.ChangeState(monster.IdleState);
        }
    }

    public void FixedUpdate() {}

    public void Exit() { }
}