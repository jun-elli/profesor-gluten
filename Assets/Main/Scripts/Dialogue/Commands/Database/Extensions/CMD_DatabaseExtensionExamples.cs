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
    }

    private static void PrintDefaultMessage()
    {
        Debug.Log("Default message from CMD_databaseExtensionExamples, through PrintDefaultMessage");
    }
}
