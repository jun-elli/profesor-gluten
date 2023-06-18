using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public int Points { get; private set; }
    public int Lives { get; private set; }
    [SerializeField] private int levelMaxPoints;
    [SerializeField] private int levelMaxLives;

    // display points and lives
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private GameObject pointsDisplay;
    private Slider pointsBar;

    // level over popup
    [SerializeField] private GameObject levelOverPopup;
    private OverPopup overPopup;

    // Start is called before the first frame update
    void Start()
    {
        pointsBar = pointsDisplay.GetComponent<Slider>();
        overPopup = levelOverPopup.GetComponent<OverPopup>();
        ResetLevel();
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
            overPopup.DisplayPopup(true, Points, levelMaxPoints);
        }
        pointsBar.value = Points;
    }

    private void ResetPoints()
    {
        Points = 0;
        pointsBar.maxValue = levelMaxPoints;
        pointsBar.value = Points;
    }

    public void SetLives(int livesToAdd)
    {
        Lives += livesToAdd;
        if (Lives < 1)
        {
            Lives = 0;
            // call game over
            overPopup.DisplayPopup(false, Points, levelMaxPoints);
        }
        if (Lives > levelMaxLives)
        {
            Lives = levelMaxLives;
            Debug.LogError("can't have more lives");
        }
        livesText.text = "Lives: " + Lives;
    }
    private void ResetLives()
    {
        Lives = levelMaxLives;
        livesText.text = "Lives: " + Lives;
    }

    private void ResetLevel()
    {
        ResetPoints();
        ResetLives();
        levelOverPopup.SetActive(false);
    }

    public void ReturnToLevels()
    {
        Debug.Log("Level over, return to level selection screen.");
    }
}
