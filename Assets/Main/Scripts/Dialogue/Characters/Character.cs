using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Dialogue.Characters
{
    public abstract class Character
    {
        // Const
        protected const bool ShowOnStart = true;
        private const float UnhighlightedStrength = 0.65f;

        // Vars
        public string name = "";
        public string displayName = "";
        public RectTransform rootTransform = null;
        public Animator animator;
        public CharacterConfigData config;
        public Color Color { get; protected set; } = Color.white;
        protected Color displayColor => IsHighlighted ? highlightedColor : unhighlightedColor;
        protected Color highlightedColor => Color;
        protected Color unhighlightedColor => new Color(Color.r * UnhighlightedStrength, Color.g * UnhighlightedStrength, Color.b * UnhighlightedStrength, Color.a);
        public bool IsHighlighted { get; protected set; } = true;

        // Show and Hide fields
        protected Coroutine co_revealing, co_hiding;
        public bool isRevealing => co_revealing != null;
        public bool isHiding => co_hiding != null;
        public virtual bool IsVisible { get; set; }

        // Moving
        protected Coroutine co_moving;
        public bool isMoving => co_moving != null;

        // Changing color
        protected Coroutine co_changingColor;
        public bool isChangingColor => co_changingColor != null;

        // Highlighting
        protected Coroutine co_highlighting = null;
        public bool isHighlighting => IsHighlighted && co_highlighting != null;
        public bool isUnhighlighting => !IsHighlighted && co_highlighting != null;

        // Managers
        protected CharacterManager characterManager => CharacterManager.Instance;
        private DialogueSystem _dialogueSystem => DialogueSystem.Instance;

        public enum CharacterType { Text, Sprite, SpriteSheet, Live2D, Model3D }

        // Constructor
        public Character(string name, CharacterConfigData config, GameObject prefab)
        {
            this.name = name;
            displayName = name;
            this.config = config;
            if (prefab != null)
            {
                GameObject ob = Object.Instantiate(prefab, CharacterManager.Instance.CharacterPanel);
                ob.name = $"{name} - {displayName} - {config.name}";
                ob.SetActive(true);
                rootTransform = ob.GetComponent<RectTransform>();
                animator = rootTransform.GetComponentInChildren<Animator>();
            }
        }

        public Coroutine Say(string line) => Say(new List<string> { line });

        public Coroutine Say(List<string> dialogue)
        {
            List<string> conversation = new List<string>();

            foreach (string line in dialogue)
            {
                conversation.Add($"{displayName} \"{line}\"");
            }
            _dialogueSystem.ShowSpeakerName(displayName);
            UpdateTextCostumizationOnScreen();
            return _dialogueSystem.Say(conversation);
        }

        // Set config
        public void SetDialogueColor(Color color) => config.dialogueColor = color;
        public void SetNameColor(Color color) => config.nameColor = color;

        public void SetDialogueFont(TMP_FontAsset font) => config.dialogueFont = font;
        public void SetNameFont(TMP_FontAsset font) => config.nameFont = font;

        public void UpdateTextCostumizationOnScreen() => _dialogueSystem.ApplySpeakerDataToDialogueContainer(config);
        public void ResetConfigurationData() => config = CharacterManager.Instance.GetCharacterConfig(name);


        // Show and Hide methods
        public virtual Coroutine Show()
        {
            if (isRevealing)
            {
                return co_revealing;
            }
            if (isHiding)
            {
                characterManager.StopCoroutine(co_hiding);
            }
            return co_revealing = characterManager.StartCoroutine(ShowOrHide(true));
        }

        public virtual Coroutine Hide()
        {
            if (isHiding)
            {
                return co_hiding;
            }
            if (isRevealing)
            {
                characterManager.StopCoroutine(co_revealing);
            }
            return co_hiding = characterManager.StartCoroutine(ShowOrHide(false));
        }

        public virtual IEnumerator ShowOrHide(bool show)
        {
            yield return null;
        }

        /////////////////////////////
        /// Position and movement ///
        /// ////////////////////////

        // We will pass a normalized position from 0 to 1
        public virtual void SetPosition(Vector2 position)
        {
            if (rootTransform == null)
            {
                Debug.LogWarning("Text Characters don't have visuals.");
                return;
            }
            (Vector2 minAnchorTarget, Vector2 maxAnchorTarget) = ConvertUITargetPositionToRelativeCharacterAnchors(position);

            rootTransform.anchorMin = minAnchorTarget;
            rootTransform.anchorMax = maxAnchorTarget;
        }

        protected virtual (Vector2, Vector2) ConvertUITargetPositionToRelativeCharacterAnchors(Vector2 position)
        {
            // We find the vector that goes from the bottom left corner to the upper right corner of our character rect
            // These are normalized values
            Vector2 diagonal = rootTransform.anchorMax - rootTransform.anchorMin;

            // We get how much X and Y we have left to move our character around
            float maxX = 1f - diagonal.x;
            float maxY = 1f - diagonal.y;

            // We multiply our normalized position for the max movement we can afford
            Vector2 minAnchorTarget = new Vector2(maxX * position.x, maxY * position.y);
            Vector2 maxAnchorTarget = minAnchorTarget + diagonal;

            return (minAnchorTarget, maxAnchorTarget);
        }

        public virtual Coroutine MoveToPosition(Vector2 position, float speed = 2f, bool isSmooth = false)
        {
            if (rootTransform == null)
            {
                Debug.LogWarning("Text Characters don't have visuals and can't move.");
                return null;
            }

            if (isMoving)
            {
                characterManager.StopCoroutine(co_moving);
            }

            co_moving = characterManager.StartCoroutine(MovingToPosition(position, speed, isSmooth));
            return co_moving;
        }

        protected virtual IEnumerator MovingToPosition(Vector2 position, float speed, bool isSmooth)
        {
            (Vector2 minAnchorTarget, Vector2 maxAnchorTarget) = ConvertUITargetPositionToRelativeCharacterAnchors(position);
            Vector2 diagonal = rootTransform.anchorMax - rootTransform.anchorMin;

            while (rootTransform.anchorMin != minAnchorTarget)
            {
                rootTransform.anchorMin = isSmooth ?
                    Vector2.Lerp(rootTransform.anchorMin, minAnchorTarget, speed * Time.deltaTime)
                    : Vector2.MoveTowards(rootTransform.anchorMin, minAnchorTarget, speed * Time.deltaTime * 0.35f);

                rootTransform.anchorMax = rootTransform.anchorMin + diagonal;

                if (isSmooth && Vector2.Distance(rootTransform.anchorMin, minAnchorTarget) <= 0.002f)
                {
                    rootTransform.anchorMin = minAnchorTarget;
                    rootTransform.anchorMax = maxAnchorTarget;
                    break;
                }
                yield return null;
            }
            Debug.Log("Done moving");
            co_moving = null;
        }

        /////////////////////////////
        ///////// Colors ///////////
        /// ////////////////////////

        public virtual void SetColor(Color color)
        {
            this.Color = color;
        }

        public Coroutine TransitionColor(Color color, float speed = 1f)
        {
            this.Color = color;

            if (isChangingColor)
            {
                characterManager.StopCoroutine(co_changingColor);
            }

            co_changingColor = characterManager.StartCoroutine(ChangingColor(displayColor, speed));

            return co_changingColor;
        }

        public virtual IEnumerator ChangingColor(Color color, float speed)
        {
            Debug.Log("Can't change color of a text Character");
            yield return null;
        }

        public Coroutine Highlight(float speed = 1f)
        {
            if (isHighlighting)
            {
                return co_highlighting;
            }

            if (isUnhighlighting)
            {
                characterManager.StopCoroutine(co_highlighting);
            }

            IsHighlighted = true;
            co_highlighting = characterManager.StartCoroutine(Highlighting(IsHighlighted, speed));
            return co_highlighting;
        }

        public Coroutine Unhighlight(float speed = 1f)
        {
            if (isUnhighlighting)
            {
                return co_highlighting;
            }

            if (isHighlighting)
            {
                characterManager.StopCoroutine(co_highlighting);
            }

            IsHighlighted = false;
            co_highlighting = characterManager.StartCoroutine(Highlighting(IsHighlighted, speed));
            return co_highlighting;
        }

        protected virtual IEnumerator Highlighting(bool highlight, float speed)
        {
            Debug.Log("Text Character can't be highlighted!");
            yield return null;
        }

    }
}