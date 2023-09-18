
using Level.CustomSO;
using SavableData;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Level
{
    public class LevelThumbnail : MonoBehaviour
    {
        // Information from SO
        private LevelInfoSO info;

        private string sceneName;
        private int number;
        private string title;
        private Sprite image;

        // Level state
        private bool isLocked = true;
        private bool isCompleted => maxStarsUserHasAchieved >= 3;
        private int maxStarsUserHasAchieved = 0;

        // Ref
        [SerializeField] private StarsDisplay starsDisplay;
        [SerializeField] private GameObject levelPreview;
        [SerializeField] private GameObject lockedPanel;
        [SerializeField] private TextMeshProUGUI titleText;


        private void Start()
        {
        }

        public void SetLevelThumbnail(LevelInfoSO levelInfoSO)
        {
            info = levelInfoSO;

            SetLevelInfoFromSO();

            // Set info from saved data
            SetUserProgressOnLevel();

            // Set visuals
            SetUI();
        }

        private void SetLevelInfoFromSO()
        {
            if (info == null)
            {
                Debug.LogError("No level SO found.");
                return;
            }
            else
            {
                sceneName = info.sceneName;
                title = info.title;
                number = info.number;
                image = info.image;
            }
            if (number == 1)
            {
                isLocked = false;
            }
        }

        private void SetUserProgressOnLevel()
        {
            SaveDataManager.LevelData level = SaveDataManager.SavedUserData.levelsProgress.Find(l => l.number == number);

            if (level != null)
            {
                maxStarsUserHasAchieved = level.maxStarsUserHasAchieved;
            }
        }

        private void SetUI()
        {
            // preview image
            levelPreview.GetComponent<Image>().sprite = image;

            // title
            titleText.text = title;

            // stars
            starsDisplay.SetStars(maxStarsUserHasAchieved);

            // check if previous level is completed to unlock this one
            CheckIfPreviousLevelIsCompleted();
            lockedPanel.SetActive(isLocked);
        }

        private void CheckIfPreviousLevelIsCompleted()
        {
            if (number > 1)
            {
                SaveDataManager.LevelData previousLevel = SaveDataManager.SavedUserData.levelsProgress.Find(l => l.number == number - 1);

                isLocked = previousLevel == null || !previousLevel.isCompleted;
            }

        }

        public void HandlePlayButtonClick()
        {
            // Save which page we're at
            int page = ChooseLevelManager.Instance.PageNumber;
            SaveDataManager.Instance.SetLastPageVisited(page);

            // load scene
            SceneManager.LoadScene(sceneName);
        }
    }
}

