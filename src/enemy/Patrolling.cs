using System;
using Godot;

public partial class Patrolling : State
{
	public EnemyMain _enemy;
	public VisionCone _visionCone;
	public RayCast2D _los;

	private float _sweepAngle = 0.0f;
	private int _lookDirection = 1; //1 if sweep is moving positive (down and right), -1 if moving negative (up and left)

	private float SweepAngleRad => Mathf.DegToRad(_enemy.PatrolSweepAngleDegrees); //kind of an inline thing. when _enemy is initialized, SweepAngleRad should also be initialized as such

	private bool _playerDetected = false;
	[Signal] public delegate void PatrollingSignalEventHandler(); //can to be removed, this is just for label debug on game screen

	public override void Enter()
	{
		
		GD.Print("Currently in Patrolling State");
		_visionCone.SetDetectedColor(false);
		EmitSignal(SignalName.PatrollingSignal); //can to be removed, this is just for label debug on game screen
	}
	public override void Ready()
	{
		_enemy = GetParent<EnemyMain>();
		_visionCone = GetNode<VisionCone>("../VisionCone");
		_los = GetNode<RayCast2D>("../LOS");

	}
	public override void PhysicsUpdate(double delta)
	{
		if(_enemy.PatrolSweep)
		{
			SweepRotation((float)delta); //write SweepRotation()
		}

		_enemy.PathFollow.Progress += _enemy.PatrollingSpeed * (float)delta;
		_enemy.GlobalPosition = _enemy.PathFollow.GlobalPosition;
		_enemy.Rotation = Mathf.LerpAngle(_enemy.Rotation,
										  _enemy.PathFollow.Rotation + _sweepAngle, 
										  _enemy.BankingSpeed * (float)delta);


		_playerDetected = _enemy.PlayerDetector();

		if(_playerDetected == true)
		{
			enemyFSM.TransitionTo("Chasing");
		} 

	}

	public void SweepRotation(float delta)
	{
		_sweepAngle += _enemy.PatrolSweepRate * delta * _lookDirection;
		if(_sweepAngle >= SweepAngleRad)
		{
			_lookDirection = -1;
		}
		else if(_sweepAngle <= -SweepAngleRad)
		{
			_lookDirection = 1;
		}
	}

}
