using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ResultManager : MonoBehaviour
{
    Dictionary<string, int> Operations;
    [SerializeField] InputField commandField;
    System.Random rand = new System.Random(Mathf.RoundToInt(System.DateTime.Now.Millisecond)); //PLACEHOLDER
    public List<float> ResultsTray;
    ExpressionHandler expressionHandler = new ExpressionHandler();
    [SerializeField] ResultsUI resultsUI;
    void Awake()
    {
        commandField = commandField ? commandField: gameObject.GetComponent<InputField>();
    }
    public void GiveCommand()
    {
        rand = new System.Random(Mathf.RoundToInt(System.DateTime.Now.Millisecond)); //PLACEHOLDER
        CheckCommand(commandField.text);
    }
    public string CheckCommand(string command)
    {
        for (int i = 0; i < command.Length; i++)
        {
            foreach (var operation in Operations)
            {
                if(command[i].ToString() == operation.Key)
                {
                    ResultsTray = expressionHandler.HandleExpression(command);
                    return ResultsTray.FirstOrDefault().ToString();
                }
            }
        }
        return command;
    }
}
