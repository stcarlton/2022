using TMPro;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    [SerializeField] SkillButton[] _buttons;
    [SerializeField] Spawner[] _spawners;
    public int SkillPoints = 0;
    public Skill[] Skills;
    public int Level = 1;
    public int CountDown = 22;
    Player _player;
    CanvasGroup _canvasGroup;
    HUD _hud;

    void OnDestroy()
    {
        Zombie.Death -= Zombie_Death;
    }
    void Start()
    {
        Skills = new Skill[21];
        _player = FindObjectOfType<Player>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _hud = FindObjectOfType<HUD>();
        Zombie.Death += Zombie_Death;

        string[] Names = { "Rapid Fire", "Ninja", "Head Shot", "Caliber", "Stamina", "Spread Shot"
                , "Velocity", "Lightweight", "Head Trauma", "Armor", "Armor Shred"
                , "Bullet Time", "Procrastination","Dracula","Pyro"
                , "Weaken", "Stagger", "Ricochet"
                , "Sustain", "Lifeline"
                , "Golden God"};

        string[] Descriptions =
        {
            "Increase fire rate by 20% for each point."
            , "Increase dodge chance by 10% for each point."
            , "Increase chance of a head shot by 10% for each point."
            , "Increase shot power by 20% for each point."
            , "Increase Health by 20% for each point."
            , "Add additional spread shot at half damage for each point."
            
            , "Increase bullet velocity by 20% for each point."
            , "Increase movement speed by 20% for each point."
            , "Increase head shot damage by 40% for each point."
            , "Increase armor by 20% for each point."
            , "Increase armor shred by 10% for each point."

            , "Slow down time by 10% for each point."
            , "Spread 10% of damage taken across 5 seconds for each point."
            , "Heal 2% of damage dealt for each point."
            , "Enemies take an additional 20% fire damage over 3 seconds"

            , "Each bullet increases future damage taken by enemies by 10%."
            , "Bullets now stagger enemies."
            , "Bullets now ricochet off surfaces."

            , "Eliminate delay before regenerating health and double the rate."
            , "Become Invulnerable for 3 seconds once every 2 minutes instead of dying."

            , "Increase flat bonuses by 1% for every level past 60."
        };

        int[] MaxRanks = { 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 1, 1, 1, 1, 1, 1};

        int layerSize = 6;
        int layerCounter = 1;
        for (int i = 0; i < Skills.Length; i++)
        {
            if (i < 6)
            {
                Skills[i] = new Skill(i, Names[i], Descriptions[i], MaxRanks[i], null, null);
            }
            else
            {
                Skills[i] = new Skill(i, Names[i], Descriptions[i], MaxRanks[i], Skills[i-layerSize - 1], Skills[i-layerSize]);
            }
            layerCounter++;
            if (layerCounter > layerSize)
            {
                layerSize--;
                layerCounter = 1;
            }
            _buttons[i].ThisSkill = Skills[i];
        }
        _hud.RefreshText(Level, CountDown);
        Refresh();
    }
    public void Refresh()
    {
        foreach(SkillButton sb in _buttons)
        {
            sb.Refresh();
        }
        _player.LevelUp();
        if(SkillPoints <= 0)
        {
            UnPauseGame();
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
        } else
        {
            PauseGame();
            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = true;
        }
    }
    public void LevelUp()
    {
        Level++;
        CountDown = (int)(20 * Mathf.Pow(1.01f,Level));
        if(Level % 20 == 2)
        {
            int randomIndex = Random.Range(0, _spawners.Length);
            var randSpawner = _spawners[randomIndex];
            randSpawner.SpawnItem();
        }
        if(Level < 83)
        {
            SkillPoints++;
        }
        _hud.RefreshText(Level, CountDown);
        Refresh();
    }
    void PauseGame()
    {
        Time.timeScale = 0;
    }
    void UnPauseGame()
    {
        Time.timeScale = 1;
    }
    void Zombie_Death(Zombie zombie)
    {
        CountDown--;
        _hud.RefreshText(Level, CountDown);
        if (CountDown <= 0)
        {
            LevelUp();
        }
    }
}
