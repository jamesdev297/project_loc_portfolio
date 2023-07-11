using UnityEngine;

namespace Script.Popup
{
    public class ReviewAlertPopUp : MonoBehaviour
    {
        public GameObject popUp;
        private Animator tabAnim;
        
        public void close()
        {
            hide();
        }

        public void goReview()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                Application.OpenURL("market://details?id=com.pugstudio.battleOfLegends");
            }else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                Application.OpenURL("itms-apps://itunes.apple.com/app/id1602261003");
            }
            
            hide();
        }
        
        private void hide()
        {
            tabAnim.SetTrigger("close");
            Invoke("DisableTab", 0.6f);
        }
    
        void DisableTab()
        {
            if(tabAnim.gameObject.activeSelf)
                popUp.SetActive(false);
        }
        public void show()
        {
            popUp.SetActive(true);
            tabAnim = popUp.GetComponent<Animator>();
        }
    }
}