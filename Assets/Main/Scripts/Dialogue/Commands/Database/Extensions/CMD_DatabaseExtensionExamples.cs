using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Dialogue;

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
}
