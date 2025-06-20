using Game.Interfaces;

namespace Game.System
{
	public class StateMachine
	{
		private IState currentState;

		public void ChangeState(IState newState)
		{
			currentState?.Exit();
			currentState = newState;
			currentState?.Enter();
		}

		public void Update()
		{
			currentState?.Update();
		}

		public void FixedUpdate()
		{
			currentState?.FixedUpdate();
		}
	}
}
