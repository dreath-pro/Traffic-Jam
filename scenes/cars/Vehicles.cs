using Godot;
using System;

public partial class Vehicles : Car
{
    private RandomNumberGenerator randomNumber = new RandomNumberGenerator();

    public override void _PhysicsProcess(double delta)
    {
        MoveAndSlide();

        base._PhysicsProcess(delta);

        if (GlobalPosition.X < -389)
            QueueFree();
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
        MoveLeft();
    }

    private void Decision()
    {
        int choice = randomNumber.RandiRange(0, 3);

        switch (choice)
        {
            case 0:
                MoveLeft();
                break;
            case 1:
                MoveRight();
                break;
            case 2:
                MoveLeftUp();
                break;
            case 3:
                MoveLeftDown();
                break;
        }
    }

    private void MoveRight()
    {
        Velocity = Vector2.Right * 380;
    }

    private void MoveLeft()
    {
        Velocity = Vector2.Left * 200;
    }

    private void MoveLeftUp()
    {
        Velocity = new Vector2(-200, -100);
    }

    private void MoveLeftDown()
    {
        Velocity = new Vector2(-200, 100);
    }
}
