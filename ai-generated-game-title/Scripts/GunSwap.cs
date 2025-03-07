using Godot;
using System;
using System.Collections.Generic;

public partial class GunSwap : Node2D
{
	List<PackedScene> Gun_list = new List<PackedScene>();
	int Current_gun = 0;
	string Gun_name;
	Node2D currentGunInstance;
	Label EquippedGun;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Load the scenes (make sure the paths are correct)
		Gun_list.Add(ResourceLoader.Load<PackedScene>("res://Scenes/Guns/pistol.tscn"));
		Gun_list.Add(ResourceLoader.Load<PackedScene>("res://Scenes/Guns/shotgun.tscn"));
		Gun_list.Add(ResourceLoader.Load<PackedScene>("res://Scenes/Guns/sniper.tscn"));
		Gun_list.Add(ResourceLoader.Load<PackedScene>("res://Scenes/Guns/uzi.tscn"));
		
		EquippedGun = GetParent().GetNode<Label>("CanvasLayer/EquippedGun") as Label;

		// Instantiate and add the initial gun to the scene
		currentGunInstance = Gun_list[Current_gun].Instantiate<Node2D>();
		AddChild(currentGunInstance);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		switch(Current_gun)
		{
		case 0:
			Gun_name = "Pistol";
			break;
		case 1:
			Gun_name = "Shotgun";
			break;
		case 2:
			Gun_name = "Sniper";
			break;
		case 3:
			Gun_name = "Uzi";
			break;
		default:
			break;
		}
		
		EquippedGun.Text = Gun_name;
		
		if (Input.IsActionJustPressed("swap_gun"))
		{
			// Remove the current gun instance if it exists
			if (currentGunInstance != null)
			{
				currentGunInstance.QueueFree();
			}

			// Update the gun index
			Current_gun = (Current_gun + 1) % Gun_list.Count;

			// Instantiate and add the new gun to the scene
			currentGunInstance = Gun_list[Current_gun].Instantiate<Node2D>();
			AddChild(currentGunInstance);
		}
	}
}
