using UnityEngine;
using System.Collections;

namespace NDRLiteFPS
{
    public enum SurfaceType : int
    {
        Generic,
        Blood,
        Concrete,
        Dirt,
        Glass,
        Metal,
        Water,
        Wood,
        Crate,
        Carpet,
        Vent,
        Ladder
    }
    public class WeaponsGFX : MonoBehaviour
    {


        #region Singleton
        private static WeaponsGFX _instance;

        public static WeaponsGFX instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<WeaponsGFX>();
                }

                return _instance;
            }
        }
        #endregion

        [SerializeField] Impact[] impacts;

        public Impact GetImpact(SurfaceType surfaceType)
        {
            return impacts[(int)surfaceType];
        }


    }

    [System.Serializable]
    public struct Impact
    {
        [SerializeField]
        string name;
        [SerializeField]
        GameObject[] bulletImpacts;

        public SurfaceImpact GetSurfaceImpact
        {
            get
            {
                SurfaceImpact impact = bulletImpacts[Random.Range(0, bulletImpacts.Length)].GetComponent<SurfaceImpact>();

                return impact;
            }
        }

        public GameObject SurfaceImpact
        {
            get
            {
                return bulletImpacts[Random.Range(0, bulletImpacts.Length)];
            }
        }
    }
}