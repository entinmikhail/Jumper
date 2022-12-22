using UnityEngine;

namespace Services
{
    public interface ILoadingCurtainsViewer
    {
        void Enable();
        void Disable();
    }

    public class LoadingCurtainsViewer : MonoBehaviour, ILoadingCurtainsViewer
    {
        [SerializeField] private GameObject _loadingCurtains;

        public void Enable()
        {
            _loadingCurtains.SetActive(true);
        }

        public void Disable()
        {
            _loadingCurtains.SetActive(false);
        }
    }
}