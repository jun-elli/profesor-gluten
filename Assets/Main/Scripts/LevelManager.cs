using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int Points { get; private set; }
    public int Lives { get; private set; }
    [SerializeField] private int levelMaxPoints;
    [SerializeField] private int levelMaxLives;

    // Start is called before the first frame update
    void Start()
    {
        Points = 0;
        Lives = levelMaxLives;
    }

    public void SetPoints(int pointsToAdd)
    {
        Points += pointsToAdd;
        if (Points < 1)
        {
            Points = 0;
            Debug.LogError("Points can't be less than 0");
        }
        if (Points >= levelMaxPoints)
        {
            Points = levelMaxPoints;
            // call win
            Debug.Log("You found all the gluten");
        }
    }

    public void SetLives(int livesToAdd)
    {
        Lives += livesToAdd;
        if (Lives < 1)
        {
            Lives = 0;
            // call game over
            Debug.Log("Level over");
        }
        if (Lives > levelMaxLives)
        {
            Lives = levelMaxLives;
            Debug.LogError("can't have more lives");
        }
    }
}