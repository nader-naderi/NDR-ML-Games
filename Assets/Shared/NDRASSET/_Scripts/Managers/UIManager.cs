using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace NDRLiteFPS
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager _instance;

        public static UIManager instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<UIManager>();
                }

                return _instance;
            }
        }

        [SerializeField] GameObject interactionIcon;
        [SerializeField] Text interactionTxt;

        public void SetInteraction(string text, bool value)
        {
            interactionIcon.SetActive(value);
            if (value)
                interactionTxt.text = text;
            else
                interactionTxt.text = string.Empty;
        }
    }
}