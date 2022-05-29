using System;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] TMP_Text _levelText;
    [SerializeField] TMP_Text _countDownText;
    [SerializeField] FloatingMessage _floatingTextPrefab;
    [SerializeField] Canvas _floatingScoreCanvas;

    void Start()
    {
        Zombie.Hit += Zombie_Hit;
        Player.Hit += Player_Hit;
    }
    void OnDestroy()
    {
        Zombie.Hit -= Zombie_Hit;
        Player.Hit -= Player_Hit;
    }

    void Zombie_Hit(Zombie zombie)
    {
        var floatingText = Instantiate(_floatingTextPrefab,
            zombie.transform.position + new Vector3(0,1,0),
            _floatingScoreCanvas.transform.rotation,
            _floatingScoreCanvas.transform);

        if (zombie.Burning)
        {
            floatingText.SetValues(zombie.LastHitTaken.ToString(), new Color32(255, 165, 0, 255), 36);
        }
        else if (zombie.LastCrit)
        {
            floatingText.SetValues(zombie.LastHitTaken.ToString(), new Color32(255, 255, 0, 255),72);
        }
        else
        {
            floatingText.SetValues(zombie.LastHitTaken.ToString(), new Color32(255, 255, 255, 255),36);
        }
    }
    void Player_Hit(Player player)
    {
        var floatingText = Instantiate(_floatingTextPrefab,
            player.transform.position,
            _floatingScoreCanvas.transform.rotation,
            _floatingScoreCanvas.transform);
        if (player.LastHitTaken == 0)
        {
            floatingText.SetValues("DODGE", new Color32(0, 0, 255, 255),36);
        }
        else if(player.LastHitTaken < 0)
        {
            floatingText.SetValues((-player.LastHitTaken).ToString(), new Color32(255, 0, 0, 255),36);
        }
        else
        {
            floatingText.SetValues(player.LastHitTaken.ToString(), new Color32(0, 255, 0, 255), 36);
        }
    }
    public void RefreshText(int level, int countDown)
    {
        _levelText.text = level.ToString();
        _countDownText.text = countDown.ToString();
    }
}
