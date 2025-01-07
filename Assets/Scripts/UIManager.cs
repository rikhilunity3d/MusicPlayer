using Obvious.Soap;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject PlayButton;
    public GameObject PauseButton;
    public GameObject MuteButton;
    public GameObject UnMuteButton;
    public GameObject NextButton;
    public GameObject PreviousButton;

    public Slider  slider;
    
    [SerializeField]
    BoolVariable isPause;
    [SerializeField]
    BoolVariable isMute;

    [SerializeField]
    FloatVariable sliderCurrentValue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayButton.SetActive(true);
        PauseButton.SetActive(false);     
        MuteButton.SetActive(true);
        UnMuteButton.SetActive(false);
        NextButton.SetActive(true);
        PreviousButton.SetActive(true);

        isPause.OnValueChanged += OnPlayButtonPressed;
        isMute.OnValueChanged += OnMuteButtonPressed;
        sliderCurrentValue.OnValueChanged += OnSliderValueUpdate;
    }

     void OnSliderValueUpdate(float value)
    {
        slider.value = value;           
    }

    void OnDestroy()
    {
        isPause.OnValueChanged -= OnPlayButtonPressed;
        isMute.OnValueChanged-= OnMuteButtonPressed;
        sliderCurrentValue.OnValueChanged-= OnSliderValueUpdate;
    }

    void OnMuteButtonPressed(bool isMute)
    {
        if(isMute==true)
            Mute();
        else
            UnMute();
    }


    void OnPlayButtonPressed(bool isPause)
    {
        if(isPause == true )
            Pause();
        else
            Play();
    }
    // Update is called once per frame
    

    void Play()
    {
        PlayButton.SetActive(false);
        PauseButton.SetActive(true);
        isPause.Value = false;
        // Play Audio
        //SoundManager.SendMessage("Play");
    }

    void Pause()
    {
        PlayButton.SetActive(true);
        PauseButton.SetActive(false);
        isPause.Value = true;
        // Pause Audio
        //SoundManager.SendMessage("Pause");
    }

    public void Mute()
    {
        // Mute Audio
        MuteButton.SetActive(false);
        UnMuteButton.SetActive(true);  
        isMute.Value = true;   
        //SoundManager.SendMessage("Mute");   
    }

    public void UnMute()
    {
        // UnMute Audio
        MuteButton.SetActive(true);
        UnMuteButton.SetActive(false);
        isMute.Value = false;
        //SoundManager.SendMessage("UnMute");
    }

}
