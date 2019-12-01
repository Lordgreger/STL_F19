using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public Button playButton;
    public Button size45Button;
    public Button size56Button;
    public Image Brickle;
    public Image ChooseSize;
    public Image size45Image;
    public Image size56Image;

    public Animator BrickleAnim;
    public Animator ChoosesizeAnim;
    public Animator Size45;
    public Animator Size56;
    public Animator PlayButtonImageAnim;

    // Start is called before the first frame update
    void Start()
    {
        playButton.onClick.AddListener(PlayButtonClick);
        size45Button.onClick.AddListener(size45click);
        size56Button.onClick.AddListener(size56click);
        size45Button.gameObject.SetActive(false);
        size56Button.gameObject.SetActive(false);
        ChooseSize.gameObject.SetActive(false);


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
        playButton.gameObject.SetActive(false);
        ChoosesizeAnim.Play("ChooseSizeAnimation");
        
    }

    void size45click() {
        Debug.Log("Clikked da 45");
    }

    void size56click() {
        Debug.Log("clickety clik on 56");
    }


}
