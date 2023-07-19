using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        // Manage name tag visibility
        public void ShowSpeakerName(string speaker)
        {
            if (speaker.ToUpper() != "NARRATOR")
            {
                Debug.Log("Inside ShowSpeakerName -> if not narrator");
                dialogueContainer.nameContainer.Show(speaker);
            }
            else
            {
                Debug.Log("Inside ShowSpeakerName -> hide");
                HideSpeakerName();
            }
        }
        public void HideSpeakerName() => dialogueContainer.nameContainer.Hide();


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
    }
}