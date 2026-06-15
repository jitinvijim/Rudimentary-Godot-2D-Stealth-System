using Godot;
using System;

public partial class Main : Node2D
{
	private Label _statusLabel;

	//For subscribing to Enemy's signals. REQUIRED
	private Enemy _enemy;

	public override void _Ready()
	{
		_statusLabel = GetNode<Label>("VisibilityCheck");

		//Subscribing to Enemy's signals. REQUIRED
		_enemy = GetNode<Enemy>("Enemy");
		_enemy.PlayerDetected += OnPlayerDetected;
		_enemy.PlayerUndetected += OnPlayerUndetected;
	}

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


