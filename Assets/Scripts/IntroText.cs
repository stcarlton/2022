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
        "A highly contagious virus has forced the entire world to isolate themselves indoors for the past two years, greatly increasing everyone's free time",
        "People could develop new skills and pick up new hobbies. They could finally learn how to speak a 2nd language, play the piano or sign up for that online photagraphy course",
        "A more informed, educated and worldly public was poised to usher in the next renaissance; a new era of peace, artistic expression and invention",
        "Unfortunately, this is the exact opposite of what would unfold",
        "Instead an army of hyper sophisticated robots were trained to maximize online ad performance, unintentionally radicalizing everyone's political beliefs with ruthless efficiency",
        "It didn't matter who they were, the robots would read people's minds, discover their political leniencies, and brainwash them",
        "Gradually, all independent thoughts were replaced with sensationalized cable news talking points, or worse, batshit insane conspiracy theories",
        "Eventually, their brains devolved into mush, and millions of slobbering fools roamed the streets",
        "They hunted down anyone who disagreed with them about mundane political matters, and beat them to death with their fists",
        "You are one of the few remaining hopes for humankind yet to become indoctrinated",
        "You must survive."
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
            int delay = TextSequence[i].Length / 30 + 2;
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
        _cameraTransposer.m_FollowOffset = new Vector3(0, 15, 0);
        Debug.Log("Mark end of intro: " + Time.time);
    }
    void PauseGame()
    {
        Time.timeScale = 0;
    }
    void UnPauseGame()
    {
        Time.timeScale = 1;
    }
}
