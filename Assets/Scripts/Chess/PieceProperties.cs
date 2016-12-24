using UnityEngine;
using System.Collections;
/**
 * This class initializes the piece and the piece properties.
 *  
 *
 * @Name    PieceProperties
 *
 * @author  Jeffrey Bauer, Niyati Shah, Tanvi Raut, Mityashh Dhaggai
 *
 * 
 */

public class PieceProperties : MonoBehaviour {

    public enum Type
    {
        Pawn,
        King,
        Queen,
        Knight,
        Bishop,
        Rook
    }

    public int id;

    public int team = 0;

    public int row;
    public int column;

    [SerializeField]
    public GameObject parentTile;

    [SerializeField]
    public Type type;

    public Color originalColor;

}
