using Godot;
using System;
using System.Threading.Tasks.Dataflow;

public partial class CargoTruck : CharacterBody2D
{
    private Timer timer;
    private Vector2 previousPosition;
    public override void _PhysicsProcess(double delta)
    {
        Velocity = Vector2.Left * 100;
        MoveAndSlide();

        if (GlobalPosition.X < -783)
            QueueFree();
    }

    public override void _Ready()
    {
        timer = GetNode<Timer>("StuckTimer");

        timer.Timeout += onStuckTimerTimeout;
    }

    private void onStuckTimerTimeout()
    {
        float distanceMoved = previousPosition.DistanceTo(GlobalPosition);

        if (distanceMoved < 5f)
        {
            GD.Print("Truck is stationary");
            QueueFree();
        }else
        {
            previousPosition = GlobalPosition;
        }

        //if (GlobalPosition.X > 1157)
        //{
        //    GD.Print("Might be stuck");
        //    QueueFree();
        //}
    }
}
