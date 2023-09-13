using System;
using System.Collections;
using System.Collections.Generic;
using Level.CustomSO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Level
{
    public class ChooseLevelManager : MonoBehaviour
    {
        public static ChooseLevelManager Instance { get; private set; }

        [SerializeField] private List<LevelInfoSO> levelsInformation;

        [SerializeField] private LevelThumbnail leftLevelThumbnail;
        [SerializeField] private LevelThumbnail rightLevelThumbnail;
        [SerializeField] private Image previousArrow;
        [SerializeField] private Image nextArrow;
        [SerializeField] private TextMeshProUGUI pageNumberText;

        // No need to expose it, maybe best as attribute
        public int PageNumber { get; private set; }
        private const int LEVELS_X_PAGE = 2;


        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }

        }

        // Start is called before the first frame update
        void Start()
        {
            SetPageNumber();
            SetPage(PageNumber);
        }

        private void SetPageNumber()
        {
            PageNumber = 1;
        }

        private void SetPage(int pageNumber)
        {
            // Set left level //

            int indexLeftLevel = GetIndexOfLeftLevelByPage(pageNumber);

            // If left index is less than 0, there's no page to show
            while (indexLeftLevel < 0)
            {
                pageNumber--;

                if (pageNumber < 1)
                {
                    Debug.LogError($"Page number is less than 1, can't show any level information.");
                    return;
                }

                indexLeftLevel = GetIndexOfLeftLevelByPage(pageNumber);
            }
            // Set left level info
            leftLevelThumbnail.SetLevelThumbnail(levelsInformation[indexLeftLevel]);

            // Set right level //

            // Find right index
            int indexRightLevel = GetIndexOfRightLevelIfExists(indexLeftLevel);

            // If less than 0, means there's no following level to show
            if (indexRightLevel < 0)
            {
                rightLevelThumbnail.gameObject.SetActive(false);
            }
            else
            {
                rightLevelThumbnail.gameObject.SetActive(true);
                rightLevelThumbnail.SetLevelThumbnail(levelsInformation[indexRightLevel]);
            }

            SetArrows(pageNumber);
            pageNumberText.text = $"{pageNumber}";
        }

        private int GetIndexOfLeftLevelByPage(int pageNumber)
        {

            int availablePages = Mathf.CeilToInt((float)levelsInformation.Count / (float)LEVELS_X_PAGE);

            // If page requested does not exist
            if (pageNumber > availablePages)
            {
                Debug.LogError($"Page number '{pageNumber}' is too big, doesn't exist.");
                return -1;
            }

            // -1 for index and -1 for left side
            // Ex: page 5 has element 9 and 10 (if in each page there are 2), and we want left = 9
            int leftIndex = pageNumber * LEVELS_X_PAGE - 2;
            return leftIndex;
        }

        private int GetIndexOfRightLevelIfExists(int leftLevelIndex)
        {
            int rightIndex = leftLevelIndex + 1;

            if (rightIndex >= levelsInformation.Count)
            {
                return -1;
            }
            return rightIndex;
        }

        private void SetArrows(int pageNumber)
        {
            int availablePages = Mathf.CeilToInt((float)levelsInformation.Count / (float)LEVELS_X_PAGE);

            if (pageNumber == 1)
            {
                previousArrow.gameObject.SetActive(false);

                // If there are only 1 or 2 levels total, won't need next arrow
                if (levelsInformation.Count <= LEVELS_X_PAGE)
                {
                    nextArrow.gameObject.SetActive(false);
                }
                else
                {
                    nextArrow.gameObject.SetActive(true);
                }
            }
            else if (pageNumber == availablePages)
            {
                previousArrow.gameObject.SetActive(true);
                nextArrow.gameObject.SetActive(false);
            }
            else if (pageNumber > 1 && pageNumber < availablePages)
            {
                previousArrow.gameObject.SetActive(true);
                nextArrow.gameObject.SetActive(true);
            }
        }

        public void HandleArrowClick(bool isNextPage)
        {
            if (isNextPage)
            {
                HandleNextArrowClick();
            }
            else
            {
                HandlePreviousArrowClick();
            }
        }

        private void HandlePreviousArrowClick()
        {
            int targetPage = PageNumber - 1;

            if (targetPage < 1)
            {
                Debug.LogWarning("There's no previous page.");
                return;
            }

            PageNumber--;
            SetPage(PageNumber);
        }

        private void HandleNextArrowClick()
        {
            int targetPage = PageNumber + 1;

            int availablePages = Mathf.CeilToInt((float)levelsInformation.Count / (float)LEVELS_X_PAGE);
            if (targetPage > availablePages)
            {
                Debug.LogWarning("There's no next page.");
                return;
            }

            PageNumber++;
            SetPage(PageNumber);
        }
    }
}
