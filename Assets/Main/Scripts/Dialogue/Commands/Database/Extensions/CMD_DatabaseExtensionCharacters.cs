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

        new public static void Extend(CommandsDatabase database)
        {
            database.AddCommand("ShowAll", new Func<string[], IEnumerator>(ShowAll));
            database.AddCommand("HideAll", new Func<string[], IEnumerator>(HideAll));
        }


        public static IEnumerator ShowAll(string[] data)
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
        public static IEnumerator HideAll(string[] data)
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
    }
}