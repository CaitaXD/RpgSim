using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpressionHandler
{
    public Dictionary<string, int> Operations = new Dictionary<string, int>
    {
        {"+",2},{"-",2},{"d",3}
    };
    System.Random rand = new System.Random(Mathf.RoundToInt(System.DateTime.Now.Millisecond)); //PLACEHOLDER
    public List<float> HandleExpression(string arg)
    {
        float A =0,B=0,C=0;
        List<float> results = new List<float>();
        List<int> OperationsIndex()
        {
            List<int> opi = new List<int>();
            foreach (var oper in Operations.Keys)
            {
                var index = arg.IndexOf(oper);
                while(index >= 0)
                {
                    if(index == 0)
                    {
                        index = arg.IndexOf(oper, index + 1);
                        if(index > 0) opi.Add(index);
                    }
                    else
                    {
                        if(!opi.Contains(index))opi.Add(index);
                        index = arg.IndexOf(oper, index + 1);
                    }
                }
            }
            opi.Sort();
            if(opi.Count > 0)return opi;
            else return null;
        }
        string operation;
        if(OperationsIndex() != null) operation = arg[OperationsIndex()[0]].ToString();
        else 
        {
            results.Add(StringNumber(arg));
            return results;
        }
        A = StringNumber(arg.Substring(0, OperationsIndex()[0]));
        if (OperationsIndex().Count > 1)
        {
            B = StringNumber(arg.Substring(OperationsIndex()[0]+1, OperationsIndex()[1] - OperationsIndex()[0] - 1));
            C = HandleExpression(arg.Substring(OperationsIndex()[1]))[0];
        }
        else 
        {
            B = StringNumber(arg.Substring(OperationsIndex()[0]+1));
        }
        switch (operation)
        {
            case "+":
            results.Add(A + B + C);
            return results;
            case "-":
            results.Add(A - B + C);
            return results;
            case "d":
            for(int i = 0; i < A || i < 1; i++)
            results.Add(rand.Next(1, (int)B + 1));
            return results; 
        }
    return new List<float>();
    }
    private int StringNumber(string arg)
    {
        List<string> numbers = new List<string> { "1","2","3","4","5","6","7","8","9","0","-"};
        string str = "";
        for (int i = 0; i < arg.Length; i++)
        {
            if (numbers.Contains(arg[i].ToString()))
            {
                str = str + arg[i];
            }
        }
        if (str == "") return 0;;
        return int.Parse(str);
    }
}
