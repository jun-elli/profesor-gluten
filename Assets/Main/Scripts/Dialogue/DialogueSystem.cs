using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialogue.Characters;

namespace Dialogue
{
    public class DialogueSystem : MonoBehaviour
    {
        [SerializeField] private DialogueSystemConfigurationSO _config;
        public DialogueSystemConfigurationSO Config => _config;

        public DialogueContainer dialogueContainer = new DialogueContainer();
        private ConversationManager _conversationManager;
        private TextArchitect _architect;
        private bool _isInitialized = false;
        public static DialogueSystem Instance;
        public bool isRunningConversation => _conversationManager.isRunning;

        // Manage user prompt to hurry up or go to next dialogue line
        public delegate void DialogueSystemEvent();
        public event DialogueSystemEvent onUserPromptAdvance;

        // Dialogue Panels and UI
        [SerializeField] private GameObject _charactersLayer;
        [SerializeField] private GameObject _dialogueLayer;

        // would be nice to have another background to put on top of popups but behind characters


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                Initialize();
            }
            else
            {
                DestroyImmediate(gameObject);
            }
        }
        // Initialize text architect, only one instance
        private void Initialize()
        {
            if (_isInitialized)
            {
                return;
            }
            _architect = new TextArchitect(dialogueContainer.dialogueText);
            _conversationManager = new ConversationManager(_architect);
            _isInitialized = true;
        }

        // Run methods subscribed to delegate event (if not null)
        public void OnUserPromptAdvance()
        {
            onUserPromptAdvance?.Invoke();
        }
        /////////////////////////////////////////////////////
        /// // Dialogue Container and Name Container //
        /// //////////////////////////////////////////////////

        // If we don't have the character + config
        public void ApplySpeakerDataToDialogueContainer(string speakerName)
        {
            Character character = CharacterManager.Instance.GetCharacter(speakerName, true);
            CharacterConfigData config = character.config != null ? character.config : CharacterManager.Instance.GetCharacterConfig(speakerName);

            ApplySpeakerDataToDialogueContainer(config);
        }
        // If we have the config data already
        public void ApplySpeakerDataToDialogueContainer(CharacterConfigData config)
        {
            dialogueContainer.SetConfig(config);
        }


        // Manage name tag visibility
        public void ShowSpeakerName(string speaker)
        {
            if (speaker.ToUpper() != "NARRATOR")
            {
                dialogueContainer.nameContainer.Show(speaker);
            }
            else
            {
                HideSpeakerName();
            }
        }
        public void HideSpeakerName() => dialogueContainer.nameContainer.Hide();

        ////////////////////////////////////
        /// Send to conversation manager ///
        /// ///////////////////////////////

        // Prepare dialogue file and show dialogue container

        public void PlayConversation(TextAsset file)
        {
            // File to strings
            List<string> lines = FileManager.ReadTextAsset(file, false);

            // Enable dialogue UI
            _charactersLayer.SetActive(true);
            _dialogueLayer.SetActive(true);

            // Play conversation
            Say(lines);
        }

        // Send conversation to conManager
        public Coroutine Say(string speaker, string dialogue)
        {
            List<string> conversation = new List<string>() { $"{speaker} \"{dialogue}\"" };
            return Say(conversation);
        }

        public Coroutine Say(List<string> conversation)
        {
            return _conversationManager.StartConversation(conversation);
        }

        // Hide VN layers
        public void HideDialogueLayers()
        {
            // disable characters layer 
            _charactersLayer.SetActive(false);
            // disable dialogue layer
            _dialogueLayer.SetActive(false);
        }
    }
}