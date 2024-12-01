using System.Collections;
using TMPro;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private BeatDetection randomBeatDetectionAlgorythm;
    [SerializeField] private BPMAnalyzer randomBPMAnalyzer;

    [SerializeField] private Animator loadingScreenAnimator;
    [SerializeField] private Animator songNameAnimator;

    [SerializeField] private TextMeshProUGUI songNameText;
    [SerializeField] private TextMeshProUGUI progressText;

    [SerializeField] private float addDotInterval = 1;

    private float timer;

    private string currentTask;
    private string dots = "";
    private void Update()
    {
        progressText.text = currentTask + dots;
        timer += Time.deltaTime;

        if(timer > addDotInterval)
        {
            UpdateDots();
            timer = 0;
        } 

    }

    private void UpdateDots()
    {
        if(dots.Length == 3)
        {
            dots = "";
        }
        else
        {
            dots += ".";
        }
    }


    public IEnumerator ShowLoadingSong(string songName)
    {
        loadingScreenAnimator.gameObject.SetActive(true);

        songNameText.text = songName;
        currentTask = "Gathering Anticipated Spectrum Data...";
        yield return new WaitForSeconds(GameManager.Instance.Delay);

        songNameAnimator.SetTrigger("show");

        currentTask = "Waiting for the Beat Detection Window to Fill";
        yield return new WaitUntil(() => randomBeatDetectionAlgorythm.isListFilled);
        yield return new WaitForSeconds(GameManager.Instance.Delay);
        loadingScreenAnimator.SetTrigger("hide");

        currentTask = "Analyzing BPMs";
        yield return new WaitUntil(() => randomBPMAnalyzer.BPMs != 0);
        loadingScreenAnimator.SetBool("hideTask", true);

        yield return new WaitForSeconds(1);
        songNameAnimator.SetTrigger("hide");
    }
}
