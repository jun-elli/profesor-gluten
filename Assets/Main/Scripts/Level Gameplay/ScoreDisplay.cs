using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private List<Image> dots;
    [SerializeField] private List<Image> stars;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowScore(int score, int oneStarPoints, int twoStarPoints, int threeStarPoints)
    {
        // set inactive all stars and active all dots to begin with
        ResetDotsAndStars();

        // if no points less tahn 1 star
        if (score >= oneStarPoints && score < twoStarPoints)
        {
            SetStarsActive(1);

        }
        else if (score >= twoStarPoints && score < threeStarPoints)
        {
            // show 2 stars
            SetStarsActive(2);

        }
        else if (score >= threeStarPoints)
        {
            // show 3 stars
            SetStarsActive(3);
        }
    }

    private void SetStarsActive(int starsToActivate)
    {

        for (int i = 0; i < starsToActivate; i++)
        {
            stars[i].gameObject.SetActive(true);
            dots[i].gameObject.SetActive(false);
        }
    }

    private void ResetDotsAndStars()
    {
        foreach (Image dot in dots)
        {
            dot.gameObject.SetActive(true);
        }

        foreach (Image star in stars)
        {
            star.gameObject.SetActive(false);
        }
    }
}
