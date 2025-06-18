using UnityEngine;
using Game.System;

namespace Game.Actors
{
    public class MushroomController : MonsterBase
    {
        [SerializeField] protected float skillMultiplier = 2.0f;
        [SerializeField] private GameObject gasCloudPrefab;
        
        protected override void Awake()
        {
            base.Awake();
            StateMachine = new StateMachine();
            IdleState = new IdleState(this);
            MoveState = new MoveState(this);
            AttackState = new AttackState(this);
            HitState = new HitState(this);
            DeadState = new DeadState(this);
			SkillState = new SkillState(this);
        }

        protected override void Start()
        {
            base.Start();
            StateMachine.ChangeState(IdleState);
        }

        public override void UseSkill()
        {
            Debug.Log($"머쉬룸이 독 구름을 생성! 스킬 데미지: {skillMultiplier * stats.attack.Current}");
        }

        public void SpawnGasCloud()
        {
            Instantiate(gasCloudPrefab, transform.position, Quaternion.identity);
        }
    }
}