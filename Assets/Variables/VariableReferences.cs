using System;
using UnityEngine;

[Serializable]
public class StringReference
{
    public bool UseConstant = true;
    public string ConstantValue;
    public StringVariable Variable;

    public string Value
    {
        get { return UseConstant ? ConstantValue : Variable.Value; }
    }
}

[Serializable]
public class FloatReference
{
    public bool UseConstant = true;
    public float ConstantValue;
    public FloatVariable Variable;

    public float Value
    {
        get { return UseConstant ? ConstantValue : Variable.Value; }
    }
}

[Serializable]
public class DoubleReference
{
    public bool UseConstant = true;
    public double ConstantValue;
    public DoubleVariable Variable;

    public double Value
    {
        get { return UseConstant ? ConstantValue : Variable.Value; }
    }
}


[Serializable]
public class BoolReference
{
    public bool UseConstant = true;
    public bool ConstantValue;
    public BoolVariable Variable;

    public bool Value
    {
        get { return UseConstant ? ConstantValue : Variable.Value; }
    }
}
