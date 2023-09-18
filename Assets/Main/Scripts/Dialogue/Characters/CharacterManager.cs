using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dialogue.Characters
{
    /// <summary>
    /// Manager to create, store in dict and retrieve Characters
    /// </summary>
    public class CharacterManager : MonoBehaviour
    {
        public static CharacterManager Instance { get; private set; }
        private Dictionary<string, Character> characters = new Dictionary<string, Character>();

        private CharacterConfigSO config => DialogueSystem.Instance.Config.characterConfigurationAsset;

        // To retrieve prefab
        private const string CharacterCastingID = " as ";
        private const string CharacterNameID = "<characterName>";
        private string _characterRootPath => $"Characters/{CharacterNameID}";
        private string _characterPrefabPath => $"{_characterRootPath}/{CharacterNameID}";

        [SerializeField] private RectTransform _characterPanel = null;
        public RectTransform CharacterPanel => _characterPanel;

        private void Awake()
        {
            if (Instance != null)
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        public Character CreateCharacter(string characterName, bool shouldEnableOnCreation = false)
        {
            string[] names = characterName.Split(CharacterCastingID, System.StringSplitOptions.RemoveEmptyEntries);
            string dictName = names[0].ToLower();

            // Check if it already exists in manager dictionary
            if (characters.ContainsKey(dictName))
            {
                Debug.LogError($"Character {characterName} already exists. Duplicate not created.");
                return null;
            }
            // Get character config data
            CharacterInfo info = GetCharacterInfo(characterName);
            // Create character with data
            Character character = CreateChatacterFromInfo(info);
            // Add to dict
            characters.Add(info.name.ToLower(), character);
            // Show or keep hidden
            if (shouldEnableOnCreation && !character.IsVisible)
            {
                character.Show();
            }
            else if (!shouldEnableOnCreation && character.IsVisible)
            {
                character.IsVisible = false;
            }
            return character;
        }

        // Get name and config data of a character by getting it from
        // the CharacterConfigSO asset we have in the DialogueSystemConfigSO
        private CharacterInfo GetCharacterInfo(string name)
        {
            CharacterInfo result = new CharacterInfo();
            string[] names = name.Split(CharacterCastingID, System.StringSplitOptions.RemoveEmptyEntries);
            // First item will always be the name
            result.name = names[0];
            // If there is no casting, then grab config from name
            result.castingName = names.Length > 1 ? names[1] : result.name;
            result.config = config.GetConfig(result.castingName);
            result.prefab = GetPrefabForCharacter(result.castingName);
            result.rootCharacterFolder = FormatCharacterPath(_characterRootPath, result.castingName);

            return result;
        }

        private GameObject GetPrefabForCharacter(string name)
        {
            string prefabPath = FormatCharacterPath(_characterPrefabPath, name);
            return Resources.Load<GameObject>(prefabPath);
        }

        // We inject the character name to the path to get the correct direction
        private string FormatCharacterPath(string path, string name) => path.Replace(CharacterNameID, name);

        private Character CreateChatacterFromInfo(CharacterInfo info)
        {
            CharacterConfigData config = info.config;

            switch (config.characterType)
            {
                case Character.CharacterType.Text:
                    return new CharacterText(info.name, config);
                case Character.CharacterType.Sprite:
                case Character.CharacterType.SpriteSheet:
                    return new CharacterSprite(info.name, config, info.prefab, info.rootCharacterFolder);
                case Character.CharacterType.Live2D:
                    return new CharacterLive2D(info.name, config, info.prefab, info.rootCharacterFolder);
                case Character.CharacterType.Model3D:
                    return new CharacterModel3D(info.name, config, info.prefab, info.rootCharacterFolder);
                default:
                    Debug.LogError("Wrong character type. Can't create new character.");
                    return null;
            }
        }

        public Character GetCharacter(string characterName, bool shouldCreateIfDoesNotExist = false)
        {
            // If character is cast "X as Y" another, it will be saved in the dictionary as X
            string[] names = characterName.Split(CharacterCastingID, System.StringSplitOptions.RemoveEmptyEntries);
            string lowName = names[0].ToLower();

            if (characters.ContainsKey(lowName))
            {
                return characters[lowName];
            }
            else if (shouldCreateIfDoesNotExist)
            {
                return CreateCharacter(characterName);
            }

            return null;
        }

        public CharacterConfigData GetCharacterConfig(string characterName)
        {
            return config.GetConfig(characterName);
        }

        // We sort them ALL from low to high priority
        public void SortCharacters()
        {
            List<Character> activeCharacters = characters.Values.Where(c => c.rootTransform.gameObject.activeInHierarchy && c.IsVisible).ToList();
            List<Character> inactiveCharacters = characters.Values.Except(activeCharacters).ToList();

            activeCharacters.Sort((a, b) => a.Priority.CompareTo(b.Priority));
            activeCharacters.Concat(inactiveCharacters);
            SortCharactersInHierarchy(activeCharacters);
        }

        // We prioritize this group of characters above the rest, in order we write them on the array
        public void SortCharacters(string[] characterNames)
        {
            List<Character> sortedCharacters = characterNames
                .Select(name => GetCharacter(name))
                .Where(c => c != null)
                .Reverse()
                .ToList();

            List<Character> remainingCharacters = characters.Values
                .Except(sortedCharacters)
                .OrderBy(character => character.Priority)
                .ToList();

            // Update priority to match sorting
            int startPriority = remainingCharacters.Count > 0 ? remainingCharacters.Max(c => c.Priority) : 0;
            for (int i = 0; i < sortedCharacters.Count; i++)
            {
                Character character = sortedCharacters[i];
                character.SetPriority(startPriority + i + 1, false);
            }

            List<Character> allCharacters = remainingCharacters.Concat(sortedCharacters).ToList();
            SortCharactersInHierarchy(allCharacters);
        }

        private void SortCharactersInHierarchy(List<Character> charactersSortingOrder)
        {
            int i = 0;
            foreach (Character character in charactersSortingOrder)
            {
                character.rootTransform.SetSiblingIndex(i++);
            }
        }

        // Class to store character data: name and config
        private class CharacterInfo
        {
            public string name = "";
            public string castingName = "";
            public CharacterConfigData config = null;
            public GameObject prefab = null;
            public string rootCharacterFolder = "";
        }
    }
}