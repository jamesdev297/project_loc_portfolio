using System;
using UnityEngine;

namespace Script.Popup
{
    public class RewardAlertPopUp : MonoBehaviour
    {
        public GameObject popUp;
        private Animator tabAnim;

        private void Start()
        {
            if (GameInfoManager.Instance.firstLoginUser)
            {
                show();
                GameInfoManager.Instance.firstLoginUser = false;
            }
        }

        public void close()
        {
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