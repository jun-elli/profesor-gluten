using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Dialogue;
using Level.CustomSO;
using System.Linq;
using UnityEngine.SceneManagement;
using SavableData;
using Level;

public class LevelManager : MonoBehaviour
{
    // Level vars
    [SerializeField] private string levelName;
    [SerializeField] private int levelNumber;
    public int Points { get; private set; }
    public int Lives { get; private set; }
    [SerializeField] private int levelMaxPoints;
    [SerializeField] private int levelMaxLives;

    [SerializeField] private int oneStarPoints;
    [SerializeField] private int twoStarPoints;
    [SerializeField] private int threeStarPoints;

    [SerializeField] private List<Item> items;

    // display points and lives
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private Slider pointsBar;

    // level over popup
    [SerializeField] private OverPopup overPopup;

    // Coroutine waiting for dialogue to finish and show over popup
    private Coroutine waitingProcess = null;
    private bool isWaitingForDialogueToEnd => waitingProcess != null;

    // Constants
    private const string LivesText = "Intentos: ";

    // Scene management
    [SerializeField] private int chooseLevelSceneIndex;

    // Start is called before the first frame update
    void Start()
    {
        ResetLevel();
        AudioManager.Instance.StopMainTheme();
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
            // overPopup.DisplayPopup(true, Points, oneStarPoints, twoStarPoints, threeStarPoints);
            ShowOverPopup();
        }
        pointsBar.value = Points;

        // Check if there are still items actives

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
            // overPopup.DisplayPopup(false, Points, oneStarPoints, twoStarPoints, threeStarPoints);
            ShowOverPopup();
        }
        if (Lives > levelMaxLives)
        {
            Lives = levelMaxLives;
            Debug.LogError("can't have more lives");
        }
        livesText.text = LivesText + Lives;
    }
    private void ResetLives()
    {
        Lives = levelMaxLives;
        livesText.text = LivesText + Lives;
    }

    private void ResetLevel()
    {
        ResetPoints();
        ResetLives();
        overPopup.gameObject.SetActive(false);
    }

    public void ReturnToLevels()
    {
        SaveLevelOutcome();
        SceneManager.LoadScene(chooseLevelSceneIndex);
    }

    private void SaveLevelOutcome()
    {
        SaveDataManager.Instance.SetLevelProgress(levelNumber, levelName, Points);
        SaveDataManager.Instance.SaveCurrentUserData();
    }

    // Check if there are still active items on scene

    public void CheckForActiveItemsOnScene()
    {
        if (items.All(i => !i.isActiveAndEnabled))
        {
            ShowOverPopup();
        }
    }

    // ////// Handle Running dialogue and wait to show game over popup //////

    private Coroutine ShowOverPopup()
    {
        StopWaitingProcess();

        waitingProcess = StartCoroutine(WaitForDialogueEndToShowOverPopup());

        return waitingProcess;
    }

    private IEnumerator WaitForDialogueEndToShowOverPopup()
    {
        while (DialogueSystem.Instance.isRunningConversation)
        {
            yield return null;
        }
        GameOverInformation information = new(Points, oneStarPoints, twoStarPoints, threeStarPoints, levelName);
        overPopup.DisplayPopup(information);
    }

    private void StopWaitingProcess()
    {
        if (isWaitingForDialogueToEnd)
        {
            StopCoroutine(waitingProcess);
        }
    }

}

public readonly struct GameOverInformation
{
    public readonly int points;
    public readonly int oneStarPoints;
    public readonly int twoStarPoints;
    public readonly int threeStarPoints;
    public readonly string levelName;

    public GameOverInformation(int points, int oneStarPoints, int twoStarPoints, int threeStarPoints, string levelName)
    {
        this.points = points;
        this.oneStarPoints = oneStarPoints;
        this.twoStarPoints = twoStarPoints;
        this.threeStarPoints = threeStarPoints;
        this.levelName = levelName;
    }
}
