using Godot;

[Tool]
//code that has to be compiled in the editor (in this case for the inspector tab), is bracketed by [Tool] and if(Engine.isEditorHint()) return;
public partial class EnemyMain : CharacterBody2D
{
	[ExportCategory("Movement")]
	[Export] public PathFollow2D PathFollow { get; set; }
	[Export] public float PatrollingSpeed { get; set; } = 200.0f;
	[Export] public float ChasingSpeed {get; set;} = 300.0f;
	[Export] public float BankingSpeed { get; set; } = 2.0f;

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
	[Export] public float SweepAngleDegrees {get; set;}
	[Export] public float SweepSpeed {get; set;}


	//Stealth System Signals
	[Signal] public delegate void PlayerDetectedEventHandler();
	[Signal] public delegate void PlayerUndetectedEventHandler();

	private VisionCone _visionCone;
	private RayCast2D _los;
	
	public bool PlayerDetector()
	{
		var bodies = _visionCone.GetOverlappingBodies();
		bool playerInCone = false;

		foreach( var body in bodies)
		{
			if(body is Player)
			{
				playerInCone = true;
				break;
			}
		}

		if(playerInCone)
		{
			var player = GetTree().GetFirstNodeInGroup("Player") as Node2D;
			_los.TargetPosition = _los.ToLocal(player.GlobalPosition);
			_los.ForceRaycastUpdate();

			if(_los.IsColliding() && _los.GetCollider() is Player)
			{
				return true;
			}
		}
		return false;
	}

    public override void _Ready()
    {
		if (Engine.IsEditorHint()) return;

		_visionCone = GetNode<VisionCone>("VisionCone");
		_los = GetNode<RayCast2D>("LOS");

		 //Stealth System Signal Connections
        var chasing = GetNode<Chasing>("Chasing");
		var returning = GetNode<Returning>("Returning");
		chasing.PlayerDetected += () => EmitSignal(SignalName.PlayerDetected);
		returning.PlayerUndetected += () => EmitSignal(SignalName.PlayerUndetected);
		
    }

	

}

	