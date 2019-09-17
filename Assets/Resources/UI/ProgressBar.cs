using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {

    public static ProgressBar progressBar;

    public Image playerProgress;
    public float f_fill;

    void Start () {
        progressBar = this;
	
    }

	void Update () {


        playerProgress.fillAmount = f_fill;
    }
}
