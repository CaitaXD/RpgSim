﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExpressionHandler
{
    public string _argument = "";
    public Dictionary<string, int> Operations = new Dictionary<string, int>
    {
        {"+",2},{"-",2},{"d",3},{"*",1},{"/",1}
    };
    System.Random rand = new System.Random(Mathf.RoundToInt(System.DateTime.Now.Millisecond)); //PLACEHOLDER
    public List<float> HandleExpression(string argument)
    {
        _argument = argument;
        List<int> IndexesOfOperations =  GetIndexesOfOperations(argument);
        bool isDiceRoll = argument[0] == 'd'; 
        bool existOperations = IndexesOfOperations[0] > 0;

        if (existOperations == false && !isDiceRoll) 
        return new List<float> { StringToNumber(argument) };

        List<float> Results = new List<float>();
        SortIndexesByOrderOfOperations(IndexesOfOperations, argument);
        int indexOfOperation = IndexesOfOperations[0];
        
        string operandA = GetOperandA(indexOfOperation, argument);
        string operandB = GetOperandB(indexOfOperation, argument);
        string Operator = argument[indexOfOperation].ToString();

        float A = StringToNumber(operandA);
        float B = StringToNumber(operandB);

        void ParseNANoperators(float value) //StrinToNumber will return NAN if the string is empty
        {
            A = AssginValueIfNAN(A, value); 
            B = AssginValueIfNAN(B, value);
        }
        string currentOperation;
        float newOperation;
        int leanghtOfA = A.ToString().Length;
        int leanghtOfB = B.ToString().Length;
        switch (Operator)
        {
            case "+":
                ParseNANoperators(0);
                currentOperation = (A + B).ToString();
                newOperation = HandleExpression(GetNewOperationLeftOfCurrent(argument, leanghtOfA, IndexesOfOperations) + currentOperation + GetNewOperationRightOfCurrent(argument, leanghtOfB, IndexesOfOperations))[0];
                Results.Add(newOperation);
                return Results;

            case "-":
                ParseNANoperators(0);
                currentOperation = (A - B).ToString();
                newOperation = HandleExpression(GetNewOperationLeftOfCurrent(argument, leanghtOfA, IndexesOfOperations) + currentOperation + GetNewOperationRightOfCurrent(argument, leanghtOfB, IndexesOfOperations))[0];
                Results.Add(newOperation);
                return Results;

            case "*":
                ParseNANoperators(1);
                currentOperation = (A * B).ToString();
                newOperation = HandleExpression(GetNewOperationLeftOfCurrent(argument, leanghtOfA, IndexesOfOperations) + currentOperation + GetNewOperationRightOfCurrent(argument, leanghtOfB, IndexesOfOperations))[0];
                Results.Add(newOperation);
                return Results;

            case "/":
                ParseNANoperators(1);
                if(B == 0)
                { 
                    Results.Add(float.NaN);
                    return Results;
                }
                currentOperation = (A / B).ToString();
                newOperation = HandleExpression(GetNewOperationLeftOfCurrent(argument, leanghtOfA, IndexesOfOperations) + currentOperation + GetNewOperationRightOfCurrent(argument, leanghtOfB, IndexesOfOperations))[0];
                Results.Add(newOperation);
                return Results;

            case "d":
                ParseNANoperators(1);
                for (int i = 0; i < A || i < 1; i++)
                    Results.Add(rand.Next(1, (int)B + 1));
                return Results;
        }
        return new List<float>();
    }

    private string GetNewOperationLeftOfCurrent(string argument, int lenght, List<int> IndexesOfOperations)
    {
        if (IndexesOfOperations.Count > 1)
        {
            string beforeOperator = argument.Substring(0, IndexesOfOperations[0] - lenght);
                        return beforeOperator;
        }
        return "";
    }
    private string GetNewOperationRightOfCurrent(string argument, int lenght, List<int> IndexesOfOperations)
    {
        if (IndexesOfOperations.Count > 1)
        {
            string afterOperator = argument.Substring(IndexesOfOperations[0] + lenght + 1);
            return afterOperator;
        }
        return "";
    }
    private List<int> GetIndexesOfOperations(string argument)
    {               
        List<int> opis = new List<int>();
        foreach (var oper in Operations.Keys)
        {
            var index = argument.IndexOf(oper);
            while(index >= 0)
            {
                if(index == 0 && argument[0] != 'd')
                {
                    index = argument.IndexOf(oper, index + 1);
                    if(index > 0) opis.Add(index);
                }
                else
                {
                    if(!opis.Contains(index))opis.Add(index);
                    index = argument.IndexOf(oper, index + 1);
                }
            }
        }
        opis.Sort();
        if(opis.Count > 0)return opis;
        else return new List<int>(){0};
    }
    private float StringToNumber(string arg)
    {
        List<string> numbers = new List<string> { "1","2","3","4","5","6","7","8","9","0","-",",","."};
        string str = "";
        for (int i = 0; i < arg.Length; i++)
        {
            if (numbers.Contains(arg[i].ToString()))
            {
                str = str + arg[i];
            }
        }
        if (str == "") return float.NaN;
        return float.Parse(str);
    }

    private string GetOperandA(int index,string argument)
        {
            string sub = argument.Substring(0,index);
            char sign = sub.FirstOrDefault();
            bool check = true;
            int operations = GetIndexesOfOperations(sub).Count;
            bool firstDigitisSign = Operations.ContainsKey(sign.ToString());
            if(operations <= 1 && (!firstDigitisSign && GetIndexesOfOperations(sub)[0] == 0)) return sub;
            while(check)
            {
                index--;
                check = !Operations.Keys.Any(x => x == sub[index].ToString());
                if(check == false)index++;
            }
            if(Operations.ContainsKey(sign.ToString())) sub = sign + sub.Substring(index);
            else sub = sub.Substring(index);
            return sub;
        }
        private string GetOperandB(int index,string argument)
        {
            string sub = argument.Substring(index + 1);
            char sign = sub[0];
            int operations = GetIndexesOfOperations(sub).Count;
            bool firstDigitisSign = Operations.ContainsKey(sign.ToString());
            if(operations <= 1 && (!firstDigitisSign && GetIndexesOfOperations(sub)[0] == 0)) return sub;
            bool check = true;
            index = 0;
            while(check)
            {
                check = !Operations.Keys.Any(x => x == sub[index].ToString());
                index++;
            }
            if(index <= sub.Length)sub = sub.Substring(0, index);
            if(Operations.ContainsKey(sub[sub.Length - 1].ToString())) sub = sub.Substring(0, sub.Length - 1);
            return sub;
        }
        private void SortIndexesByOrderOfOperations(List<int> OperationsIndexes, string argument)
        {
            int aux = 999;
            for (int i = 0; i < OperationsIndexes.Count; i++)
            {
                int op = OperationsIndexes[i];
                if(!Operations.ContainsKey(argument[op].ToString())) return;
                if (Operations[argument[op].ToString()] < aux)
                {
                    aux = Operations[argument[op].ToString()];
                    OperationsIndexes.Remove(op);
                    OperationsIndexes.Insert(0,op);
                }
            }
        }
        private float AssginValueIfNAN(float shoringerNumber,float value)
        {
           if(float.IsNaN(shoringerNumber)) return value; //if is not a number raturn a number
            else return shoringerNumber; //if its is a number return it
        }
}
