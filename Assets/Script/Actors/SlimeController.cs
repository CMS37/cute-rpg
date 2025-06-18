using UnityEngine;
using Game.System;

namespace Game.Actors
{
    public class SlimeController : MonsterBase
    {
        protected override void Awake()
        {
            base.Awake();
            StateMachine = new StateMachine();
            IdleState = new IdleState(this);
            MoveState = new MoveState(this);
            AttackState = new AttackState(this);
            HitState = new HitState(this);
            DeadState = new DeadState(this);
        }

        protected override void Start()
        {
            base.Start();
            StateMachine.ChangeState(IdleState);
        }

    }
}