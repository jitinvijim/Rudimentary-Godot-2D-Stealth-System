using Godot;

public partial class Player : CharacterBody2D
{
    [Export] public float Speed { get; set; } = 500.0f;

    private Vector2 _screenSize;
    private Vector2 _halfSize;

    public override void _Ready()
    {
        _screenSize = GetViewportRect().Size;
        var sprite = GetNode<Sprite2D>("Sprite2D");
        _halfSize = sprite.Texture.GetSize() * sprite.Scale / 2;
    }
    public override void _Process(double delta)
    {
        var direction = Vector2.Zero;

        if (Input.IsActionPressed("ui_right")) direction.X += 1;
        if (Input.IsActionPressed("ui_left"))  direction.X -= 1;
        if (Input.IsActionPressed("ui_down"))  direction.Y += 1;
        if (Input.IsActionPressed("ui_up"))    direction.Y -= 1;

        if (direction != Vector2.Zero)
            direction = direction.Normalized();

        Velocity = direction * Speed;
        MoveAndSlide();
        Position = Position.Clamp(_halfSize, _screenSize - _halfSize);
    }
}