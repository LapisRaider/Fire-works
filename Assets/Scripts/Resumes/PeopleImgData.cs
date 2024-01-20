using System;
using System.Collections.Generic;
using UnityEngine;

public class PeopleImgData : MonoBehaviour
{
    public List<ImgData> m_People = new List<ImgData>();

    [Serializable]
    public class ImgData
    {
        public GameObject m_Animal;
        public GameObject m_Clothe;
    }
}
