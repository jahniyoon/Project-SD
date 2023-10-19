using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnyClassContainer<T> where T : class
{
    public T classType;

    public void SetClassType(T type)
    {
        classType = type;
    }
}
