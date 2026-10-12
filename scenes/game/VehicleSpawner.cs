using Godot;
using System;

public partial class VehicleSpawner : Node2D
{
    [Export] public PackedScene VehicleScene { get; set; }

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

        GetNode<Timer>("SpawnTimer").Timeout += SpawnVehicle;
    }

    private void SpawnVehicle()
    {
        if (VehicleScene == null)
            return;

        Marker2D point = spawnPoints[rng.RandiRange(0, spawnPoints.Length - 1)];

        Vehicles vehicle = VehicleScene.Instantiate<Vehicles>();
        GetTree().CurrentScene.AddChild(vehicle);
        vehicle.GlobalPosition = point.GlobalPosition;
    }
}
