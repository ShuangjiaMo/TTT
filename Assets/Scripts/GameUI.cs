using UnityEngine ;
using UnityEngine.UI ;
using UnityEngine.SceneManagement ;

public class GameUI : MonoBehaviour {
   [Header ("UI References :")]
   [SerializeField] private Text uiTextTip ;
   [SerializeField] private Button uiRestartButton ;

   [Header ("Board Reference :")]
   [SerializeField] private Board board ;

   private AudioSource audioClick ;

   private void Start () {
      audioClick = Camera.main.transform.Find("AudioClick").GetComponent<AudioSource> () ;
      uiRestartButton.onClick.AddListener (RestartGame) ;
      board.OnWinAction += OnWinEvent ;
      board.OnFirstAction += OnFirstEvent ;

      uiTextTip.text = "\"你先下吧\"";
      uiRestartButton.gameObject.SetActive (false) ;
   }

   private void RestartGame() {
      audioClick.Play();
      SceneManager.UnloadScene("Game");
      SceneManager.LoadScene("Game", LoadSceneMode.Additive);
   }

   private void OnWinEvent (Mark mark) {
      if (mark == Mark.X)
      {
         uiTextTip.text = "\"厉害啊，要再来一局吗？\"";
      }
      else if (mark == Mark.O)
      {
         uiTextTip.text = "\"嘿嘿，我赢啦，要再来一局吗？\"";
      }
      else
      {
         uiTextTip.text = "\"势均力敌呢，要再来一局吗？\"";
      }

      uiRestartButton.gameObject.SetActive (true) ;
   }

   private void OnFirstEvent () {
      uiTextTip.text = "";
   }

   private void OnDestroy () {
      uiRestartButton.onClick.RemoveAllListeners () ;
      board.OnWinAction -= OnWinEvent ;
      board.OnFirstAction -= OnFirstEvent ;
   }
}
