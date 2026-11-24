using Godot;
using System;
using System.Threading.Tasks.Dataflow;

public partial class CargoTruck : Car
{
    private Timer stuckTimer;
    private Vector2 previousPosition;

    public override void _PhysicsProcess(double delta)
    {
        Velocity = Vector2.Left * 100;
        MoveAndSlide();

        base._PhysicsProcess(delta);

        if (GlobalPosition.X < -783)
            QueueFree();
    }

    public override void _Ready()
    {
        base._Ready();

        stuckTimer = GetNode<Timer>("StuckTimer");
        stuckTimer.Timeout += onStuckTimerTimeout;
    }

    private void onStuckTimerTimeout()
    {
        float distanceMoved = previousPosition.DistanceTo(GlobalPosition);

        if (distanceMoved < 5f)
        {
            QueueFree();
        }
        else
        {
            previousPosition = GlobalPosition;
        }
    }
}
