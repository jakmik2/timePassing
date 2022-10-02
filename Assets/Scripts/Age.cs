using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AgeEnum
{
    Baby,
    Young,
    Adult,
    Old
}

public class Age
{
    public AgeEnum state;

    public Age()
    {
        state = AgeEnum.Baby;
    }

    public void Next()
    {
        switch (state)
        {
            case AgeEnum.Baby:
                state = AgeEnum.Young;
                break;
            case AgeEnum.Young:
                state = AgeEnum.Adult;
                break;
            case AgeEnum.Adult:
                state = AgeEnum.Baby;
                break;
            case AgeEnum.Old:
                state = AgeEnum.Baby;
                break;
            default:
                state = AgeEnum.Baby;
                break;
        }
    }
}