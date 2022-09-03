using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Skill
{
    public int Id;
    public string Name;
    public string Description;
    public int CurrentRank;
    public int MaxRank;
    
    private List<Skill> _dependencies;
    
    public string Display
    {
        get
        {
            return CurrentRank.ToString() + " / " + MaxRank.ToString();
        }
    }
    public bool Unlocked
    {
        get
        {
            bool tempLock = true;
            if(_dependencies == null)
            {
                Debug.Log("No dependencies");
            }
            else
            {
                foreach (Skill s in _dependencies)
                {
                    tempLock = tempLock && s.Maxed;
                }
            }
            return tempLock;
        }
    }
    public bool Maxed
    {
        get
        {
            return CurrentRank >= MaxRank;
        }
    }

    public Skill(int id, string name, string description, int maxRank, List<Skill> dependencies)
    {
        Id = id;
        Name = name;
        Description = description;
        CurrentRank = 0;
        MaxRank = maxRank;
        _dependencies = dependencies;
    }
    public void IncreaseRank()
    {

    }
}
