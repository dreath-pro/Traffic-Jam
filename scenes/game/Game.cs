using Godot;
using System;

public partial class Game : Node2D
{
    private int seconds = 0, minutes = 0, hours = 0;
    private Player player;
    private Timer timeCount;

    public override void _Process(double delta)
    {

    }

    public override void _Ready()
    {
        timeCount = GetNode<Timer>("%TimeCount");
        timeCount.Timeout += DisplayTime;

        player = GetNode<Player>("Player");
        GetNode<Label>("%HealthLabel").Text = "x" + player.health;

        player.GameOver += OnGameOver;
        GetNode<Button>("%RestartButton").Pressed += OnRestartPressed;
        GetNode<CanvasLayer>("GameOverHUD").ProcessMode = Node.ProcessModeEnum.WhenPaused;
    }

    private void OnGameOver()
    {
        timeCount.Stop();
        GetTree().Paused = true;
        GetNode<CanvasLayer>("GameOverHUD").Visible = true;
    }

    private void OnRestartPressed()
    {
        GD.Print("Restart button pressed");
        GetTree().Paused = false;
        GetTree().ReloadCurrentScene();
    }

    private void DisplayTime()
    {
        String timeText = "";
        seconds++;

        if (seconds > 59)
        {
            minutes++;
            seconds = 0;
        }
        if (minutes > 59)
        {
            hours++;
            minutes = 0;
            seconds = 0;
        }

        if (hours > 0)
        {
            if (hours < 10)
            {
                timeText = "0" + hours + ":";
            }
            else
            {
                timeText = hours + ":";
            }
        }

        if (minutes < 10)
        {
            if (hours == 0)
            {
                timeText += minutes + ":";
            }else
            {
                timeText += "0" + minutes + ":";
            }
        }
        else
        {
            timeText += minutes + ":";
        }

        if (seconds < 10)
        {
            timeText += "0" + seconds;
        }
        else
        {
            timeText += seconds;
        }

        GetNode<Label>("%TimeLabel").Text = timeText;
    }
}
