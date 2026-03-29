using Godot;
using System;

public partial class Main : Node2D
{
	private Label _statusLabel;
	private Enemy _enemy;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_statusLabel = GetNode<Label>("VisibilityCheck");
		_enemy = GetNode<Enemy>("Enemy");
		_enemy.PlayerDetected += OnPlayerDetected;
		_enemy.PlayerUndetected += OnPlayerUndetected;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public void OnPlayerDetected()
	{
		_statusLabel.Text = "Detected!";
	}

	public void OnPlayerUndetected()
	{
		_statusLabel.Text = "Undetected!";
	}

	private void OnResetButtonPressed()
{
    GetTree().ReloadCurrentScene();
}
}


