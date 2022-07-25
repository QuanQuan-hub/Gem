using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InputManager 
{
    //单例模式
    private static InputManager instance;

    Dictionary<ActionEnum, KeyCode> Action2Key;
    Dictionary<ActionEnum, KeyCode> Action2KeyCopy;
    public static InputManager Instance 
    { get
      {
          if (instance == null)
              instance = new InputManager();
          return instance;
      }
      set { instance = value; } 
    }
    public InputManager(){ Init();}

    private void Init()
    {
        Action2Key = new Dictionary<ActionEnum, KeyCode>();
        //读json表,通过Action2KeyConfig表初始化字典
        StreamReader sr = new StreamReader(CommonValue.Action2KeyConfigPath);
        Action2Key = JsonConvert.DeserializeObject<Dictionary<ActionEnum, KeyCode>>(sr.ReadToEnd());
        Action2KeyCopy = Action2Key;
        sr.Close();
    }
    /// <summary>
    /// 根据行动枚举获取对应的KeyCode
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public KeyCode GetActionKey(ActionEnum action) 
    {
        if (Action2Key.ContainsKey(action))
        {
            return Action2Key[action];
        }
        return KeyCode.None;
    }
    /// <summary>
    /// 通过KeyCode获取对应的行为
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public ActionEnum GetKeyAction(KeyCode key)
    {
        foreach (KeyValuePair<ActionEnum, KeyCode> pair in Action2Key)
        {
            if (pair.Value == key)
            {
                return pair.Key;
            }
        }
        return ActionEnum.None;
    }
    public bool IsKeyDown(ActionEnum action)
    {
        return Input.GetKeyDown(GetActionKey(action));
    }
    public bool IsKeyUp(ActionEnum action)
    {
        return Input.GetKeyUp(GetActionKey(action));
    }
    public bool IsKeyHold(ActionEnum action)
    {
        return Input.GetKey(GetActionKey(action));
    }
    public void ModifyKey(ActionEnum action,KeyCode key)
    {
        if (!Action2Key.ContainsKey(action)) return;
        if (Action2KeyCopy.ContainsValue(key))
        {
            foreach (KeyValuePair<ActionEnum,KeyCode> pair in Action2KeyCopy)
            {
                if (pair.Value == key)
                {
                    Action2KeyCopy[pair.Key] = KeyCode.None;
                    Debug.Log(string.Format("该键位已由{0}占有", pair.Key));
                    break;
                }
            }
        }
        Action2KeyCopy[action] = key;
    }
    public void SaveActionKey()
    {
        Action2Key = Action2KeyCopy;
        string jsonString = JsonConvert.SerializeObject(Action2Key, Formatting.Indented);

        string jsonPath = CommonValue.Action2KeyConfigPath;
        if (!File.Exists(jsonPath))
        {
            File.CreateText(jsonPath).Dispose();
        }
        File.WriteAllText(jsonPath, jsonString, System.Text.Encoding.UTF8);
        Debug.Log(jsonString);
    }
}