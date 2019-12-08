using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public Button playButton;
    public Button size45Button;
    public Button size56Button;
    public Image Brickle;
    public Image ChooseSize;
    public Toggle TutorialCheck;


    public Animator BrickleAnim;
    public Animator ChoosesizeAnim;
    public Animator Size45;
    public Animator Size56;
    public Animator PlayButtonImageAnim;
    public Animator ToggleAmin;

    // Start is called before the first frame update
    void Start()
    {
        playButton.onClick.AddListener(PlayButtonClick);
        size45Button.onClick.AddListener(size45click);
        size56Button.onClick.AddListener(size56click);
        size45Button.gameObject.SetActive(false);
        size56Button.gameObject.SetActive(false);
        ChooseSize.gameObject.SetActive(false);
        TutorialCheck.gameObject.SetActive(false);


    }

    // Update is called once per frame
    void Update()
    {

    }

    void PlayButtonClick() {
        Debug.Log("I clicked da btton");
        PlayButtonImageAnim.Play("PlayButtonImage");
        BrickleAnim.Play("BrickleAnimation");
        size45Button.gameObject.SetActive(true);
        size56Button.gameObject.SetActive(true);
        ChooseSize.gameObject.SetActive(true);
        TutorialCheck.gameObject.SetActive(true);
        playButton.gameObject.SetActive(false);
        ChoosesizeAnim.Play("ChooseSizeAnimation");
        ToggleAmin.Play("ToggleAnimation");
        
    }

    void size45click() {
        Size45.Play("size45AnimationImage");
        size56Button.interactable = false;
        if (TutorialCheck.isOn == true) {
            StartCoroutine(DelayedLoad(1f, "MobileTutorial"));
        }
        else {

            StartCoroutine(DelayedLoad(1f, "SinglePlayerMobile"));
        }

        
    }

    void size56click() {
        Size56.Play("size56AnimationImage");
        size45Button.interactable = false;
        if (TutorialCheck.isOn == true) {
            StartCoroutine(DelayedLoad(0.5f, "MobileTutorial"));
        }
        else {
            StartCoroutine(DelayedLoad(0.5f, "SinglePlayerMobile"));
        }
    }


    IEnumerator DelayedLoad(float delay, string scene) {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(scene);
    }


}
