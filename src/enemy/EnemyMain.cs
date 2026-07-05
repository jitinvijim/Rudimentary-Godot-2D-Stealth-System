using System.Dynamic;
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


	[ExportCategory("Patrol Sweep")]
	private bool _sweep = false;
	[Export] public bool PatrolSweep
	{
		get => _sweep;
		set
		{
			_sweep = value;
			PatrolSweepAngleDegrees = value ? 45f : 0f;
			PatrolSweepRate = value ? 2.0f : 0f;
			NotifyPropertyListChanged();
		}
	}
	[ExportGroup("Sweep Settings")]
	[Export] public float PatrolSweepAngleDegrees {get; set;} = 330.0f;
	[Export] public float PatrolSweepRate {get; set;} = 2.0f;

	[ExportCategory("Search Sweep")]
	[Export] public float SearchSweepAngleDegrees { get; set; } = 300.0f;
	[Export] public float SearchSweepRate {get; set;} = 10.0f;



	//Stealth System Signals
	[Signal] public delegate void PatrollingSignalEventHandler();
	[Signal] public delegate void ChasingSignalEventHandler();
	[Signal] public delegate void SearchingSignalEventHandler();
	[Signal] public delegate void ReturningSignalEventHandler();

	private VisionCone _visionCone;
	private RayCast2D _los;

	public Vector2 ? _playerLastSeenPos;

	
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
		var patrolling = GetNode<Patrolling>("Patrolling");
        var chasing = GetNode<Chasing>("Chasing");
		var searching = GetNode<Searching>("Searching");
		var returning = GetNode<Returning>("Returning");

		patrolling.PatrollingSignal += () => EmitSignal(SignalName.PatrollingSignal);
		chasing.ChasingSignal += () => EmitSignal(SignalName.ChasingSignal);
		searching.SearchingSignal += () => EmitSignal(SignalName.SearchingSignal);
		returning.ReturningSignal += () => EmitSignal(SignalName.ReturningSignal);
		
    }	
}

	