using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Dialogue;

namespace Dialogue.Commands
{
    public class CMD_DatabaseExtensionExamples : CMD_DatabaseExtension
    {
        new public static void Extend(CommandsDatabase database)
        {
            // Add command with no parameters
            database.AddCommand("print", new Action(PrintDefaultMessage));
            // Add command with one parameter
            database.AddCommand("PrintUserMessage", new Action<string>(PrintUserMessage));
            // Add command with multiple params
            database.AddCommand("PrintMultipleMessages", new Action<string[]>(PrintMultipleMessages));

            // Add lambdas
            database.AddCommand("lambda", new Action(() => Debug.Log("Hi, I'm a lambda")));
            database.AddCommand("lambda_1p", new Action<string>(text => Debug.Log($"Hi, I'm a lambda with text: {text}")));
            database.AddCommand("lambda_mp", new Action<string[]>(args => Debug.Log($"Hi, I'm a lambda with multiple lines: {string.Join(',', args)}")));

            // Add coroutines
            database.AddCommand("SimpleProcess", new Func<IEnumerator>(SimpleProcess));
            database.AddCommand("SimpleProcessWithParam", new Func<string, IEnumerator>(SimpleProcessWithParam));
            database.AddCommand("SimpleProcessWithMultipleParam", new Func<string[], IEnumerator>(SimpleProcessWithMultipleParam));

            // Move example
            database.AddCommand("MoveCharacter", new Func<string, IEnumerator>(MoveCharacter));

        }

        private static void PrintDefaultMessage()
        {
            Debug.Log("Default message from CMD_databaseExtensionExamples, through PrintDefaultMessage");
        }

        private static void PrintUserMessage(string message)
        {
            Debug.Log($"User message: {message}");
        }

        private static void PrintMultipleMessages(string[] lines)
        {
            int i = 1;
            foreach (string line in lines)
            {
                Debug.Log($"Message {i++}: {line}");
            }
        }

        private static IEnumerator SimpleProcess()
        {
            for (int i = 0; i <= 5; i++)
            {
                Debug.Log($"Process running... {i}");
                yield return new WaitForSeconds(1);
            }
        }

        private static IEnumerator SimpleProcessWithParam(string num)
        {
            if (int.TryParse(num, out int number))
            {
                for (int i = 0; i <= number; i++)
                {
                    Debug.Log($"Process running... {i}");
                    yield return new WaitForSeconds(1);
                }
            }
        }

        private static IEnumerator SimpleProcessWithMultipleParam(string[] lines)
        {
            foreach (string line in lines)
            {
                Debug.Log($"MyLines: {line}");
                yield return new WaitForSeconds(0.7f);
            }
        }

        private static IEnumerator MoveCharacter(string direction)
        {
            bool isLeft = direction.ToLower() == "left";
            float moveSpeed = 500f;

            // Get image transform. Should be defined somewhere else. For now, it works.
            Transform character = GameObject.Find("Frogo").transform;
            // Local
            // Debug.Log($"Local position is x: {character.localPosition.x}, y: {character.localPosition.y}, z: {character.localPosition.z}");

            // Calculate target direction
            float targetX = isLeft ? -300f : 300f;

            // Calculate current position
            float currentX = character.localPosition.x;

            while (Mathf.Abs(targetX - currentX) > 0.1f)
            {
                // Debug.Log($"Character is moving to the {(isLeft ? "left" : "right")}: current {currentX} / target {targetX}");
                currentX = Mathf.MoveTowards(currentX, targetX, moveSpeed * Time.deltaTime);
                character.localPosition = new Vector3(currentX, character.localPosition.y, character.localPosition.z);
                yield return null;
            }


        }
    }
}
