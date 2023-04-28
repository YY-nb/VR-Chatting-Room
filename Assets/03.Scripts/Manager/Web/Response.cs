using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Response<T>
{
    public int code;
    public string message;
    public T data;
}
