using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionManager : MonoBehaviour
{
    FullScreenMode screenMode;
    public Dropdown resolutionDropdown;
    public Toggle fullscreenBtn;

    List<Resolution> resolutions = new List<Resolution>()
    {
        new Resolution { width = 1920, height = 1080 }, new Resolution { width = 2560, height = 1440 }
    };

    int resolutionNum;

    void Start()
    {        
        InitUI();
    }

    void InitUI()
    {        
        resolutionDropdown.options.Clear();
        int optionNum = 0;

        foreach(Resolution item in resolutions)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = item.width + "x" + item.height + " ";
            resolutionDropdown.options.Add(option);

            if(item.width == Screen.width && item.height == Screen.height)
            {
                resolutionDropdown.value = optionNum;
            }
            optionNum++;
        }
        
        resolutionDropdown.RefreshShownValue();
        fullscreenBtn.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? false : true;
    }

    public void DropboxOptionChange(int x)
    {
        resolutionNum = x;
    }

    public void FullScreenBtn(bool isFull)
    {
        screenMode = isFull ? FullScreenMode.Windowed : FullScreenMode.FullScreenWindow;
    }

    public void OkBtnClick()
    {
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, screenMode);
    }    
}
