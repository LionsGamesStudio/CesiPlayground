using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStorage 
{
    /// <summary>
    /// Store a content in a file
    /// </summary>
    /// <typeparam name="T">Type of the content to store</typeparam>
    /// <param name="filepath">Filepath where storing the content</param>
    /// <param name="value">The content to stock</param>
    public void Store<T>(string filepath, T value) where T : class;

    /// <summary>
    /// Get a content from a file
    /// </summary>
    /// <typeparam name="T">Type of the content to get</typeparam>
    /// <param name="filepath">Filepath where content is stored</param>
    /// <returns>The content stored</returns>
    public T Get<T>(string filepath) where T : class;

    /// <summary>
    /// Get a content from a string
    /// </summary>
    /// <typeparam name="T">Type of the content to get</typeparam>
    /// <param name="text">String containing the content</param>
    /// <returns>The content</returns>
    public T GetFromString<T>(string text) where T : class;
}
