using Godot;
using System;

public partial class Game : Node2D
{
    private int seconds = 0, minutes = 0, hours = 0;

    private Player player;
    private Timer timeCount;
    private String timeText;
    private Button restartButton, menuButton, menuButton2, pauseButton, resumeButton;

    public override void _Process(double delta)
    {

    }

    public override void _Ready()
    {
        timeCount = GetNode<Timer>("%TimeCount");
        timeCount.Timeout += DisplayTime;

        player = GetNode<Player>("%Player");
        GetNode<Label>("%HealthLabel").Text = "x" + player.health;

        player.GameOver += OnGameOver;

        restartButton = GetNodeOrNull<Button>("%RestartButton");
        restartButton.Pressed += OnRestartPressed;

        menuButton = GetNodeOrNull<Button>("%MenuButton");
        menuButton.Pressed += OnMenuPressed;

        pauseButton = GetNodeOrNull<Button>("%PauseButton");
        pauseButton.Pressed += OnPausePressed;

        resumeButton = GetNodeOrNull<Button>("%ResumeButton");
        resumeButton.Pressed += OnResumeButton;

        menuButton2 = GetNodeOrNull<Button>("%MenuButton2");
        menuButton2.Pressed += OnMenuPressed;

        if (restartButton == null)
            GD.PrintErr("Your restart button is null");
        if (menuButton == null)
            GD.PrintErr("Your menu button is null");
        if (pauseButton == null)
            GD.PrintErr("Your pause button is null");
        if (resumeButton == null)
            GD.PrintErr("Your resume button is null");
        if (menuButton2 == null)
            GD.PrintErr("Your menu button 2 is null");

        GetNode<CanvasLayer>("GameOverHUD").ProcessMode = Node.ProcessModeEnum.WhenPaused;
        GetNode<CanvasLayer>("PauseHUD").ProcessMode = Node.ProcessModeEnum.WhenPaused;
    }

    private void OnGameOver()
    {
        timeCount.Stop();
        GetTree().Paused = true;
        GetNode<CanvasLayer>("GameOverHUD").Visible = true;
        GetNode<Label>("%SurviveTimeLabel").Text = "Survive Time: " + timeText;
        GetNode<Label>("%CopsEscapedLabel").Text = "Escaped from cops: " + Cops.totalEscaped;
    }

    private void OnResumeButton()
    {
        timeCount.Start();
        GetTree().Paused = false;
        GetNode<CanvasLayer>("PauseHUD").Visible = false;
    }

    private void OnPausePressed()
    {
        timeCount.Stop();
        GetTree().Paused = true;
        GetNode<CanvasLayer>("PauseHUD").Visible = true;
    }

    private void OnRestartPressed()
    {
        Cops.totalEscaped = 0;
        GetTree().Paused = false;
        GetTree().ReloadCurrentScene();
    }

    private void OnMenuPressed()
    {
        GetTree().Paused = false;
        GetTree().ChangeSceneToFile("res://scenes/layout/homescreen.tscn");
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

        this.timeText = timeText;
        GetNode<Label>("%TimeLabel").Text = this.timeText;
    }
}
