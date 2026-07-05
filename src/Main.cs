using Godot;
using System;

public partial class Main : Node2D
{
	private Label _statusLabel;

	//For subscribing to Enemy's signals. REQUIRED
	private EnemyMain _enemy;

	public Vector2? _playerLastSeenPos;

	public override void _Ready()
	{
		_statusLabel = GetNode<Label>("StateCheck");

		//Subscribing to Enemy's signals. REQUIRED
		_enemy = GetNode<EnemyMain>("Enemy");
		_enemy.PatrollingSignal += OnPatrollingSignal;
		_enemy.ChasingSignal += OnChasingSignal;
		_enemy.SearchingSignal += OnSearchingSignal;
		_enemy.ReturningSignal += OnReturningSignal;
	}

    public override void _Process(double delta)
    {
        _playerLastSeenPos = _enemy._playerLastSeenPos;

		if(_enemy._playerLastSeenPos == null) 
		{
			_playerLastSeenPos = null;
		}
    }


	public void OnPatrollingSignal()
	{
		_statusLabel.Text = "Patrolling";
	}

	public void OnChasingSignal()
	{
		_statusLabel.Text = "Chasing";
	}

	public void OnSearchingSignal()
	{
		_statusLabel.Text = "Searching";
	}
	
	public void OnReturningSignal()
	{
		_statusLabel.Text = "Returning";
	}

	private void OnResetButtonPressed()
	{
    	GetTree().ReloadCurrentScene();
	}
}


