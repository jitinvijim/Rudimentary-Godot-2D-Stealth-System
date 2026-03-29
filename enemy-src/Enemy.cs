using Godot;


public partial class Enemy : CharacterBody2D
{
	[ExportCategory("Movement")]
	[Export] public PathFollow2D PathFollow { get; set; }
	[Export] public float Speed { get; set; } = 200.0f;
	[Export] public float bankingSpeed { get; set; } = 2.0f;

	//Stealth System Signals
	[Signal] public delegate void PlayerDetectedEventHandler();
	[Signal] public delegate void PlayerUndetectedEventHandler();

    public override void _Ready()
    {
		//Stealth System Signal Connections
        var stealth = GetNode<EnemyStealth>("EnemyStealth");
		stealth.PlayerDetected += () => EmitSignal(SignalName.PlayerDetected);
		stealth.PlayerUndetected += () => EmitSignal(SignalName.PlayerUndetected);
    }

	

}

	