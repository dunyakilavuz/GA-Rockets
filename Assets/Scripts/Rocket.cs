using Godot;
using System.Collections;
using System;
using GodotExt.Maths;
using GodotExt.Engine;

using Vector3 = GodotExt.Maths.Vector3;

public class Rocket : Spatial
{
    public TransformExt transform;
    Particles exhaust;
    Target target;
    Vector3 velocity;
    Vector3 acceleration;
    Vector3 startPos;
    float currentThrust;
    float thrustPercentage; 
    float heading;
    float deltaTime;
    float maxExhaustSpeed;
    float maxExhaustTTL;
    Instruction[] instructions;
    float time;
    int index = 0;
    bool dead = false;
    float fitness;
    float timeAlive;
    bool punish;
    bool reward;

    
    public override void _Ready()
    {
        transform = new TransformExt();
        startPos = new Vector3(0,References.rocketStartHeight,0);
        transform.position = startPos;
        transform.rotation = Quaternion.identity;
        transform.scale = Vector3.one * References.rocketScale;
        exhaust =  (Particles) GetChild(1);
        velocity = Vector3.zero;
        currentThrust = 0;
        thrustPercentage = 0;
        maxExhaustSpeed = 20;
        maxExhaustTTL = 1f;

        instructions = new Instruction[References.instructionSize];

        for (int i = 0; i < References.instructionSize; i++)
        {
            instructions[i] = new Instruction();
        }
    }
    public override void _PhysicsProcess(float delta)
    {
        deltaTime = delta;
        time += deltaTime;
        
        if(time / References.instructionsPerSecond < References.instructionSize && dead == false)
        {
            index = (int)(time * References.instructionsPerSecond);
            if(index < instructions.Length)
                SetInstruction(instructions[index]);
        }
        else
        {
            SetInstruction(Instruction.zero);
            Kill();
        }

        ApplyGravity();
        Thrust();
        Heading();
        transform.position += velocity * delta;
        SetTransform(TransformExt.ToTransform(transform));
    }

    private void AddForce(Vector3 force)
    {
        acceleration = force / References.rocketMass;
        velocity += acceleration;
        acceleration = Vector3.zero;
    }

    private void ApplyGravity()
    {
        if(References.applyGravity == true)
        {
            AddForce(Vector3.down * References.gravityMagnitude);
        }
    }

    private void SetHeading(float degrees)
    {
        heading = degrees;
    }

    private void SetThrust(float percentage)
    {
        if(percentage >= 0 && percentage <= 100)
        {
            currentThrust = percentage * References.maxThrust / 100;
            thrustPercentage = percentage;
        }
    }

    private void SetInstruction(Instruction instruction)
    {
        SetThrust(instruction.thrust);
        SetHeading(instruction.heading);
    }

    private void Thrust()
    {
        if(currentThrust > 0)
        {
            AddForce(transform.up * currentThrust);
            exhaust.Emitting = true;
            exhaust.SpeedScale = thrustPercentage * maxExhaustSpeed / 100;
            exhaust.Lifetime = thrustPercentage * maxExhaustTTL / 100;
        }
        else
        {
            exhaust.Emitting = false;
        }
    }

    private void Heading()
    {
        if(heading != 0)
        {
            transform.rotation *= Quaternion.AngleAxis(heading * deltaTime, Vector3.back);
        }
    }

    public void SetTarget(Target target)
    {
        this.target = target;
    }


    public void Kill()
    {
        velocity = Vector3.zero;
        dead = true;
    }

    public void Revive()
    {
        transform.position = startPos;
        transform.rotation = Quaternion.identity;
        velocity = Vector3.zero;
        acceleration = Vector3.zero;
        dead = false;
        time = 0;
        index = 0;
        fitness = 0;
    }

    public void SetToPunish(float timeAlive)
    {
        punish = true;
        this.timeAlive = timeAlive;
    }

    public void SetToReward(float timeAlive)
    {
        reward = true;
        this.timeAlive = timeAlive;
    }
    private void Punish()
    {
        float newFitness = GetFitness() / (References.PunishRate / timeAlive);
        SetFitness(newFitness);
        punish = false;
    }

    private void Reward()
    {
        float newFitness = GetFitness() * (References.RewardRate / timeAlive);
        SetFitness(newFitness);
        reward = false;
    }
    public bool isDead()
    {
        return dead;
    }

    public float CalculateFitness()
    {
        float distanceToTarget = (target.transform.position - transform.position).magnitude;
        fitness = 1 / distanceToTarget;      // Closer to the target, higher the fitness.
        Punish();
        Reward();
        return fitness;
    }

    public float GetFitness()
    {
        return fitness;
    }

    public void SetFitness(float fitness)
    {
        this.fitness = fitness;
    }

    public Instruction[] GetGenes()
    {
        Instruction[] instructionsCloned = new Instruction[instructions.Length];
        for(int i = 0; i < instructions.Length; i++)
        {
            instructionsCloned[i] = instructions[i];
        }
        return instructionsCloned;
    }

    public void SetGenes(Instruction[] newInstructions)
    {
        for(int i = 0; i < instructions.Length; i++)
        {
            instructions[i] = newInstructions[i];
        }
    }
}