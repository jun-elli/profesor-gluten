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



    // Start is called before the first frame update
    void Start()
    {
        titleText.text = SceneManager.GetActiveScene().name;
    }

    public void DisplayPopup(GameOverInformation information)
    {
        // set win or lose message
        SetResultMessage(information.hasUserWon);
        // show stars
        display.ShowScore(information);
        // set active
        gameObject.SetActive(true);
    }

    private void SetResultMessage(bool hasWon)
    {
        if (hasWon)
        {
            resultText.text = "Has encontrado todo el gluten!";
        }
        else
        {
            resultText.text = "Que pena, sigue buscando!";
        }
    }

}
