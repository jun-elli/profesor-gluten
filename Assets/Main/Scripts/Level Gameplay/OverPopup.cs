using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class OverPopup : MonoBehaviour
{

    [SerializeField] private ScoreDisplay display;


    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI resultText;

    private const string ThreeStarsMessage = "¡Tienes el gluten controlado!";
    private const string TwoStarsMessage = "¡Casi lo tenías!";
    private const string OneStarMessage = "¡No pasa nada, sigue buscando!";

    // Start is called before the first frame update
    void Start()
    {

    }

    public void DisplayPopup(GameOverInformation information)
    {
        // set level name
        titleText.text = information.levelName;
        // set win or lose message
        SetResultMessage(information);
        // show stars
        display.ShowScore(information);
        // set active
        gameObject.SetActive(true);
    }

    private void SetResultMessage(GameOverInformation information)
    {

        if (information.hasUserWon)
        {
            resultText.text = ThreeStarsMessage;
        }
        else if (!information.hasUserWon && information.points >= information.twoStarPoints)
        {
            resultText.text = TwoStarsMessage;
        }
        else
        {
            resultText.text = OneStarMessage;
        }
    }

}
