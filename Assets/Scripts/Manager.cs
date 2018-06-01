using Godot;
using System.Collections;
using System.Collections.Generic;
using System;
using GodotExt.Maths;
using GodotExt.Engine;

using Vector3 = GodotExt.Maths.Vector3;

public class Manager : Node
{
    RocketFactory rf;
    ObstacleFactory of;
    TargetFactory tf;
    Rocket[] rockets;
    Obstacle[] obstacles;
    Target target;

    bool populationDead;
    int targetHits = 0;
    int generation = 1;
    int mutationHappened = 0;

    bool stoppingCriteria = false;
    
    float time;
    float timeThisGeneration;
    float timeToRevive;

    float maxFitnessAchieved;
    float maxFitness;
    int maxFitIndex;
    float averageFitness;
    List<Instruction[]> genePool;
    List<string> fitnessHistory;
    RichTextLabel infoText;

    public override void _Ready()
    {
        rf = (RocketFactory)GetChild(0);
        of = (ObstacleFactory)GetChild(1);
        tf = (TargetFactory)GetChild(2);

        rockets = rf.createPopulation(References.populationSize);
        obstacles = of.createPopulation(References.obstaclePopulationSize);
        target = tf.createTarget();
        populationDead = false;
        time = 0;
        maxFitnessAchieved = 0;

        for(int i = 0; i < rockets.Length; i++)
        {
            rockets[i].SetTarget(target);
        }

        genePool = new List<Instruction[]>();
        fitnessHistory = new List<string>();

        infoText = (RichTextLabel)GetNode("/root/MainScene/UI/InfoText");
    } 

    public override void _Process(float delta)
    {
        time += delta;
        timeThisGeneration += delta;
        infoText.Text = InfoString();
        HandleCollisions();
        CheckVitals();
        CheckStoppingCriteria();
        
        if(Input.IsActionJustPressed("ui_accept"))
        {
            GetResults();
        }
        if(generation == References.getResultsOnGenerations)
        {
            GetResults();
        }
    }

    private void GetResults()
    {
        for(int i = 0; i < fitnessHistory.Count; i++)
        {
            System.IO.File.WriteAllLines(@"results.txt", fitnessHistory);
        }

        GD.Print("Results written to file.");
    }

    private void HandleCollisions()
    {
        for(int i = 0; i < rockets.Length; i++)
        {
            Vector3 rocketPos = rockets[i].transform.position;
            bool kill = false;

            for (int j = 0; j < obstacles.Length; j++)
            {
                Vector3 obstaclePos = obstacles[j].transform.position;
                float obstacleSize = obstacles[j].transform.scale.x;

                if((rocketPos - obstaclePos).magnitude < obstacleSize && rockets[i].isDead() == false)
                {
                    rockets[i].SetToPunish(timeThisGeneration);
                    kill = true;
                }
            }

            Vector3 targetPos = target.transform.position;
            float targetSize = target.transform.scale.x;

            if((rocketPos - targetPos).magnitude < targetSize && rockets[i].isDead() == false)
            {
                rockets[i].SetToReward(timeThisGeneration);
                kill = true;
                targetHits++;
            }

            if((rocketPos.y > References.northWall || rocketPos.y < References.southWall ||
                rocketPos.x > References.eastWall || rocketPos.x < References.westWall)
                 && rockets[i].isDead() == false)
            {
                rockets[i].SetToPunish(timeThisGeneration);
                kill = true;
            }

            if(kill == true)
            {
                rockets[i].Kill();
            }
        }
    }
    private void CheckVitals()
    {
        populationDead = true;
        for (int i = 0; i < rockets.Length; i++)
        {
            if(rockets[i].isDead() == false)
            {
                populationDead = false;
            }
        }

        if(populationDead == false)
        {
            timeToRevive = time + References.reviveAfterSec;
        }

        if(timeToRevive < time)
        {
            EvolvePopulation();
            RevivePopulation();
        }
    }

