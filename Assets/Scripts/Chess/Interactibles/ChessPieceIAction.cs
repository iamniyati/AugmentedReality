using UnityEngine;
using System.Collections;

/**
 *  This class manages all the manipulation of all the pieces
 *  It interacts with all the other pieces and tiles.
 *
 * @Name    ChessPieceIAction
 *
 * @author  Jeffrey Bauer, Niyati Shah, Tanvi Raut, Mityashh Dhaggai
 *
 * 
 */

public class ChessPieceIAction : InteractibleAction {

    private ChessboardManager chessboardManager;
    private PieceProperties properties;

    private Material[] defaultMaterials;

    private bool isSelected = false;
    private float lastSelectActionTime = -999;

    private Vector3 iPosition;
    private Vector3 ePosition;
    private float journeyLength;
    private float maxHeight = 0.15f;
    private float speed = 1.0f;

   /**
     * @description : This function starts the piece action.
     *                 It does the initial set up of adding colliders to objects
     *		        It positions the pieces to its correct place in the board.      
     *  
     *  
     */
    void Start() {

        properties = GetComponent<PieceProperties>();

        defaultMaterials = GetComponent<Renderer>().materials;

        // Add a BoxCollider if the intractable does not contain one.
        Collider collider = GetComponentInChildren<Collider>();
        if (collider == null) {
            gameObject.AddComponent<BoxCollider>();
        }

        iPosition = this.transform.localPosition;
        ePosition = iPosition + new Vector3(0, maxHeight, 0);
        journeyLength = Vector3.Distance(iPosition, ePosition);

    }

   /**
     * @description : This function is called every time to check
     *                if pieces are selected and any movements need 
     *		          any kind of updates or not.
     *  
     *  
     */

    void Update() {

        if (isSelected)
        {
            float distCovered = (Time.time - lastSelectActionTime) * speed;
            float fracJourney = distCovered / journeyLength;
            transform.localPosition = Vector3.Lerp(iPosition, ePosition, fracJourney);
        }
        else {
            float distCovered = (Time.time - lastSelectActionTime) * speed;
            float fracJourney = distCovered / journeyLength;
            transform.localPosition = Vector3.Lerp(ePosition, iPosition, fracJourney);
        }

        // Move to parent tile
        GameObject parentTile = properties.parentTile;
        if (parentTile != null) {
            transform.localPosition = new Vector3(parentTile.transform.localPosition.x, transform.localPosition.y, parentTile.transform.localPosition.z);
        }
        

    }

   /**
     * @description : This function is overriden.
     *      glow the object that the gaze has entered.          
     *  	and increase the focusedObject count.
     *  
     */

    public override void ActionGazeEntered() {
        //glow the piece on gaze
        base.ActionGazeEntered();

        SetGlow(true);
        chessboardManager.focusedObjectCount++;

    }

  /**
     * @description : This function is overriden.
     *      remove the glow the object that the gaze has exited.          
     *  	and decrease the focusedObject count.
     *  
     */

    public override void ActionGazeExited() {
        //discontinue glowing the piece once gaze is exited
        base.ActionGazeExited();

        SetGlow(false);
        chessboardManager.focusedObjectCount--;

    }

     /**
     * @description : This function is overriden.
     *          Perform the corresponding    
     *  	   action on select an object  
     *  
     */

    public override void ActionOnSelect() {
        base.ActionOnSelect();


        if (chessboardManager.localUser == null) chessboardManager.GetLocalUser();
        //if (chessboardManager.currentTurn != chessboardManager.localUser.GetComponent<UserController>().playerNum) return;
        chessboardManager.SelectPosition("p", GetComponent<PieceProperties>().id);

    }

     /**
     * @description : This sets the ChessboardManager
     * 
     *  @param : ChessboardManager: ChessboardManager
     */

    public void SetManager(ChessboardManager chessboardManager) {
        this.chessboardManager = chessboardManager;
    }

    public void SetSelected(bool isSelected) {
        this.isSelected = isSelected;
        lastSelectActionTime = Time.time;
    }

   /**
     * @description : Function to Glow the objects or 
     *                 remove the glow	from objects
     *                 
     *  @param : bool: glow
     *  
     */

    public void SetGlow(bool glow) {

        int rimActive = 0;
        if (glow) {
            rimActive = 1;
        }

        for (int i = 0; i < defaultMaterials.Length; i++) {
            defaultMaterials[i].SetInt("_RimActive", rimActive);
        }

        foreach (Transform child in this.transform) {
            GameObject childGO = child.gameObject;
            Material[] childDM = childGO.GetComponent<Renderer>().materials;
            for (int i = 0; i < childDM.Length; i++) {
                childDM[i].SetInt("_RimActive", rimActive);
            }
        }

    }

}
