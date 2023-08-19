using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialogue;
using System;
using System.Linq;
using Dialogue.Characters;

namespace Dialogue.Commands
{
    public class CMD_DatabaseExtensionCharacters : CMD_DatabaseExtension
    {
        private static readonly string[] ImmediateIdentifier = { "?i", "?immediate" };
        private static readonly string[] EnableIdentifier = { "?e", "?enable" };
        private static readonly string[] SpeedIdentifier = { "?spd", "?speed" };
        private static readonly string[] SmoothIdentifier = { "?sm", "?smooth" };
        private const string XIdentifier = "?x";
        private const string YIdentifier = "?y";

        new public static void Extend(CommandsDatabase database)
        {
            // Show and hide
            database.AddCommand("ShowAll", new Func<string[], IEnumerator>(ShowAll));
            database.AddCommand("HideAll", new Func<string[], IEnumerator>(HideAll));
            // Create character
            database.AddCommand("CreateCharacter", new Action<string[]>(CreateCharacter));
            // Move character
            database.AddCommand("MoveCharacter", new Func<string[], IEnumerator>(MoveCharacter));
        }


        private static IEnumerator ShowAll(string[] data)
        {
            List<Character> characters = new();

            foreach (string name in data)
            {
                Character c = CharacterManager.Instance.GetCharacter(name, shouldCreateIfDoesNotExist: false);
                if (c != null)
                {
                    characters.Add(c);
                }
            }

            if (characters.Count == 0)
            {
                yield break;
            }
            // Convert data to paramters
            CommandParameters paramters = ConvertToCommandParameters(data);

            // Find if show will be immediate
            bool isImmediate = false;
            paramters.TryGetValue(ImmediateIdentifier, out isImmediate, defaultValue: false);

            // Show characters
            foreach (Character character in characters)
            {
                if (isImmediate)
                {
                    character.IsVisible = true;
                }
                else
                {
                    character.Show();
                }
            }

            // We wait until all characters are revealed
            if (!isImmediate)
            {
                while (characters.Any(c => c.isRevealing))
                {
                    yield return null;
                }
            }
        }
        private static IEnumerator HideAll(string[] data)
        {
            List<Character> characters = new();

            foreach (string name in data)
            {
                Character c = CharacterManager.Instance.GetCharacter(name, shouldCreateIfDoesNotExist: false);
                if (c != null)
                {
                    characters.Add(c);
                }
            }

            if (characters.Count == 0)
            {
                yield break;
            }
            // Convert data to paramters
            CommandParameters paramters = ConvertToCommandParameters(data);

            // Find if show will be immediate
            bool isImmediate = false;
            paramters.TryGetValue(ImmediateIdentifier, out isImmediate, defaultValue: false);

            // Show characters
            foreach (Character character in characters)
            {
                if (isImmediate)
                {
                    character.IsVisible = false;
                }
                else
                {
                    character.Hide();
                }
            }

            // We wait until all characters are revealed
            if (!isImmediate)
            {
                while (characters.Any(c => c.isHiding))
                {
                    yield return null;
                }
            }
        }

        private static void CreateCharacter(string[] data)
        {
            // We assume first string will be the name
            string characterName = data[0];
            // Chech if we have enable parameter
            var parameters = ConvertToCommandParameters(data);
            parameters.TryGetValue(EnableIdentifier, out bool shouldEnable, defaultValue: false);
            // Create character
            CharacterManager.Instance.CreateCharacter(characterName, shouldEnable);

        }
        private static IEnumerator MoveCharacter(string[] data)
        {
            // Get character
            string characterName = data[0];
            Character character = CharacterManager.Instance.GetCharacter(characterName);

            if (character == null)
            {
                yield break;
            }

            // Get params
            var parameters = ConvertToCommandParameters(data);

            // Get x
            parameters.TryGetValue(XIdentifier, out float x);
            // Get y
            parameters.TryGetValue(YIdentifier, out float y);
            // Get speed
            parameters.TryGetValue(SpeedIdentifier, out float speed, defaultValue: 1);
            // Get smooth
            parameters.TryGetValue(SmoothIdentifier, out bool isSmooth);
            // Get immediate
            parameters.TryGetValue(ImmediateIdentifier, out bool isImmediate);

            Vector2 position = new(x, y);

            if (isImmediate)
            {
                character.SetPosition(position);
            }
            else
            {
                yield return character.MoveToPosition(position, speed, isSmooth);
            }
        }
    }
}