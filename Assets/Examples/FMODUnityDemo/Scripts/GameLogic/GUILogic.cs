using UnityEngine;
using UnityEngine.UI;

/*
* Handles GUI logic, such as clicking the english and swedish buttons on
* the pause menu.
*/

namespace GameLogic
{
    public class GUILogic : MonoBehaviour
    {
        public Button engButton;
        public Button sweButton;

        private void Start()
        {
            //add listener for english UI button
            engButton.onClick.AddListener(() =>
            {
                //disable eng button and enable swe button
                engButton.interactable = false;
                sweButton.interactable = true;

                //switch VO bank to english
                LocalisationVO.switchBankTo(VOLanguage.ENGLISH);
            });
            //add listener for swedish UI button
            sweButton.onClick.AddListener(() =>
            {
                //disable swe button and enable eng button
                engButton.interactable = true;
                sweButton.interactable = false;

                //switch VO bank to swedish
                LocalisationVO.switchBankTo(VOLanguage.SWEDISH);
            });
        }
    }
};
