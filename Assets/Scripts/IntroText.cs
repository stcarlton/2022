using Cinemachine;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntroText : MonoBehaviour
{
    [SerializeField] TMP_Text _intro;
    CanvasGroup _canvasGroup;
    [SerializeField] CinemachineVirtualCamera _camera;
    CinemachineTransposer _cameraTransposer;

    string[] TextSequence =
    {
        "It's the year",
        "2022",
        "A highly contagious virus has forced the entire world to isolate themselves indoors for the past two years",
        "With more free time, people could finally develop new skills and learn new things",
        "A more informed and worldly public was poised to usher in the next renaissance",
        "Instead, an army of hyper sophisticated robots were trained to maximize online ad performance",
        "Unintentionally radicalizing everyone's political beliefs with ruthless efficiency",
        "Gradually, all independent thoughts were replaced with sensationalized cable news talking points",
        "Or worse, batshit insane conspiracy theories",
        "Eventually, their brains devolved into mush, and millions of slobbering fools roamed the streets",
        "They hunt down anyone who disagrees with them about mundane political matters, and beat them to death with their fists",
        "You are one of the few remaining hopes for humankind",
        "You must survive"
    };

    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _cameraTransposer = _camera.GetCinemachineComponent<CinemachineTransposer>();
        _canvasGroup.blocksRaycasts = false;
        StartCoroutine(PlayIntro());    
    }
    IEnumerator PlayIntro()
    {
        _canvasGroup.alpha = 1;
        for (int i = 0; i < TextSequence.Length; i++)
        {
            int delay = TextSequence[i].Length / 30 +3;
            _intro.text = TextSequence[i];
            if (i == 1)
            {
                _intro.fontSize = 432;
            }
            else
            {
                _intro.fontSize = 72;
            }
            yield return new WaitForSeconds(delay);
        }
        _canvasGroup.alpha = 0;
        _cameraTransposer.m_FollowOffset = new Vector3(0, 20, 0);
        Debug.Log("Mark end of intro: " + Time.time);
    }
}
