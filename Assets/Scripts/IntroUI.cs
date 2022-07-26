using UnityEngine ;
using UnityEngine.UI ;
using UnityEngine.SceneManagement ;

public class IntroUI : MonoBehaviour {
   [Header ("UI References :")]
   [SerializeField] private Button uiButtonStart ;
   [SerializeField] private Button uiButtonQuit ;

   private Animator anim ;
   private AudioSource audioClick ;

   private void Start () {
      audioClick = Camera.main.transform.Find("AudioClick").GetComponent<AudioSource> () ;
      uiButtonStart.onClick.AddListener (StartGame) ;
      uiButtonQuit.onClick.AddListener (QuitGame) ;
      anim = Camera.main.GetComponent<Animator> ()  ;
   }

   private void StartGame() {
      audioClick.Play();
      SceneManager.UnloadScene("Intro");
      anim.SetTrigger("start");
   }

   private void QuitGame() {
#if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
#else
      Application.Quit();
#endif
   }

   private void OnDestroy () {
      uiButtonStart.onClick.RemoveAllListeners () ;
   }
}
