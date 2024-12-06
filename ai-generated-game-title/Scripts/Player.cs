using Godot;
using System;

public partial class Player : CharacterBody2D
{
	
	[Export] private int speed = 500;
	[Export] private int dashSpeed = 1500;
	bool canDash = true;
	bool dashing = false;
	
	[Export] private int MAX_HEALTH = 20;
	[Export] private int health;
	
	ProgressBar HealthBar;
	Timer DashTimer;
	Timer DashCooldown;
	AnimatedSprite2D moveanims;
	
	public override void _Ready()
	{
		InitHealth();
		DashTimer = GetNode<Timer>("DashTimer") as Timer;
		DashCooldown = GetNode<Timer>("DashCooldown") as Timer;
		moveanims = GetNode<AnimatedSprite2D>("MovementAnimations") as AnimatedSprite2D;
	}

	public void GetInput()
	{
		
		Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		
		if(inputDir[0] < 0){
			moveanims.SetAnimation("left");
		}
		else if (inputDir[0] > 0){
			moveanims.SetAnimation("right");
		}
		else if (inputDir[0] == 0 && inputDir[1] > 0){
			moveanims.SetAnimation("down");
		}
		else if (inputDir[0] == 0 && inputDir[1] < 0){
			moveanims.SetAnimation("up");
		}
		else{
			moveanims.SetAnimation("default");
		}
		
		if(Input.IsActionJustPressed("dash") && canDash){
			dashing = true;
			canDash = false;
			DashTimer.Start();
			DashCooldown.Start();
		}
		
		if(Input.IsActionJustPressed("damageplayerjustbecause")){
			Damage(5);
		}
		
		if(Input.IsActionJustPressed("ui_cancel")){
			GetTree().Quit();
		}
		
		if(dashing){
			Velocity = inputDir * dashSpeed;
		} else {
			Velocity = inputDir * speed;
		}
	}
	
	public void InitHealth(){
		health = MAX_HEALTH;
		HealthBar = GetNode<ProgressBar>("CanvasLayer/HealthBar") as ProgressBar;
		HealthBar.MaxValue = health;
		SetHealth();
	}
	
	public void SetHealth(){
		HealthBar.Value = health;
	}
	
	public void Damage(int amount){
		health -= amount;
		if(health <= 0){
			Die();
		}
		SetHealth();
	}
	
	public void Die(){
		GetTree().ChangeSceneToFile("res://Scenes/DeathScreen.tscn");
	}
	
	public void _on_dash_timer_timeout(){
		dashing = false;
	}
	
	public void _on_dash_cooldown_timeout(){
		canDash = true;
	}

	public override void _PhysicsProcess(double delta)
	{
		GetInput();
		MoveAndSlide();
	}
}
