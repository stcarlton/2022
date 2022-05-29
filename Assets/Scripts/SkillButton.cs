using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public SkillTree ThisSkillTree;
    public Skill ThisSkill;
    public Button ThisButton;
    [SerializeField] Text _text;
    [SerializeField] Text _title;

    private void Awake()
    {
        ThisButton = GetComponent<Button>();
        ThisButton.onClick.AddListener(RankUp);
        _text.text = ThisSkill.Display;
        _title.text = ThisSkill.Name;
        ThisButton.interactable = ThisSkill.Unlocked;
        ThisSkillTree = FindObjectOfType<SkillTree>();
    }

    void Update()
    {
    }
    void RankUp()
    {
        if(ThisSkillTree.SkillPoints > 0 && ThisSkill.CurrentRank < ThisSkill.MaxRank)
        {
            ThisSkill.CurrentRank++;
            ThisSkillTree.SkillPoints--;
            ThisSkillTree.Refresh();
        }
    }
    public void Refresh()
    {
        ThisButton.interactable = ThisSkill.Unlocked;
        ThisButton.GetComponentInChildren<Text>().text = ThisSkill.Display;
    }
    public void OnPointerEnter(PointerEventData p)
    {
        _text.text = ThisSkill.Description;
        _text.fontSize = 14;
    }
    public void OnPointerExit(PointerEventData p)
    {
        _text.text = ThisSkill.Display;
        _text.fontSize = 36;
    }
}
