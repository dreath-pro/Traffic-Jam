using Godot;
using System;

public partial class BackSpawn : Node2D
{
    [Export] public PackedScene VehicleScene { get; set; }
    [Export] public PackedScene TruckScene { get; set; }

    private Marker2D[] spawnPoints;
    private RandomNumberGenerator rng = new();
    private int selectedSpawn;

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
        SpawnVehicle();

        selectedSpawn = rng.RandiRange(0, 10);
        if (selectedSpawn == 0)
            SpawnTruck();
        else
            SpawnVehicle();
    }

    private void SpawnTruck()
    {
        if (TruckScene == null)
            return;

        Node2D truck = TruckScene.Instantiate<Node2D>();
        GetTree().CurrentScene.AddChild(truck);

        Marker2D point = spawnPoints[rng.RandiRange(0, spawnPoints.Length - 1)];
        truck.GlobalPosition = point.GlobalPosition;
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
