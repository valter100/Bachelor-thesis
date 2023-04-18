using UnityEngine;
using System.Collections;

namespace Michsky.DreamOS
{
    public class PopupPanelManager : MonoBehaviour
    {
        [Header("Resources")]
        public Animator panelAnimator;

        [Header("Settings")]
        public bool enableBlurAnim = true;

        [Header("Optimization")]
        public bool disableWhenFading = false;
        [Range(0.1f, 5)] public float disableAfter = 1;

        BlurManager bManager;
        [HideInInspector] public bool isOn;

        void Start()
        {
            if (panelAnimator == null)
                panelAnimator = gameObject.GetComponent<Animator>();

            bManager = gameObject.GetComponent<BlurManager>();

            if (isOn == false && disableWhenFading == true)
                gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }

        public void AnimatePanel()
        {
            if (isOn == true)
            {
                isOn = false;
                panelAnimator.Play("Panel Minimize");
                StartCoroutine("DisableContent");

                if (enableBlurAnim == true)
                    bManager.BlurOutAnim();
            }

            else
            {
                isOn = true;
                panelAnimator.Play("Panel Expand");
                StopCoroutine("DisableContent");
                gameObject.transform.GetChild(0).gameObject.SetActive(true);

                if (enableBlurAnim == true)
                    bManager.BlurInAnim();
            }
        }

        public void OpenPanel()
        {
            isOn = true;
            panelAnimator.Play("Panel Expand");
            StopCoroutine("DisableContent");
            gameObject.transform.GetChild(0).gameObject.SetActive(true);

            if (enableBlurAnim == true)
                bManager.BlurInAnim();
        }

        public void ClosePanel()
        {
            isOn = false;
            panelAnimator.Play("Panel Minimize");
            StartCoroutine("DisableContent");

            if (enableBlurAnim == true)
                bManager.BlurOutAnim();
        }

        IEnumerator DisableContent()
        {
            yield return new WaitForSeconds(disableAfter);

            if (disableWhenFading == true)
                gameObject.transform.GetChild(0).gameObject.SetActive(false);

            StopCoroutine("DisableContent");
        }
    }
}