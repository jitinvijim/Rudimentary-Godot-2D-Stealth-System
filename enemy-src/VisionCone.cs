using Godot;

public partial class VisionCone : Area2D
{
	[Export] public Color DetectedColor { get; set; } = new Color(1, 0, 0, 0.4f);
	[Export] public Color UndetectedColor { get; set; } = new Color(1, 1, 0, 0.4f);

	private Polygon2D _visual;

	public override void _Ready()
	{
		_visual = GetNode<Polygon2D>("VisionConeVisual");
		var collisionShape = GetNode<CollisionPolygon2D>("VisionConeCollisionShape");
		_visual.Polygon = collisionShape.Polygon;
		_visual.Color = UndetectedColor;
	}

	public void SetDetectedColor(bool detected)
	{
		_visual.Color = detected ? DetectedColor : UndetectedColor;
	}
}