    private string InfoString()
    {
        string infoString;
        infoString = "Generation : " + generation + "\n";
        infoString += "Mutation rate : " + References.MutationRate / 100 + "\n";
        infoString += "Population size : " + References.populationSize + "\n";
        infoString += "Max. fitness achieved : " + maxFitnessAchieved + "\n";
        infoString += "Max. fitness last generation : " + maxFitness + "\n";
        infoString += "Avg. fitness last generation : " + averageFitness + "\n";
        infoString += "Mutations happened : " + mutationHappened + "\n";

        if(References.applyGravity == true)
            infoString += "Gravity : " + References.gravityMagnitude  + " m/s^2\n";
        else
            infoString += "Gravity : " + 0.0f  + "m/s^2\n";
        
        infoString += "Total time elapsed : " + time + "\n";
        if(stoppingCriteria == true)
            infoString += "Stopping criteria reached." + "\n";

        return infoString;
    }

    private void CheckStoppingCriteria()
    {
        if(targetHits > (rockets.Length / 3))
        {
            stoppingCriteria = true;
        }
    }

    private void RevivePopulation()
    {
        for (int i = 0; i < rockets.Length; i++)
        {
            rockets[i].Revive();
        }
        fitnessHistory.Add(generation + "," +  
                    maxFitness.ToString("0.########", System.Globalization.CultureInfo.InvariantCulture) + "," + 
                    averageFitness.ToString("0.########", System.Globalization.CultureInfo.InvariantCulture) + ",");
        timeThisGeneration = 0;
        targetHits = 0;
        generation++;
    }

    private void EvolvePopulation()
    {
        EvaluatePopulation();
        FillGenePool();
        Selection();
        EmptyGenePool();
    }

    private void EvaluatePopulation()
    {
        maxFitness = 0;
        maxFitIndex = 0;
        averageFitness = 0;

        for (int i = 0; i < rockets.Length; i++)
        {
            if(rockets[i].CalculateFitness() > maxFitness)
            {
                maxFitness = rockets[i].GetFitness();
                maxFitIndex = i;
            }
            averageFitness += rockets[i].GetFitness();
        }
        averageFitness = averageFitness / rockets.Length;

        if(maxFitness > maxFitnessAchieved)
        {
            maxFitnessAchieved = maxFitness;
        }

        float normalize = 1 / rockets[maxFitIndex].GetFitness();
        for (int i = 0; i < rockets.Length; i++)
        {
            float original = rockets[i].GetFitness();
            rockets[i].SetFitness(original * normalize);
        }
    }

    private void FillGenePool()
    {
        for(int i = 0; i < rockets.Length; i++)
        {
            float chance = rockets[i].GetFitness() * 100;
            for(int j = 0; j < chance; j++)
            {
                genePool.Add(rockets[i].GetGenes());
            }
        }
    }

    private void EmptyGenePool()
    {
        genePool.Clear();
    }

    private void Selection()
    {
        for (int i = 0; i < rockets.Length; i++)
        {
            int geneA = Maths.RandomInt(0, genePool.Count);
            int geneB = Maths.RandomInt(0, genePool.Count);
            Instruction[] newGenes = CrossOver(genePool[geneA], genePool[geneB]);
            Mutation(newGenes);
            rockets[i].SetGenes(newGenes);
        }
    }

    private Instruction[] CrossOver(Instruction[] geneA, Instruction[] geneB)
    {
        Instruction[] newGene = new Instruction[geneA.Length];
        int crossOverPoint = Maths.RandomInt(0, newGene.Length);

        for(int i = 0; i < newGene.Length; i++)
        {
            if(i < crossOverPoint)
            {
                newGene[i] = geneA[i];
            }
            else
            {
                newGene[i] = geneB[i];
            }
        }
        return newGene;
    }

    private void Mutation(Instruction[] genes)
    {
        if(Maths.RandomFloat(0,100) < References.MutationRate)
        {
            int mutateThisManyGenes = Maths.RandomInt(0,Mathf.FloorToInt(genes.Length / 2));
            for(int i = 0; i < mutateThisManyGenes; i++)
            {
                int lastIndexExecuted = Mathf.FloorToInt(timeThisGeneration / References.instructionsPerSecond);
                int geneIndex = Maths.RandomInt(0, lastIndexExecuted);
                genes[geneIndex] = new Instruction();
            }
            mutationHappened++;
        }
    }

}
