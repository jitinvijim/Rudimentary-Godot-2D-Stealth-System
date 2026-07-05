using Godot;

public partial class PlayerLastSeenViz : Sprite2D
{
    public Main _main;

    public override void _Ready()
    {
        _main = GetParent<Main>();
        Visible = false;
    }


    public override void _PhysicsProcess(double delta)
    {
        if(_main._playerLastSeenPos != null)
        {
            GlobalPosition = _main._playerLastSeenPos.Value;
            Visible = true;
        }
        else
        {
            Visible = false;
        }
        
    }

}
