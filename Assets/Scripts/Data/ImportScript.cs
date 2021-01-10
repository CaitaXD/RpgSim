
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

public class ImportScript : MonoBehaviour
{
    string importFilePath;
    string savedFilePath;
    [SerializeField] LeftMenuUI leftUI;
    public void Import()
    {    
        string folderName = transform.parent.parent.name;
        importFilePath = SFB.StandaloneFileBrowser.OpenFilePanel("GetMonsterManual", "", "", false)[0];
        string import = File.ReadAllText(importFilePath);
        savedFilePath = Application.streamingAssetsPath+ @"\" + folderName + ".json";

        if (File.Exists(savedFilePath))
        {
            leftUI.monsterManual.Import(importFilePath);
            var jString = Newtonsoft.Json.JsonConvert.SerializeObject(leftUI.monsterManual.listFields);
            File.WriteAllText(savedFilePath, jString);
        }
        else
        {
            File.WriteAllText(savedFilePath, import);
            leftUI.monsterManual = new EntetieList(importFilePath);
            leftUI.DrawMonsterManual(savedFilePath);
        } 
    }
}
