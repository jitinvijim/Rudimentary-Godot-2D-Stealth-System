using Godot;
using System;

public partial class Searching : State
{

	public EnemyMain _enemy;

    public override void Enter()
    {
        //possibly a signal saying PlayerUndetected
    }

    public override void Ready()
    {
        _enemy = GetParent<EnemyMain>();
    }


}
