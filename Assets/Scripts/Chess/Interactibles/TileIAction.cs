using UnityEngine;
using System.Collections;

/**
 * This class manages the actions of the tile.
 *  The class is of the type intractable.
 *
 * @Name    TileIAction
 *
 * @author  Jeffrey Bauer, Niyati Shah, Tanvi Raut, Mityashh Dhaggai
 *
 * 
 */

public class TileIAction : InteractibleAction
{

    private ChessboardManager chessboardManager;
    private Material[] defaultMaterials;
    private TileProperties properties;

    private bool isGlowing = false;

   /**
     * @description : This function starts the tile action.
     *                 It does the initial set up tiles that 
     *		        make up the board.      
     *  
     *  
     */
    void Start()
    {
        chessboardManager = GameObject.FindGameObjectWithTag("Chessboard").GetComponent<ChessboardManager>();
        properties = GetComponent<TileProperties>();
        defaultMaterials = GetComponent<Renderer>().materials;
    }

   /**
     * @description : This function is overriden.
     *      glow the object that the gaze has entered.          
     *  	and increase the focusedObject count.
     *  
     */

    public override void ActionGazeEntered()
    {
        base.ActionGazeEntered();
        chessboardManager.focusedObjectCount++;

    }

  /**
     * @description : This function is overriden.
     *      remove the glow the object that the gaze has exited.          
     *  	and decrease the focusedObject count.
     *  
     */
    public override void ActionGazeExited()
    {
        base.ActionGazeExited();
        chessboardManager.focusedObjectCount--;

    }

     /**
     * @description : This function is overriden.
     *          Perform the corresponding    
     *  	   action on select an object  
     *  
     */

    public override void ActionOnSelect()
    {
        base.ActionOnSelect();
        
        if (chessboardManager.localUser == null) chessboardManager.GetLocalUser();
        //if (chessboardManager.currentTurn != chessboardManager.localUser.GetComponent<UserController>().playerNum) return;
        chessboardManager.SelectPosition("t", properties.id);

    }

   /**
     * @description : Function to Glow the objects or 
     *                 remove the glow	from objects
     *                 
     *  @param : bool: glow
     *  
     */
    public void SetGlow(bool glow)
    {

        isGlowing = glow;

        int rimActive = 0;
        if (glow)
        {
            rimActive = 1;
        }

        for (int i = 0; i < defaultMaterials.Length; i++)
        {
            defaultMaterials[i].SetInt("_RimActive", rimActive);
        }

        foreach (Transform child in this.transform)
        {
            GameObject childGO = child.gameObject;
            Material[] childDM = childGO.GetComponent<Renderer>().materials;
            for (int i = 0; i < childDM.Length; i++)
            {
                childDM[i].SetInt("_RimActive", rimActive);
            }
        }

    }

    /**
     * @description : This function checks if the object is glowing 		
     *       		or not        
     *			                      
     *  
     *
     *  @return: bool true if glowing false if not glowing
     */   
    public bool IsGlowing() {
        return isGlowing;
    }

}
