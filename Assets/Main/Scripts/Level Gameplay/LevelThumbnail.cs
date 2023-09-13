
using Level.CustomSO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Level
{
    public class LevelThumbnail : MonoBehaviour
    {
        // Information from SO
        [SerializeField] private LevelInfoSO info;

        private string sceneName;
        private int number;
        private string title;
        private Sprite image;

        // Level state
        private bool isLocked = false;
        private bool isCompleted => maxStarsUserHasAchieved >= 3;
        private int maxStarsUserHasAchieved = 0;

        // Ref
        [SerializeField] private StarsDisplay starsDisplay;
        [SerializeField] private GameObject levelPreview;
        [SerializeField] private GameObject lockedPanel;
        [SerializeField] private TextMeshProUGUI titleText;


        private void Start()
        {
            SetLevelThumbnail();
        }

        private void SetLevelThumbnail()
        {
            SetLevelInfoFromSO();

            // Set info from saved data

            // Set visuals
            SetUI();
        }

        private void SetLevelInfoFromSO()
        {
            sceneName = info.sceneName;
            title = info.title;
            number = info.number;
            image = info.image;
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

    }
}

