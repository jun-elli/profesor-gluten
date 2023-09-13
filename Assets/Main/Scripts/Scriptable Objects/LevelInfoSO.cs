using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level.CustomSO
{
    [CreateAssetMenu(fileName = "New LevelInfoSO", menuName = "Custom SO/Level Info")]
    public class LevelInfoSO : ScriptableObject
    {
        public string sceneName;
        public int number;
        public string title;
        public Sprite image;
    }
}

