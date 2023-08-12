using UnityEngine;

namespace Code.UI
{
    public class WorldCanvas : MonoBehaviour
    {
        void Start()
        {
            GetComponent<Canvas>().worldCamera = Camera.main;
        }
    }
}