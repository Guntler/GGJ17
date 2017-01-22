using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroBehavior : MonoBehaviour {
    public Light EggSpotlight;
    public Light EggDirectional;
    public Image SplashScreen;
    public Canvas Canvas;

    public AudioClip TitleTheme;
    public AudioClip IntroVoice;

    AudioSource src;

    bool showedSplashScreenP1 = false;
    bool showedSplashScreen = false;
    float elapsedWaitSplash = 0;
    float waitSplashScreen = 2f;

    // Use this for initialization
    void Start () {
        src = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if(!showedSplashScreen)
        {
            if (showedSplashScreenP1)
            {
                elapsedWaitSplash += Time.deltaTime;
                if(elapsedWaitSplash > waitSplashScreen)
                {
                    SplashScreen.color = new Color(1, 1, 1, SplashScreen.color.a - Time.deltaTime * 0.5f);
                    if (SplashScreen.color.a <= 0.0f)
                        showedSplashScreen = true;
                }
            }
            else
            {
                SplashScreen.color = new Color(1,1,1,SplashScreen.color.a + Time.deltaTime*0.5f);
                if (SplashScreen.color.a >= 1.0f)
                {
                    src.PlayOneShot(IntroVoice);
                    showedSplashScreenP1 = true;
                }
            }
        }
        else
        {
            elapsedWaitSplash = 0;
            if (EggSpotlight.intensity < 8)
            {
                EggSpotlight.intensity += Time.deltaTime * 2.0f;
            }
            else
            {
                if (EggDirectional.intensity < 4)
                {
                    EggDirectional.intensity += Time.deltaTime * 10.0f;
                    if (EggDirectional.intensity >= 3 && EggDirectional.intensity < 4)
                    {
                        src.clip = TitleTheme;
                        src.loop = true;
                        src.Play();
                        Canvas.gameObject.SetActive(true);

                    }
                }
            }
        }
	}

    IEnumerator DoIntroSequence()
    {

        yield return null;
    }
}
