using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Level
{
    public class StarsDisplay : MonoBehaviour
    {
        [SerializeField] private List<Image> stars;
        [SerializeField] private Sprite noStarSprite;
        [SerializeField] private Sprite starSprite;


        public void SetStars(int numberOfStars)
        {
            // Guard
            if (numberOfStars < 0 || numberOfStars > 3)
            {
                Debug.LogWarning($"Number can't be {numberOfStars}");
                return;
            }

            // Reset stars
            foreach (Image star in stars)
            {
                star.sprite = noStarSprite;
            }

            // Apply stars
            for (int i = 0; i < numberOfStars; i++)
            {
                stars[i].sprite = starSprite;
            }
        }
    }
}