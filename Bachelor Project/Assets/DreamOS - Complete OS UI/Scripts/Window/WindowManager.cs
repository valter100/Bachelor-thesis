using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Michsky.DreamOS
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CanvasGroup))]
    public class WindowManager : MonoBehaviour, IPointerClickHandler
    {
        // Resources
        public Animator windowAnimator;
        public GameObject taskBarButton;
        public WindowDragger windowDragger;
        public WindowResize windowResize;

        // Fullscreen & minimize
        public Image imageObject;
        public Sprite fullscreenImage;
        public Sprite minimizeImage;

        // Settings
        public bool enableBackgroundBlur = true;
        public bool hasNavDrawer = true;
        public bool enableMobileMode = false;

        // Events
        public UnityEvent onEnableEvents;
        public UnityEvent onLaunchEvents;
        public UnityEvent onQuitEvents;

        Animator navDrawerAnimator;
        TaskBarButton tbbHelper;
        BlurManager windowBGBlur;
        RectTransform windowRect;

        float left;
        float right;
        float top;
        float bottom;
        bool isNavDrawerOpen = true;

        [HideInInspector] public bool isNormalized;
        [HideInInspector] public bool isFullscreen;
        [HideInInspector] public bool disableAtStart = true;

        void Start()
        {
            SetupWindow();
        }

        void OnEnable()
        {
            if (hasNavDrawer == true)
            {
                if (navDrawerAnimator == null)
                    navDrawerAnimator = transform.Find("Content").GetComponent<Animator>();

                if (PlayerPrefs.GetString(gameObject.name + "NavDrawer") == "true")
                {
                    navDrawerAnimator.Play("Content Expand");
                    isNavDrawerOpen = true;
                }

                else if (PlayerPrefs.GetString(gameObject.name + "NavDrawer") == "false"
                    || !PlayerPrefs.HasKey(gameObject.name + "NavDrawer"))
                {
                    navDrawerAnimator.Play("Content Minimize");
                    isNavDrawerOpen = false;
                }
            }

            onEnableEvents.Invoke();
        }

        public void SetupWindow()
        {
            if (windowAnimator == null)
                windowAnimator = gameObject.GetComponent<Animator>();

            if (taskBarButton != null && enableMobileMode == false)
            {
                try
                {
                    tbbHelper = taskBarButton.GetComponent<TaskBarButton>();
                    tbbHelper.windowManager = this.GetComponent<WindowManager>();
                }

                catch { Debug.Log("<b>[Window Manager]</b> No variable attached to Task Bar Button. Task Bar functions won't be working properly.", this); }
            }

            if (enableBackgroundBlur == true)
            {
                try { windowBGBlur = gameObject.GetComponent<BlurManager>(); }
                catch { Debug.Log("<b>[Window Manager]</b> No Blur Manager attached to Game Object. Background Blur won't be working.", this); }
            }

            windowRect = gameObject.GetComponent<RectTransform>();

            if (hasNavDrawer == true)
            {
                navDrawerAnimator = transform.Find("Content").GetComponent<Animator>();

                if (PlayerPrefs.GetString(gameObject.name + "NavDrawer") == "true")
                {
                    navDrawerAnimator.Play("Content Expand");
                    isNavDrawerOpen = true;
                }

                else if (PlayerPrefs.GetString(gameObject.name + "NavDrawer") == "false")
                {
                    navDrawerAnimator.Play("Content Minimize");
                    isNavDrawerOpen = false;
                }

                else if (PlayerPrefs.GetString(gameObject.name + "NavDrawer") == "")
                {
                    navDrawerAnimator.Play("Content Minimize");
                    isNavDrawerOpen = false;
                }
            }

            if (windowDragger != null)
                windowDragger.wManager = this.GetComponent<WindowManager>();

            left = windowRect.offsetMin.x;
            right = -windowRect.offsetMax.x;
            top = -windowRect.offsetMax.y;
            bottom = windowRect.offsetMin.y;

            if (disableAtStart == true)
                gameObject.SetActive(false);

            if (enableMobileMode == true)
                return;

            if (imageObject != null)
                imageObject.sprite = fullscreenImage;
        }

        public void NavDrawerAnimate()
        {
            if (isNavDrawerOpen == true)
            {
                PlayerPrefs.SetString(gameObject.name + "NavDrawer", "false");
                navDrawerAnimator.Play("Content Minimize");
                isNavDrawerOpen = false;
            }

            else
            {
                PlayerPrefs.SetString(gameObject.name + "NavDrawer", "true");
                navDrawerAnimator.Play("Content Expand");
                isNavDrawerOpen = true;
            }
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            FocusToWindow();
        }

        public void FocusToWindow()
        {
            gameObject.transform.SetAsLastSibling();
        }

        public void OpenWindow()
        {
            StopCoroutine("DisableObject");
            gameObject.SetActive(true);

            if (!windowAnimator.GetCurrentAnimatorStateInfo(0).IsName("Panel Fullscreen")
                && (!windowAnimator.GetCurrentAnimatorStateInfo(0).IsName("Panel Normalize")))
                windowAnimator.Play("Panel In");

            if (taskBarButton != null && enableMobileMode == false)
                tbbHelper.SetOpen();
            else if (taskBarButton != null && enableMobileMode == true)
                taskBarButton.SetActive(true);

            if (windowBGBlur != null)
                windowBGBlur.BlurInAnim();

            FocusToWindow();
            onLaunchEvents.Invoke();
        }

        public void CloseWindow()
        {
            if (enableMobileMode == false)
            {
                if (taskBarButton != null)
                    tbbHelper.SetClosed();
                else if (taskBarButton != null)
                    taskBarButton.SetActive(false);

                if (enableBackgroundBlur == true && windowBGBlur != null)
                    windowBGBlur.BlurOutAnim();

                windowAnimator.Play("Panel Out");
            }

            else
            {
                taskBarButton.SetActive(false);

                if (windowAnimator.GetCurrentAnimatorStateInfo(0).IsName("Panel Shrink")
                    || windowAnimator.GetCurrentAnimatorStateInfo(0).IsName("Panel Shrink Helper"))
                    windowAnimator.Play("Panel Shrink Minimize");
                else
                    windowAnimator.Play("Panel Out");
            }

            StartCoroutine("DisableObject");
            onQuitEvents.Invoke();
        }

        public void MinimizeWindow()
        {
            windowAnimator.Play("Panel Minimize");

            if (taskBarButton != null)
                tbbHelper.SetMinimized();

            if (enableBackgroundBlur == true && windowBGBlur != null)
                windowBGBlur.BlurOutAnim();
        }

        public void FullscreenWindow()
        {
            if (isFullscreen == false)
            {
                isFullscreen = true;
                isNormalized = false;

                if (imageObject != null)
                    imageObject.sprite = minimizeImage;

                StartCoroutine("SetFullscreen");
            }

            else
            {
                isFullscreen = false;
                isNormalized = true;

                if (imageObject != null)
                    imageObject.sprite = fullscreenImage;

                StartCoroutine("SetNormalized");
            }
        }

        public void ShrinkWindowMobile()
        {
            windowAnimator.Play("Panel Shrink");
        }

        public void ShrinkWindowInstantMobile()
        {
            windowAnimator.Play("Panel Shrink Helper");
        }

        public void ShrikWindowMinimizeMobile()
        {
            windowAnimator.Play("Panel Shrink Minimize");
        }

        public void ExpandWindowMobile()
        {
            windowAnimator.Play("Panel Expand");
        }

        public void MinimizeWindowMobile()
        {
            windowAnimator.Play("Panel Out");
        }

        IEnumerator SetFullscreen()
        {
            left = windowRect.offsetMin.x;
            right = -windowRect.offsetMax.x;
            top = -windowRect.offsetMax.y;
            bottom = windowRect.offsetMin.y;

            windowAnimator.Play("Panel Fullscreen");

            // Left and bottom
            windowRect.offsetMin = new Vector2(0, 0);

            // Right and top
            windowRect.offsetMax = new Vector2(0, 0);

            isFullscreen = true;
            isNormalized = false;

            if (windowResize != null)
                windowResize.gameObject.SetActive(false);

            yield return null;
        }

        IEnumerator SetNormalized()
        {
            windowAnimator.Play("Panel Normalize");

            // Left and bottom
            windowRect.offsetMin = new Vector2(left, bottom);

            // Right and top
            windowRect.offsetMax = new Vector2(-right, -top);

            isFullscreen = false;
            isNormalized = true;

            if (windowResize != null)
                windowResize.gameObject.SetActive(true);

            yield return null;
        }

        IEnumerator DisableObject()
        {
            yield return new WaitForSeconds(1f);
            gameObject.SetActive(false);
        }
    }
}