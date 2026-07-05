using System.IO;
using Godot;

public partial class Returning : State
{

    public EnemyMain _enemy;
    public VisionCone _visionCone;

    public Path2D _enemyPath;
    private float _closestOffset;
    private Vector2 _targetPosition;


    [Signal] public delegate void ReturningSignalEventHandler();

	public override void Enter()
	{

		GD.Print("Currently in Returning State");
		_visionCone.SetDetectedColor(false);
		EmitSignal(SignalName.ReturningSignal);

        var _enemyLocalPosition = _enemyPath.ToLocal(_enemy.GlobalPosition);
        _closestOffset = _enemyPath.Curve.GetClosestOffset(_enemyLocalPosition); //finds the closest _enemy.PathFollow.Progress this point is
        _targetPosition =   _enemyPath.Curve.SampleBaked(_closestOffset); //converts the above to an actual point
        
	}

	public override void Ready()
	{
		_enemy = GetParent<EnemyMain>();
		_visionCone = GetNode<VisionCone>("../VisionCone");
        _enemyPath = _enemy.PathFollow.GetParent<Path2D>();
	}

    public override void PhysicsUpdate(double delta)
    {
        if(_enemy.PlayerDetector())
        {
            enemyFSM.TransitionTo("Chasing");
        }
        _enemy.GlobalPosition = _enemy.GlobalPosition.MoveToward(_targetPosition, _enemy.PatrollingSpeed * (float)delta);
        var _lookAngle = (_targetPosition - _enemy.GlobalPosition).Angle();
		_enemy.GlobalRotation = Mathf.LerpAngle(_enemy.GlobalRotation, _lookAngle, _enemy.BankingSpeed * (float)delta);

        if(_enemy.GlobalPosition == _targetPosition)
        {
            _enemy.PathFollow.Progress = _closestOffset;
            enemyFSM.TransitionTo("Patrolling");
        }
    }
	
}