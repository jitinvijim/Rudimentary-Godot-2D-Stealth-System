using Godot;

[Tool]
public partial class EnemyStealth : Node
{
	public Enemy _enemy;

	[ExportCategory("Sweep")]
	private bool _sweep = false;
	[Export] public bool Sweep 
	{
		get => _sweep;
		set
		{
			_sweep = value;
			SweepAngleDegrees = value ? 45f : 0f;
			SweepSpeed = value ? 1.0f : 0f;
			NotifyPropertyListChanged();
		}
	}
	[ExportGroup("Sweep Settings")]
	[Export] public float SweepAngleDegrees { get; set; } = 0f;
	[Export] public float SweepSpeed { get; set; } = 0.0f;

	[Signal] public delegate void PlayerDetectedEventHandler();
	[Signal] public delegate void PlayerUndetectedEventHandler();

	private int _lookDirection = 1;
	private float _sweepAngle = 0f;

	private bool _patrolling = true;
	private bool _playerDetected = false;

	private VisionCone _visionCone;
	private RayCast2D _los;

	public override void _Ready()
	{
		if (Engine.IsEditorHint()) return;

		_enemy = GetParent<Enemy>();
		_visionCone = GetNode<VisionCone>("../VisionCone");
		_los = GetNode<RayCast2D>("LOS");
		_sweepAngle = Mathf.DegToRad(SweepAngleDegrees);
	}

	public override void _Process(double delta)
	{
		if (Engine.IsEditorHint()) return;

		if (Sweep)
			SweepRotation((float)delta);

		if (_patrolling)
		{
			_enemy.PathFollow.Progress += _enemy.Speed * (float)delta;
			_enemy.GlobalPosition = _enemy.PathFollow.GlobalPosition;
			_enemy.Rotation = Mathf.LerpAngle(_enemy.Rotation, _enemy.PathFollow.Rotation + _sweepAngle, _enemy.bankingSpeed * (float)delta);
		}
		PlayerDetector();
	}

	private float SweepAngleRad => Mathf.DegToRad(SweepAngleDegrees);

	public void SweepRotation(float delta)
	{
		_sweepAngle += SweepSpeed * delta * _lookDirection;
		if (_sweepAngle >= SweepAngleRad)
			_lookDirection = -1;
		else if (_sweepAngle <= -SweepAngleRad)
			_lookDirection = 1;
	}

	public void PlayerDetector()
	{
		var bodies = _visionCone.GetOverlappingBodies();
		bool playerInCone = false;

		foreach (var body in bodies)
		{
			if (body is Player)
			{
				playerInCone = true;
				break;
			}
		}

		if (playerInCone)
		{
			var player = GetTree().GetFirstNodeInGroup("Player") as Node2D;
			_los.TargetPosition = _los.ToLocal(player.GlobalPosition);
			_los.ForceRaycastUpdate();

			if (!_los.IsColliding() || _los.GetCollider() is Player)
			{
				if (!_playerDetected)
				{
					_playerDetected = true;
					_patrolling = false;
					_visionCone.SetDetectedColor(true);
					EmitSignal(SignalName.PlayerDetected);
				}
			}
		}
		else if (_playerDetected)
		{
			_playerDetected = false;
			_patrolling = true;
			_visionCone.SetDetectedColor(false);
			EmitSignal(SignalName.PlayerUndetected);
		}
	}
}