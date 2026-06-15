using Godot;
using System;

public partial class EnemyMovement : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

// Enemy movement should probably be a state machine. with states patrol, chase, search and return 