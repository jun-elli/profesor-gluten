using System.Collections;
using UnityEngine;
using TMPro;

public class TextArchitect
{
    // to make this reusable for other games, must account for ui text or 3d world text
    private TextMeshProUGUI _tmproUI;
    private TextMeshPro _tmproWorld;
    public TMP_Text tmpro => _tmproUI != null ? _tmproUI : _tmproWorld;

    // references to texts
    public string currentText => tmpro.text;
    public string TargetText { get; private set; } = ""; // text we wanna build
    public string PreText { get; private set; } = ""; // previous text to append to
    private int _preTextLength = 0;

    // full text that will appear in the dialogue box when completed
    public string fullTargetText => PreText + TargetText;

    public enum BuildMethod { Instant, Typewriter, Fade } // Fade not implemented for now
    public BuildMethod buildMethod = BuildMethod.Typewriter;

    public Color TextColor { get { return tmpro.color; } set { tmpro.color = value; } }

    // speed
    public float Speed { get { return _BaseSpeed * _speedMultiplier; } set { _speedMultiplier = value; } }
    private const float _BaseSpeed = 1;
    private float _speedMultiplier = 1;

    // in order to speed up, we will reveal more characters per frame / cycle
    public int CharactersPerCycle { get { return Speed <= 2f ? characterMultiplier : Speed <= 2.5f ? characterMultiplier * 2 : characterMultiplier * 3; } }
    private int characterMultiplier = 1;
    public bool hurryUp = false;

    // Constructors
    public TextArchitect(TextMeshProUGUI tmproUI)
    {
        this._tmproUI = tmproUI;
    }

    public TextArchitect(TextMeshPro tmproWorld)
    {
        this._tmproWorld = tmproWorld;
    }

    // Builds text, will delay everything else till finished
    public Coroutine Build(string text)
    {
        PreText = "";
        TargetText = text;

        Stop(); // make sure nothing's running

        buildProcess = tmpro.StartCoroutine(Building());
        return buildProcess;
    }

    // Appends text to what is already in the TA
    public Coroutine Append(string text)
    {
        PreText = tmpro.text;
        TargetText = text;

        Stop(); // make sure nothing's running

        buildProcess = tmpro.StartCoroutine(Building());
        return buildProcess;
    }

    // monitor if TA is already building and stop it if we want
    private Coroutine buildProcess = null;
    public bool isBuilding => buildProcess != null;

    private void Stop()
    {
        if (!isBuilding)
        {
            return;
        }
        tmpro.StopCoroutine(buildProcess);
        buildProcess = null;
    }

    // prepares text, chooses bulding method, makes sure it is finished
    IEnumerator Building()
    {
        Prepare();

        switch (buildMethod)
        {
            case BuildMethod.Typewriter:
                yield return BuildTypewriter();
                break;
            default:
                break;
        }

        OnComplete();
    }

    // prepares TA according to method
    private void Prepare()
    {
        switch (buildMethod)
        {
            case BuildMethod.Typewriter:
                PrepareTypewriter();
                break;
            default:
                PrepareInstant();
                break;
        }
    }
    private void OnComplete()
    {
        buildProcess = null;
        hurryUp = false;
    }

    public void ForceComplete()
    {
        // only implemented for typewriter mode
        tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
        Stop();
        OnComplete();
    }

    // PREPARE & BUILD ACCORDING TO METHOD

    // Prepare text to be build by Instant method
    private void PrepareInstant()
    {
        tmpro.color = tmpro.color; // reset color, fade would change color on vertices but not change main
        tmpro.text = fullTargetText;
        tmpro.ForceMeshUpdate();
        tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount; // make sure all characters are visible
    }
    private void PrepareTypewriter()
    {
        tmpro.color = tmpro.color;
        tmpro.maxVisibleCharacters = 0;
        // if there's already text, show it since it's a previous line
        tmpro.text = PreText;
        if (PreText != "")
        {
            tmpro.ForceMeshUpdate();
            tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
        }
        // then add the line we want to build using typewriter
        tmpro.text += TargetText;
        tmpro.ForceMeshUpdate();
    }
    // prepare fade not yet implemented

    private IEnumerator BuildTypewriter()
    {
        while (tmpro.maxVisibleCharacters < tmpro.textInfo.characterCount)
        {
            tmpro.maxVisibleCharacters += hurryUp ? CharactersPerCycle * 5 : CharactersPerCycle;

            yield return new WaitForSeconds(0.015f / Speed);
        }
    }

    // build fade not yet implemented
}
