using Godot;
using System.Collections;
using System;
using GodotExt.Maths;
using GodotExt.Engine;

using Vector3 = GodotExt.Maths.Vector3;

public class Instruction
{
    public float thrust;
    public float heading;
    
    public Instruction(float thrustPercentage, float heading)
    {
        this.thrust = thrustPercentage;
        this.heading = heading;
    }

    public Instruction()
    {
        this.thrust = Maths.RandomFloat(0,100);
        this.heading = Maths.RandomFloat(-References.rocketRotateSpeed,References.rocketRotateSpeed);
    }

    public static Instruction zero
    {
        get
		{
			return new Instruction(0,0);
		}
    }
}