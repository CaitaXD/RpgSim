using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionConsole : MonoBehaviour
{
    public string command;
    public Vector3 target;
    public EntetieScript entetieScript;
    public float result;
    System.Random rand = new System.Random(Mathf.RoundToInt(System.DateTime.Now.Millisecond));

    private void Start()
    {
    }
    private void Update()
    {
        rand = new System.Random(Mathf.RoundToInt(System.DateTime.Now.Millisecond));
        result = CheckCommand(command);
    }

    private float CheckCommand(string command)
    {
        int openingBracketIndex = command.IndexOf('(');
        int closingBracketIndex = command.LastIndexOf(')');
        if ((command.IndexOf('+') != -1 || command.IndexOf('-') != -1) && (openingBracketIndex == -1 && closingBracketIndex == -1))
        {
            return HandleExpression(command);
        }
        else if (openingBracketIndex == -1 || closingBracketIndex == -1)
        {
            return StringNumber(command);
        }
        if (command == "") return Mathf.NegativeInfinity;
        string func = command.Substring(0,openingBracketIndex);
        string arg = command.Substring(openingBracketIndex + 1, command.Length - (openingBracketIndex + 1));
        if (arg.Length == arg.LastIndexOf(')') + 1) arg = arg.Substring(0, arg.Length - 1);
        func = func.Trim();
        switch (func)
        {
            case "Contest":
                int commaIndex = arg.IndexOf(',');
                if (commaIndex == -1) return Mathf.NegativeInfinity;
                string fArg = arg.Substring(0, commaIndex);
                var fValue = HandleExpression(fArg);
                string sArg = arg.Substring(commaIndex + 1, arg.Length - (commaIndex + 1));
                var sValue = HandleExpression(sArg);
                if (fValue > sValue) return 1;
                else return 0;
            case "Roll":
                int die = StringNumber(arg);
                if (die == -0) return Mathf.NegativeInfinity;
                return rand.Next(1, die + 1);
            case "Stat":
                if (entetieScript == null) return Mathf.NegativeInfinity;
                string field = entetieScript.Entetie.fields[arg];
                return StringNumber(field);
            default:
                if (command.IndexOf('+') != -1 || command.IndexOf('-') != -1)
                {
                    return HandleExpression(command);
                }
                else
                return StringNumber(command);
        }
    }

    private float HandleExpression(string arg)
    {
        string left, right;
        int exppressionIndex = ExpressionIndex(Expression());
        if (exppressionIndex == -1) return ResolveStep(arg);

        left = arg.Substring(0, exppressionIndex);
        
        if (arg.IndexOf('+',exppressionIndex+1) == -1 && arg.IndexOf('-', exppressionIndex+1) == -1)
        {
            right = arg.Substring(left.Length);
        }
        else return ResolveStep(left);

        float A = ResolveStep(left);
        float B = ResolveStep(right);

        string recursion;
        if (left.Length + right.Length < arg.Length)
        {
            recursion = arg.Substring(left.Length + right.Length);
            float recursiveValue = HandleExpression(recursion);
            return A + B + recursiveValue;
        }
        else
        {
            return A + B;
        }
        float ResolveStep(string number)
        {
            if (number[0] != '+' && number[0] != '-')
            {
                return CheckCommand(number);
            }
            else if (number[0] == '+')
            {
                return CheckCommand(number.Substring(1));
            }
            else if (number[0] == '-')
            {
                return -CheckCommand(number.Substring(1));
            }
            return Mathf.NegativeInfinity;
        }
        char Expression()
        {
            int indexE1 = arg.IndexOf('-');
            int indexE2 = arg.IndexOf('+');
            if (indexE1 != -1 || indexE2 != -1)
            {
                if (indexE1 == -1) return '+';
                if (indexE2 == -1) return '-';
                if (indexE1 < indexE2) return '-';
                else return '+';
            }
            return '~';
        }
        int ExpressionIndex(char expression)
        {
            if (arg[0] == '+' || arg[0] == '-')
                exppressionIndex = arg.IndexOf(expression, 1);
            else
                exppressionIndex = arg.IndexOf(expression);
            return exppressionIndex;
        }
    }
    private int StringNumber(string arg)
    {
        List<char> numbers = new List<char> { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
        int number = 0;
        string str = "";
        for (int i = 0; i < arg.Length; i++)
        {
            if (numbers.Contains(arg[i]))
            {
                str = str + arg[i];
            }
        }
        if (str == "") return 0;
        number = int.Parse(str);
        return number;
    }
}
