using Godot;
using System;

public partial class CopSpawner : Node2D
{
    [Export] public PackedScene CopScene { get; set; }

    private Marker2D[] spawnPoints;
    private RandomNumberGenerator rng = new();
    
    public override void _Ready()
    {
        spawnPoints = new Marker2D[]
        {
            GetNode<Marker2D>("SpawnPoint1"),
            GetNode<Marker2D>("SpawnPoint2"),
            GetNode<Marker2D>("SpawnPoint3")
        };

        GetNode<Timer>("SpawnTimer").Timeout += SpawnCop;
    }

    private void SpawnCop()
    {
        if (CopScene == null)
            return;

        Marker2D point = spawnPoints[rng.RandiRange(0, spawnPoints.Length - 1)];

        Cops cop = CopScene.Instantiate<Cops>();
        GetTree().CurrentScene.AddChild(cop);
        cop.GlobalPosition = point.GlobalPosition;
    }
}
