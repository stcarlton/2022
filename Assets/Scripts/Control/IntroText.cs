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
        "People finally had the free time to develop skills and learn new things",
        "A more informed and worldly public was poised to usher in the next renaissance",
        "Instead, an army of hyper sophisticated robots were trained to maximize online ad performance, unintentionally radicalizing all political beliefs with ruthless efficiency",
        "Eventually, hordes of slobbering fools roamed the streets, mumbling about cable news talking points and insane conspiracies",
        "Angry mobs chase down anyone who disagrees with them about mundane political matters and beats them to death with their bare hands",
        "You are the last remaining hope for humankind",
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
                _intro.fontSize = 648;
            }
            else
            {
                _intro.fontSize = 72;
            }
            yield return new WaitForSeconds(delay);
        }
        _canvasGroup.alpha = 0;
        Debug.Log("Mark end of intro: " + Time.time);
    }
}
