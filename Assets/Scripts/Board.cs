using UnityEngine ;
using UnityEngine.Events ;
using System ;

public class Board : MonoBehaviour {
   [Header ("Input Settings : ")]
   [SerializeField] private LayerMask boxesLayerMask ;
   [SerializeField] private float touchRadius ;

   [Header ("Mark Sprites : ")]
   [SerializeField] private Sprite spriteX ;
   [SerializeField] private Sprite spriteO ;

   public UnityAction<Mark> OnWinAction ;
   public UnityAction OnFirstAction ;
   public Mark[] marks ;
   private Camera cam ;
   private Mark currentMark ;
   private bool canPlay ;
   private LineRenderer lineRenderer ;
   private int marksCount = 0 ;
   private Vector3 clickDelta = new Vector3(0.01f, 0.01f, 0.01f) ;
   private int thinkTime = 1;
   private AudioSource audioDraw ;

   private void Start () {
      cam = Camera.main ;
      audioDraw = Camera.main.transform.Find("AudioDraw").GetComponent<AudioSource> () ;
      currentMark = Mark.X ;
      marks = new Mark[9] ;
      canPlay = true ;
      lineRenderer = GetComponent<LineRenderer> () ;
      lineRenderer.enabled = false ;
   }

   private void Update () {
      if (canPlay && Input.GetMouseButtonUp (0) && currentMark == Mark.X) {
         Ray ray = cam.ScreenPointToRay (Input.mousePosition) ;
         RaycastHit[] hitColliders = Physics.BoxCastAll (ray.origin, clickDelta, ray.direction);

         if (hitColliders.Length > 0)
            HitBox (hitColliders[0].transform.GetComponent <Box> ()) ;
      }
   }

   private void HitBox (Box box) {
      if (box && !box.isMarked) {
         audioDraw.Play();

         if (marksCount == 0)
         {
            if (OnFirstAction != null)
               OnFirstAction.Invoke () ;
         }

         marks [ box.index ] = currentMark ;
         box.SetAsMarked (GetSprite (), currentMark) ;
         marksCount++ ;

         if (CheckIfWin ()) {
            if (OnWinAction != null)
               OnWinAction.Invoke (currentMark) ;

            canPlay = false ;
            return ;
         }

         if (marksCount == 9) {
            if (OnWinAction != null)
               OnWinAction.Invoke (Mark.None) ;

            canPlay = false ;
            return ;
         }

         SwitchPlayer () ;

         if (currentMark == Mark.O)
         {
            Invoke("AIMove", thinkTime);
         }
      }
   }

   private bool CheckIfWin (bool evaluate = false) {
      return AreBoxesMatched (0, 1, 2, evaluate) || 
      AreBoxesMatched (3, 4, 5, evaluate) || 
      AreBoxesMatched (6, 7, 8, evaluate) ||
      AreBoxesMatched (0, 3, 6, evaluate) || 
      AreBoxesMatched (1, 4, 7, evaluate) || 
      AreBoxesMatched (2, 5, 8, evaluate) ||
      AreBoxesMatched (0, 4, 8, evaluate) || 
      AreBoxesMatched (2, 4, 6, evaluate) ;

   }

   private bool AreBoxesMatched (int i, int j, int k, bool evaluate) {
      Mark m = currentMark ;
      bool matched = (marks [ i ] == m && marks [ j ] == m && marks [ k ] == m) ;

      if (matched && !evaluate)
         DrawLine (i, k) ;

      return matched ;
   }

   private void DrawLine (int i, int k) {
      lineRenderer.SetPosition (0, transform.GetChild (i).position) ;
      lineRenderer.SetPosition (1, transform.GetChild (k).position) ;
      lineRenderer.enabled = true ;
   }

   private void SwitchPlayer () {
      currentMark = (currentMark == Mark.X) ? Mark.O : Mark.X ;
   }

   private Sprite GetSprite () {
      return (currentMark == Mark.X) ? spriteX : spriteO ;
   }

   private int Minimax (bool isMax) {
      if (CheckIfWin(true))
      {
         return (currentMark == Mark.X) ? 1 : -1 ;
      }

      if (marksCount == 9)
      {
         return 0;
      }

      if (isMax)
      {
         int maxVal = -10;
         for (int i = 0; i < 9; i++)
         {
            if (marks[i] == Mark.None)
            {
               marks[i] = Mark.X;
               currentMark = Mark.X;
               marksCount++;
               maxVal = Mathf.Max(maxVal, Minimax(!isMax));
               marks[i] = Mark.None;
               currentMark = Mark.O;
               marksCount--;
            }
         }
         return maxVal;
      }
      else
      {
         int minVal = 10;
         for (int i = 0; i < 9; i++)
         {
            if (marks[i] == Mark.None)
            {
               marks[i] = Mark.O;
               currentMark = Mark.O;
               marksCount++;
               minVal = Math.Min(minVal, Minimax(!isMax));
               marks[i] = Mark.None;
               currentMark = Mark.X;
               marksCount--;
            }
         }
         return minVal;
      }
   }

   private void AIMove () {
      int minVal = 10, moveVal, moveIndex = 0;
      for (int i = 0; i < 9; i++)
      {
         if (marks[i] == Mark.None)
         {
            marks[i] = Mark.O;
            currentMark = Mark.O;
            marksCount++;
            moveVal = Minimax(true);
            marks[i] = Mark.None;
            currentMark = Mark.X;
            marksCount--;
            if (moveVal < minVal)
            {
               moveIndex = i;
               minVal = moveVal;
            }
         }
      }

      currentMark = Mark.O;
      HitBox(GameObject.Find(moveIndex.ToString()).GetComponent<Box> ()) ;
   }
}
