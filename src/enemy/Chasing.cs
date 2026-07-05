using System;
using Godot;


public partial class Chasing : State
{
	
	public EnemyMain _enemy;

	public VisionCone _visionCone;

	public Node2D _player;
	private bool _playerDetected = true;


	[Signal] public delegate void ChasingSignalEventHandler();

	public override void Enter()
	{
		GD.Print("Currently in Chasing State");
		_visionCone.SetDetectedColor(true);
		EmitSignal(SignalName.ChasingSignal);
	}

	public override void Ready()
	{
		_enemy = GetParent<EnemyMain>();
		_visionCone = GetNode<VisionCone>("../VisionCone");
		_player = GetTree().GetFirstNodeInGroup("Player") as Node2D;
	}

    public override void PhysicsUpdate(double delta)
    {
		var _lookAngle = (_player.GlobalPosition - _enemy.GlobalPosition).Angle();
		_enemy.GlobalRotation = Mathf.LerpAngle(_enemy.GlobalRotation, _lookAngle, _enemy.BankingSpeed * (float)delta);
		_enemy.GlobalPosition = _enemy.GlobalPosition.MoveToward(_player.GlobalPosition, _enemy.ChasingSpeed * (float)delta);
		_enemy.MoveAndSlide();
		
		
		
        _playerDetected = _enemy.PlayerDetector();

		if(_playerDetected == false)
		{
			_enemy._playerLastSeenPos = _player.GlobalPosition;
			enemyFSM.TransitionTo("Searching");
		} 
    }

	
}
