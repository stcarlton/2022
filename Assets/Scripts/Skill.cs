using System;
using System.Collections.Generic;

[Serializable]
public class Skill
{
    public int Id;
    public string Name;
    public string Description;
    public int CurrentRank;
    public int MaxRank;
    public Skill Left;
    public Skill Right;
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
            if(Left != null && Right != null)
            {
                return Left.Maxed && Right.Maxed;
            }
            else
            {
                return true;
            }
            
        }
    }
    public bool Maxed
    {
        get
        {
            return CurrentRank >= MaxRank;
        }
    }

    public Skill(int id, string name, string description, int maxRank, Skill left, Skill right)
    {
        Id = id;
        Name = name;
        Description = description;
        CurrentRank = 0;
        MaxRank = maxRank;
        Left = left;
        Right = right;
    }
    public void IncreaseRank()
    {

    }
}
