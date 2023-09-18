using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System;

namespace SavableData
{
    public class SaveDataManager : MonoBehaviour
    {
        public static SaveDataManager Instance { get; private set; }

        public static UserData SavedUserData { get; private set; }

        private const string USER_DATA_FILE_NAME = "userDataSave.json";

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);


            }
        }

        private void Start()
        {
            LoadUserData();
        }

        private void LoadUserData()
        {
            if (!File.Exists(FilePaths.root + USER_DATA_FILE_NAME))
            {
                InitializeBlankSaveFile();
            }

            string json = File.ReadAllText(FilePaths.root + USER_DATA_FILE_NAME);
            UserData data = JsonUtility.FromJson<UserData>(json);
            SavedUserData = data;
        }

        // Create an empty
        private void InitializeBlankSaveFile()
        {
            UserData data = new()
            {
                lastPageVisited = 1
            };

            string json = JsonUtility.ToJson(data);
            File.WriteAllText(FilePaths.root + USER_DATA_FILE_NAME, json);
        }

        public void SaveCurrentUserData()
        {
            string json = JsonUtility.ToJson(SavedUserData);
            File.WriteAllText(FilePaths.root + USER_DATA_FILE_NAME, json);
        }

        public void SetLastPageVisited(int pageNumber)
        {
            SavedUserData.lastPageVisited = pageNumber;
        }

        public void SetLevelProgress(int levelNumber, string levelTitle, int maxStarsUserHasAchieved)
        {
            // check if level is already saved
            LevelData level = SavedUserData.levelsProgress.Find(level => level.number == levelNumber);

            // If level is null, create new
            if (level == null)
            {
                level = new()
                {
                    number = levelNumber,
                    title = levelTitle,
                    maxStarsUserHasAchieved = maxStarsUserHasAchieved
                };

                SavedUserData.levelsProgress.Add(level);
            }
            else
            {   // Update stars
                level.maxStarsUserHasAchieved = maxStarsUserHasAchieved;
            }
        }

        [System.Serializable]
        public class UserData
        {
            public List<LevelData> levelsProgress;
            public int starsProgress => levelsProgress.Sum(level => level.maxStarsUserHasAchieved);
            public int lastPageVisited;
        }

        [System.Serializable]
        public class LevelData
        {
            public int number;
            public string title;
            public int maxStarsUserHasAchieved;
            public bool isCompleted => maxStarsUserHasAchieved >= 3;
        }



    }


}
