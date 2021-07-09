
using UnityEngine;
using System.IO;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.UI;

public class ImportScript : MonoBehaviour
{
    public delegate void ImportAction();
    public static event ImportAction OnImport;
    private string importFilePath;
    string savedFilePath;
    string folderName;
    public void EntetieList()
    {
        folderName = EntetieListName.MonsterManual.ToString();
        importFilePath = SFB.StandaloneFileBrowser.OpenFilePanel("Select the txt file", "", "", false)[0];  
        savedFilePath = Application.streamingAssetsPath+ @"\" + folderName + ".json";
        Debug.Log(savedFilePath);
        if (File.Exists(savedFilePath))
        {
            EntetieList entetieList = new EntetieList(savedFilePath);
            entetieList.Import(importFilePath);
            var jString = Newtonsoft.Json.JsonConvert.SerializeObject(entetieList.listFields);
            File.WriteAllText(savedFilePath, jString);     
        }
        else
        {
            string import = File.ReadAllText(importFilePath);
            File.WriteAllText(savedFilePath, import);
            new EntetieList(importFilePath);
        }
        OnImport?.Invoke();
    }
}
