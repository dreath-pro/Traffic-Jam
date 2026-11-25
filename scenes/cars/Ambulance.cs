using Godot;
using System;

public partial class Ambulance : Car
{
    private RandomNumberGenerator randomNumber = new RandomNumberGenerator();
    [Export] public PackedScene HeartScene;
    private Marker2D heartSpawn;
    private Timer dropTimer;
    private int dropCount = 2;

    public override void _PhysicsProcess(double delta)
    {
        MoveAndSlide();

        base._PhysicsProcess(delta);

        if (GlobalPosition.X > 2000)
        {
            QueueFree();
        }
    }

    public override void _Ready()
    {
        base._Ready();

        Timer decision = GetNode<Timer>("Decision");
        decision.Timeout += Decision;
        Decision();

        Timer steady = GetNode<Timer>("Steady");
        steady.Timeout += Steady;
        Steady();

        heartSpawn = GetNodeOrNull<Marker2D>("HeartSpawn");
        if (heartSpawn == null)
        {
            GD.PrintErr("Your heartspawn marker is null");
            return;
        }

        dropTimer = GetNodeOrNull<Timer>("DropTimer");
        if (dropTimer == null)
        {
            GD.PrintErr("Your drop timer is null");
            return;
        }else
        {
            dropTimer.Timeout += OnDropTimeout;
        }
    }

    private void OnDropTimeout()
    {
        DropHeart();
        dropCount--;

        if (dropCount > 0)
        {
            dropTimer.Start();
        }
    }

    private void DropHeart()
    {
        var heart = HeartScene.Instantiate<HeartFloat>();
        GetTree().CurrentScene.AddChild(heart);
        //GetNode<Node2D>("/root/Game/Actors").AddChild(heart);
        heart.GlobalPosition = heartSpawn.GlobalPosition;
    }

    private void Steady()
    {
        MoveRight();
    }

    private void Decision()
    {
        int choice = randomNumber.RandiRange(0, 2);

        switch (choice)
        {
            case 0:
                MoveRight();
                break;
            case 1:
                MoveRightUp();
                break;
            case 2:
                MoveRightDown();
                break;
        }
    }

    private void MoveRight()
    {
        Velocity = Vector2.Right * 150;
    }

    private void MoveRightUp()
    {
        Velocity = new Vector2(150, -100);
    }

    private void MoveRightDown()
    {
        Velocity = new Vector2(150, 100);
    }
}
