using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Handles saving and loading

public class FileManager
{
    // from absolute path
    public static List<string> ReadTextFile(string filePath, bool includeBlankLines = true)
    {
        // if provided path is relative, we add root
        if (!filePath.StartsWith("/"))
        {
            filePath = FilePaths.root + filePath;
        }
        List<string> lines = new List<string>();

        try
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (includeBlankLines || !string.IsNullOrWhiteSpace(line))
                    {
                        lines.Add(line);
                    }
                }
            }
        }
        catch (FileNotFoundException ex)
        {
            Debug.Log($"File not found: {ex.FileName}");
        }
        return lines;
    }

    // from resources, check there is an asset and call read method
    public static List<string> ReadTextAsset(string filePath, bool includeBlankLines = true)
    {
        TextAsset asset = Resources.Load<TextAsset>(filePath);
        if (asset == null)
        {
            Debug.LogError($"Asset not found: '{filePath}' ");
            return null;
        }

        return ReadTextAsset(asset, includeBlankLines);
    }

    // actual read the text asset
    public static List<string> ReadTextAsset(TextAsset asset, bool includeBlankLines = true)
    {
        List<string> lines = new List<string>();
        using (StringReader sr = new StringReader(asset.text))
        {
            // check wheter there's a following character or return -1
            while (sr.Peek() > -1)
            {
                string line = sr.ReadLine();
                if (includeBlankLines || !string.IsNullOrWhiteSpace(line))
                {
                    lines.Add(line);
                }
            }
        }
        return lines;
    }
}
