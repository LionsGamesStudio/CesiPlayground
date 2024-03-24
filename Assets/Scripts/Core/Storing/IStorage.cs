using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStorage 
{
    public void Store<T>(string filepath, T value) where T : class;
    public T Get<T>(string filepath) where T : class;
    public T GetFromString<T>(string text) where T : class;
}
