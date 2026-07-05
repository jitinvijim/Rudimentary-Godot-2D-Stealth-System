using Godot;
using System;

public partial class Searching : State
{

	public EnemyMain _enemy;

    public VisionCone _visionCone;

    private Vector2  ? _playerLastSeenPos;

    private float _searchSweepAngleRad;
    private int _sweepDirection;
    private float _originalRotationAngle;
    private bool _originalAngleInitialized;

    [Signal] public delegate void SearchingSignalEventHandler();


    public override void Enter()
    {
        GD.Print("Currently in Searching State");
        _enemy = GetParent<EnemyMain>();
        _playerLastSeenPos = _enemy._playerLastSeenPos;
        _sweepDirection = 1;
        _originalAngleInitialized = false;
        _visionCone.SetDetectedColor(false);
        EmitSignal(SignalName.SearchingSignal);
        
    }

    public override void Ready()
    {
        _enemy = GetParent<EnemyMain>();
        _visionCone = GetNode<VisionCone>("../VisionCone");
        _searchSweepAngleRad = _enemy.SearchSweepAngleDegrees * (float)(Math.PI / 180.0);
    }


    public override void PhysicsUpdate(double delta)
    {
        if(_enemy.GlobalPosition != _playerLastSeenPos)
        {
            _enemy.GlobalPosition = _enemy.GlobalPosition.MoveToward(_playerLastSeenPos.Value, _enemy.PatrollingSpeed * (float)delta);
            var _lookAngle = (_enemy._playerLastSeenPos.Value - _enemy.GlobalPosition).Angle();
		    _enemy.GlobalRotation = Mathf.LerpAngle(_enemy.GlobalRotation, _lookAngle, _enemy.BankingSpeed * (float)delta);
        }
        else
        {
            if(!_originalAngleInitialized)
            {
                _originalRotationAngle = _enemy.Rotation;
                _originalAngleInitialized = true;

            }
            if(!Search((float)delta))
            {
                enemyFSM.TransitionTo("Returning");
            }
        }
    }

    public bool Search(float delta)
    {

        _enemy.RotationDegrees += _enemy.SearchSweepRate * delta * _sweepDirection; 
        if(_enemy.PlayerDetector())
        {
            enemyFSM.TransitionTo("Chasing");
            return true;
        }
        if(_enemy.Rotation >= _originalRotationAngle + _searchSweepAngleRad)
        {
            _sweepDirection = -1; //positive sweep to negative sweep
        } 
        else if(_enemy.Rotation <= _originalRotationAngle) //negative sweep
        {
            return false;
        }

        return true;
    }

    public override void Exit()
    {
        _enemy._playerLastSeenPos = null;
    }

}
