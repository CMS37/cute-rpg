using UnityEngine;
using Game.Interfaces;
using Game.Actors;

public class DeadState : IState
{
    private MonsterBase monster;
    private float deathTimer = 0f;
    private float deathDuration = 1.0f;

    public DeadState(MonsterBase monster)
    {
        this.monster = monster;
    }

    public void Enter()
    {
        monster.animator.SetTrigger("Die");
        deathTimer = 0f;
    }

    public void Update()
    {
        deathTimer += Time.deltaTime;
        if (deathTimer >= deathDuration)
        {
            Object.Destroy(monster.gameObject);
        }
    }
    
    public void FixedUpdate() {}

    public void Exit() { }
}
