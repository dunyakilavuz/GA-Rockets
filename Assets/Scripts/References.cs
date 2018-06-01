using System.Collections;
using System.Collections.Generic;
using Godot;
using GodotExt.Engine;

public class References : Node
{
    /*GA related settings*/
    public static float PunishRate = 10;
    public static float RewardRate = 15;
    public static float MutationRate = 10f; // In percent
    public static int populationSize = 10;
    public static int instructionSize = 10; // Gene size
    public static float instructionsPerSecond = 1f;  // Gene's executed per second.
    public static float getResultsOnGenerations = 150;
    /*GA related settings*/



    /*Rocket settings */
    public static float rocketStartHeight = 10;
    public static float rocketScale = 2.5f;
    public static float rocketMass = 10;
    public static float maxThrust = 50;
    public static float rocketRotateSpeed = 250;
    /*Rocket settings */


    /*Obstacle settings */
    public static float obstacleStartHeight = 200;
    public static float obstacleEndHeight = 350;
    public static float obstacleHorizontalRange = 250;
    public static float obstacleMinScale = 15;
    public static float obstacleMaxScale = 25;
    public static int obstaclePopulationSize = 10;
    /*Obstacle settings */

    /*Target settings */
    public static float targetStartHeight = 350;
    public static float targetEndHeight = 450;
    public static float targetHorizontalRange = 200;
    public static float targetMinScale = 10;
    public static float targetMaxScale = 20;


    /*General settings */
    public static float reviveAfterSec = 1.5f;
    public static bool applyGravity = true;
    public static float gravityMagnitude = 9.8f;
    /*General settings */

    /*Wall settings */
    public static float wallThickness = 5 / 2;
    public static float northWall = 495 - wallThickness; //Dont go upper than in y;
    public static float southWall = -5 + wallThickness; //Dont go belower than in y;
    public static float eastWall = 250 - wallThickness; //Dont go upper than in x;
    public static float westWall = -250 + wallThickness;//Dont go belower than in x;
    /*Wall settings */
}