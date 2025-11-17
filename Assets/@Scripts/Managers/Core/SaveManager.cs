using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Data.Contents;

public class SaveManager
{
    private bool _isInitialized = false;

    private int _currentSlot = 0;
    public int CurrentSlot => _currentSlot;
    private string SavePath => Path.Combine(Application.persistentDataPath, $"SaveData{_currentSlot}.json");

    public SaveData Data = new ();


    readonly List<ISaveSection> _sections = new();


    public void Init()
    {
        if (_isInitialized)
            return;

        _isInitialized = true;
    }

    public void Register(ISaveSection section)
    {
        if (section == null) return;
        if (!_sections.Contains(section))
            _sections.Add(section);
    }

    public void Unregister(ISaveSection section)
    {
        _sections.Remove(section);
    }

    public void SetSlot(int slot)
    {
        _currentSlot = Mathf.Clamp(slot,0,2);
    }

    public void Save()
    {
        foreach (var s in _sections)
            s.Capture(ref Data);

        var json = JsonUtility.ToJson(Data, true);

        try
        {
            var temp = SavePath + ".tmp";
            File.WriteAllText(temp, json);
            if (File.Exists(SavePath)) File.Replace(temp, SavePath, null);
            else File.Move(temp, SavePath);
        }
        catch (Exception e)
        {
            Debug.LogError($"[SaveManager] Save failed: {e}");
            return;
        }
    }

    public void DeleteData()
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
        }
        Data = new SaveData();
    }

    public SaveData LoadData()
    {
        if (!File.Exists(SavePath))
        {
            return null;
        }

        string json = File.ReadAllText(SavePath);
        Data = JsonUtility.FromJson<SaveData>(json);
        return Data;
    }


}
