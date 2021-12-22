using System.Collections;
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

    // Breaks the command string into to varius operation, checking operation precedence
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

        //Neutral Member of Group, Varies by operation
        void SetNeutralMemberValue(float value) 
        {
            A = AssignIfNeutralMember(A, value); 
            B = AssignIfNeutralMember(B, value);
        }
        string currentOperation;
        float newOperation;
        int leanghtOfA = A.ToString().Length;
        int leanghtOfB = B.ToString().Length;

        switch (Operator)
        {
            case "+":
                SetNeutralMemberValue(0);
                currentOperation = (A + B).ToString();
                newOperation = HandleExpression(GetNewLeftOperation(argument, leanghtOfA, IndexesOfOperations) + currentOperation + GetNewRightOperation(argument, leanghtOfB, IndexesOfOperations))[0];
                Results.Add(newOperation);
                return Results;

            case "-":
                SetNeutralMemberValue(0);
                currentOperation = (A - B).ToString();
                newOperation = HandleExpression(GetNewLeftOperation(argument, leanghtOfA, IndexesOfOperations) + currentOperation + GetNewRightOperation(argument, leanghtOfB, IndexesOfOperations))[0];
                Results.Add(newOperation);
                return Results;

            case "*":
                SetNeutralMemberValue(1);
                currentOperation = (A * B).ToString();
                newOperation = HandleExpression(GetNewLeftOperation(argument, leanghtOfA, IndexesOfOperations) + currentOperation + GetNewRightOperation(argument, leanghtOfB, IndexesOfOperations))[0];
                Results.Add(newOperation);
                return Results;

            case "/":
                SetNeutralMemberValue(1);
                if(B == 0)
                { 
                    Results.Add(float.NaN);
                    return Results;
                }
                currentOperation = (A / B).ToString();
                newOperation = HandleExpression(GetNewLeftOperation(argument, leanghtOfA, IndexesOfOperations) + currentOperation + GetNewRightOperation(argument, leanghtOfB, IndexesOfOperations))[0];
                Results.Add(newOperation);
                return Results;

            case "d":
                A = AssignIfNeutralMember(A, 1);
                if (float.IsNaN(B)) return Results;
                for (int i = 0; i < A || i < 1; i++) Results.Add(rand.Next(1, (int)B + 1));
                return Results;
        }
        return new List<float>();
    }
    /*
     *  Gets the new operation left of the current one, if one exists.
     *  Else returns an empty string.
     */
    private string GetNewLeftOperation(string argument, int lenght, List<int> IndexesOfOperations)
    {
        if (IndexesOfOperations.Count > 1)
        {
            string beforeOperator = argument.Substring(0, IndexesOfOperations[0] - lenght);
                        return beforeOperator;
        }
        return string.Empty;
    }
    /*
    *  Gets the new operation right of the current one, if one exists.
    *  Else returns an empty string.
    */
    private string GetNewRightOperation(string argument, int lenght, List<int> IndexesOfOperations)
    {
        if (IndexesOfOperations.Count > 1)
        {
            string afterOperator = argument.Substring(IndexesOfOperations[0] + lenght + 1);
            return afterOperator;
        }
        return string.Empty;
    }
    /*
    *  Searches for the indexes of all the operations within the string
    */
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

    //Extracts a number for an given string and NaN if theres none
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
    /*
    *  Returns a substring with the number left of the current operation
    */
    private string GetOperandA(int index,string argument)
    {
        string substring = argument.Substring(0, index);
        char sign = substring.FirstOrDefault();
        int operations = GetIndexesOfOperations(substring).Count;

        bool oneOp = OperandContainsAtMostOneOperation(substring, sign, operations);
        if (oneOp) return substring;

        // Finds the index where the current operation ends
        bool check = true;
        while (check)
        {
            index--;
            check = !Operations.Keys.Any(x => x == substring[index].ToString());
            if(check == false)index++;
        }
        if(Operations.ContainsKey(sign.ToString())) substring = sign + substring.Substring(index);
        else substring = substring.Substring(index);
        return substring;
    }
    /*
    *  Returns a substring with the number right of the current operation
    */
    private string GetOperandB(int index, string argument)
    {
        string substring = argument.Substring(index + 1);
        if (substring.Length == 0) substring = " ";
        char sign;
        sign = substring[0];
        int operations = GetIndexesOfOperations(substring).Count;

        bool oneOp = OperandContainsAtMostOneOperation(substring, sign, operations);
        if (oneOp) return substring;

        // Finds the index where the current operation ends
        bool check = true;
        index = 0;
        while (check)
        {
            check = !Operations.Keys.Any(x => x == substring[index].ToString());
            index++;
        }
        if (index <= substring.Length) substring = substring.Substring(0, index);
        if (Operations.ContainsKey(substring[substring.Length - 1].ToString())) substring = substring.Substring(0, substring.Length - 1);
        return substring;
    }
    /*
    *  Returns true if the given substring contains at most one operation, expect in the cases were the first digit represents a sign
    */
    private bool OperandContainsAtMostOneOperation(string substring, char sign, int operations)
    {
        bool firstDigitisSign = Operations.ContainsKey(sign.ToString());
        bool firstDigitIsOperation = (!firstDigitisSign && GetIndexesOfOperations(substring)[0] == 0);
        bool operandContainsAtMostOneOperation = operations <= 1 && firstDigitIsOperation;
        return operandContainsAtMostOneOperation;
    }

    /*
    *  Given a list of indexs of the operations of the given string, sorts it by said order of operations
    */
    private void SortIndexesByOrderOfOperations(List<int> OperationsIndexes, string argument)
        {
            int aux = int.MaxValue;
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

        // Assign the value of the Neutral memeber, NaN is representing the neutral Member
        private float AssignIfNeutralMember(float shoringerNumber,float value)
        {
           if(float.IsNaN(shoringerNumber)) return value;
            else return shoringerNumber;
        }
}
