using Godot;


public partial class Chasing : State
{
	
	public EnemyMain _enemy;

	public VisionCone _visionCone;

	private bool _playerDetected = true;

	[Signal] public delegate void PlayerDetectedEventHandler();

	public override void Enter()
	{

		GD.Print("Currently in Chasing State");
		_visionCone.SetDetectedColor(true);
		EmitSignal(SignalName.PlayerDetected);
		//probably a signal saying PlayerDetected
	}

	public override void Ready()
	{
		_enemy = GetParent<EnemyMain>();
		_visionCone = GetNode<VisionCone>("../VisionCone");
	}

    public override void PhysicsUpdate(double delta)
    {
        _playerDetected = _enemy.PlayerDetector();

		if(_playerDetected == false)
		{
			enemyFSM.TransitionTo("Patrolling");
		} 
    }

	
}
