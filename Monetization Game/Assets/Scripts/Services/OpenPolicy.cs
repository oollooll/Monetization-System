using UnityEngine;
using UnityEngine.SceneManagement;

namespace Services
{
    public class OpenPolicy:MonoBehaviour
    {
        public void Policy()
        {
            SceneManager.LoadSceneAsync("PrivacyPolicy");
        }
    }
}