using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    [SerializeField] SkillButton[] _buttons;
    [SerializeField] Spawner[] _spawners;
    [SerializeField] TMP_Text _message;
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
        Skills = new Skill[22];
        _player = FindObjectOfType<Player>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _hud = FindObjectOfType<HUD>();
        Zombie.Death += Zombie_Death;

        string[] Names = {
            "Head Shot"
            , "Lightweight"
            , "Head Trauma"
            , "Rapid Fire"
            , "Weaken"
            , "Velocity"
            , "Bullet Time"

            , "Stamina"
            , "Procrastination"
            , "Armor"
            , "Sustain"
            , "Ninja"
            , "Lifeline"
            , "Dracula"

            , "Caliber"
            , "Armor Shred"
            , "CQC"
            , "Pyro"
            , "Spread Shot"
            , "Ricochet"
            , "Stagger"

            , "Golden God"};

        string[] Descriptions =
        {
            "+10% head shot chance"
            , "+20% movement speed"
            , "+40% head shot damage"
            , "+20% fire rate"
            , "+1% future damage p/bullet"
            , "+20% bullet velocity"
            , "Time is slowed down 50%"

            , "+20% health"
            , "10% of damage is spread 5 seconds"
            , "+20% armor"
            , "Reduces regeneration delay by 20%"
            , "+10% dodge chance"
            , "Avoid dying once per 90/60/30 sec"
            , "Heal 10% of damage dealt"

            , "+20% shot power"
            , "+10% armor shred"
            , "+10% damage within 5 meters"
            , "+20% burn damage over 3 seconds (ignores armor)"
            , "+spread shot at half damage p/pont; full damage @rank 3"
            , "Bullets ricochet +1 time"
            , "Bullets now stagger enemies"

            , "+1% to all flat bonuses past level 60"
        };

        int[] MaxRanks = {
                5, 5, 5, 5, 3, 3, 1
                , 5, 5, 5, 5, 3, 3, 1
                , 5, 5, 5, 5, 3, 3, 1
                , 1 };
        List<int>[] Dependencies = new List<int>[22];
        for(int i = 0; i < 22; i++)
        {
            Dependencies[i] = new List<int>();
        }
        Dependencies[2].Add(0);
        Dependencies[3].Add(1);
        Dependencies[4].Add(2);
        Dependencies[5].Add(3);
        Dependencies[6].Add(4);
        Dependencies[6].Add(5);
        Dependencies[9].Add(7);
        Dependencies[10].Add(8);
        Dependencies[11].Add(9);
        Dependencies[12].Add(10);
        Dependencies[13].Add(11);
        Dependencies[13].Add(12);
        Dependencies[16].Add(14);
        Dependencies[17].Add(15);
        Dependencies[18].Add(16);
        Dependencies[19].Add(17);
        Dependencies[20].Add(18);
        Dependencies[20].Add(19);
        Dependencies[21].Add(6);
        Dependencies[21].Add(13);
        Dependencies[21].Add(20);

        List<Skill>[] SkillDependencies = new List<Skill>[22];
        for(int i = 0; i < 22; i++)
        {
            SkillDependencies[i] = new List<Skill>();
        }


        for (int i = 0; i < Skills.Length; i++)
        {
            foreach(int k in Dependencies[i])
            {
                SkillDependencies[i].Add(Skills[k]);
            }
            Skills[i] = new Skill(i, Names[i], Descriptions[i], MaxRanks[i], SkillDependencies[i]);
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
            _message.text = "";
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
            _message.text = "A power up has spawned in an alleyway.";
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
