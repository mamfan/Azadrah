using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EasyTransition
{
    public class DemoLoadScene : MonoBehaviour
    {
        public TransitionSettings transition;
        public float startDelay;
        public AudioSource startCarAudio;

        public void LoadScene(string _sceneName)
        {
            Time.timeScale = 1;

            if (SceneManager.GetActiveScene().name == "Main Menu" && _sceneName == "Game")
            {
                startCarAudio.Play();
            }

            if (PhotonNetwork.IsConnected && _sceneName == "Main Menu") PhotonNetwork.Disconnect();

            TransitionManager.Instance().Transition(_sceneName, transition, startDelay);
        }

        public void LoadSceneAsync(string _sceneName)
        {
            Time.timeScale = 1;

            TransitionManager.Instance().TransitionAsync(_sceneName, transition, startDelay);
        }
    }
}