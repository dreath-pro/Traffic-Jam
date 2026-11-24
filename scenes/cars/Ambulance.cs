using Godot;
using System;

public partial class Ambulance : Car
{
    private RandomNumberGenerator randomNumber = new RandomNumberGenerator();

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
