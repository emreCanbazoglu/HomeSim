using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class ActionReader : MonoBehaviour 
{
    static ActionReader _instance;

    public static ActionReader Instance { get { return _instance; } }

    string _filePath;

    int _curLineCount;

    void Awake()
    {
        _instance = this;
    }

    void OnDestroy()
    {
        _instance = null;
    }

    public void SetFilePath(string path)
    {
        _filePath = path;

        StartCoroutine(ReadActionText());
    }

    IEnumerator ReadActionText()
    {
        string[] lineArr;

        lineArr = File.ReadAllLines(_filePath);

        _curLineCount = lineArr.Length;

        while(true)
        { 
            do
            {
                lineArr = File.ReadAllLines(_filePath);
                yield return null;

            }while(lineArr.Length == _curLineCount);

            _curLineCount++;

            ParseNewAction(lineArr[lineArr.Length - 1]);
            
            yield return null;
        }
    }

    void ParseNewAction(string newActionStr)
    {
        ActionEnum newAction = IdentifyObjectEnum(newActionStr);
        Debug.Log("new action: " + newAction);
        HomeController.Instance.ProcessAction(newAction);
    }

    public static ActionEnum IdentifyObjectEnum(string objectname)
    {
        ActionEnum type = (ActionEnum)Enum.Parse(typeof(ActionEnum), objectname);
        return type;
    }
}
