using Godot;
using System;
using System.Diagnostics;

public partial class FrontSpawn : Node2D
{
    [Export] public PackedScene CopScene { get; set; }

    private Marker2D[] spawnPoints;
    private RandomNumberGenerator rng = new();
    private int rngSelected;
    
    public override void _Ready()
    {
        spawnPoints = new Marker2D[]
        {
            GetNode<Marker2D>("SpawnPoint1"),
            GetNode<Marker2D>("SpawnPoint2"),
            GetNode<Marker2D>("SpawnPoint3")
        };

        GetNode<Timer>("SpawnTimer").Timeout += SpawnAll;
    }

    private void SpawnAll()
    {
        rngSelected = rng.RandiRange(0, 50);
        if (rngSelected == 0)
        {
            SpawnCop();
        }
    }

    private void SpawnCop()
    {
        if (CopScene == null)
            return;

        Marker2D point = spawnPoints[rng.RandiRange(0, spawnPoints.Length - 1)];

        Cops cop = CopScene.Instantiate<Cops>();
        GetNode<Node2D>("/root/Game/Actors").AddChild(cop);
        cop.GlobalPosition = point.GlobalPosition;
    }
}
