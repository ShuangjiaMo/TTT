using UnityEngine ;

public class Box : MonoBehaviour {
   public int index ;
   public Mark mark ;
   public bool isMarked ;
   private SpriteRenderer spriteRenderer ;

   private void Awake () {
      spriteRenderer = GetComponent<SpriteRenderer> () ;
      index = transform.GetSiblingIndex () ;
      mark = Mark.None ;
      isMarked = false ;
   }

   public void SetAsMarked (Sprite sprite, Mark mark) {
      isMarked = true ;
      this.mark = mark ;
      spriteRenderer.sprite = sprite ;
      GetComponent<BoxCollider> ().enabled = false ;
   }
}
