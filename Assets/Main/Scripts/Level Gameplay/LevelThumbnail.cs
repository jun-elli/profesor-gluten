
using Level.CustomSO;
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
            if (info != null)
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
            // not implemented

            // check if previous level is completed to unlock this one
        }

        private void SetUI()
        {
            // preview image
            levelPreview.GetComponent<Image>().sprite = image;

            // title
            titleText.text = title;

            // stars
            starsDisplay.SetStars(maxStarsUserHasAchieved);

            // Locked panel
            lockedPanel.SetActive(isLocked);
        }

        public void HandlePlayButtonClick()
        {
            // Save data
            // save which page we're at


            // load scene
            SceneManager.LoadScene(sceneName);
        }
    }
}

