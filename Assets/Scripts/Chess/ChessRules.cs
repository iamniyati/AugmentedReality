using UnityEngine;
using System.Collections.Generic;

/**
 * This class manages the rules of the chess game
 *  
 *
 * @Name    ChessRules
 *
 * @author  Jeffrey Bauer, Niyati Shah, Tanvi Raut, Mityashh Dhaggai
 *
 * 
 */

public class ChessRules : MonoBehaviour {

    public static ChessboardManager chessboardManager;

/**
 * This inner class manages the position in the game
 *  
 *
 * @Name    Position
 *
 * @author  Jeffrey Bauer, Niyati Shah, Tanvi Raut, Mityashh Dhaggai
 *
 * 
 */
    public class Position {
        public int row, col;
        public Position(int row, int col) {
            this.row = row;
            this.col = col;
        }
    }

     /**
     * @description : This function checks the available moves that can be 		
     *       		made by the piece in the game.          
     *			                      
     *  @param : GameObject: piece
     *
     *  @return: list of available moves
     */   

    public static List<Position> GetAvailableMoves(GameObject piece) {

        // List to store all available moves
        List<Position> availableMoves = new List<Position>();

        // Get all the properties that we need
        PieceProperties pieceProp = piece.GetComponent<PieceProperties>();
        int team = pieceProp.team;
        int row = pieceProp.row;
        int col = pieceProp.column;
        PieceProperties.Type type = pieceProp.type;
	
	// Pawn moves
        if (PieceProperties.Type.Pawn == type) {

            int dir = 0;
            int dist = 1;
            if (team == 0) {
                dir = 1;
                if (row == 1) dist = 2; ;
            } else if (team == 1) {
                dir = -1;
                if (row == 6) dist = 2;
            }

            // Moving forwards
            GameObject otherPiece;
            for (int i = 1; i <= dist; i++) {

                // Stop right before any piece
                otherPiece = chessboardManager.FindPiece(row + i * dist, col);
                if (otherPiece != null) break;

                availableMoves.Add(new Position(row + dir * i, col));

            }

            // Moving diagonal if there is an enemy piece
            otherPiece = chessboardManager.FindPiece(row + 1, col + 1);
            if (otherPiece != null && otherPiece.GetComponent<PieceProperties>().team != team) {
                availableMoves.Add(new Position(row + 1, col + 1));
            }
            otherPiece = chessboardManager.FindPiece(row + 1, col - 1);
            if (otherPiece != null && otherPiece.GetComponent<PieceProperties>().team != team) {
                availableMoves.Add(new Position(row + 1, col - 1));
            }

        } else if (PieceProperties.Type.Rook == type) {
            AddOrthogonal(availableMoves, row, col, team, 8);
        } else if (PieceProperties.Type.Bishop == type) {
            AddDiagonal(availableMoves, row, col, team, 8);
        } else if (PieceProperties.Type.Queen == type) {
            AddOrthogonal(availableMoves, row, col, team, 8);
            AddDiagonal(availableMoves, row, col, team, 8);
        } else if (PieceProperties.Type.King == type) {
            AddOrthogonal(availableMoves, row, col, team, 1);
            AddDiagonal(availableMoves, row, col, team, 1);
        } else if (PieceProperties.Type.Knight == type) {
            AddL(availableMoves, row, col, team);
        }

        // Glow all available moves
        FindAndGlowAll(availableMoves);

        return availableMoves;

    }

     /**
     * @description : Finds the piece or a tile and glows the piece 
     *                 	or the tile at a given position
     *                 
     *  @param :int: row and column
     *  
     */
    
    private static void FindAndGlow(int row, int col) {
	
	// Tile
        GameObject foundTile;
        foundTile = chessboardManager.FindTile(row, col);
        if (foundTile != null) {
            foundTile.GetComponent<TileIAction>().SetGlow(true);
        }
	
	// Piece
        GameObject foundPiece;
        foundPiece = chessboardManager.FindPiece(row, col);
        if (foundPiece != null) {
            foundPiece.GetComponent<ChessPieceIAction>().SetGlow(true);
        }

    }

     /**
     * @description : Finds all the pieces and tiles from the given 
     *                 	set of positions and glow them
     *                 
     *  @param :list :list of positions
     *  
     */
    
    private static void FindAndGlowAll(List<Position> positions) {
        foreach(Position p in positions) {
            FindAndGlow(p.row, p.col);
        }
    }

     /**
     * @description : Finds all the pieces and tiles that are glowing 
     *                  remove their glow them
     *    
     */
    // Removes glow of all peices an tiles on the board
    public static void ClearGlow() {
        for (int i = 0; i < chessboardManager.tiles.Count; i++) {
            chessboardManager.tiles[i].GetComponent<TileIAction>().SetGlow(false);
        }
        for (int i = 0; i < chessboardManager.pieces.Count; i++) {
            chessboardManager.pieces[i].GetComponent<ChessPieceIAction>().SetGlow(false);
        }
    }

     /**
     * @description : Gets the pieces to move in a straight line 
     *                 	
     *                 
     *  @param :list :list of positions, 
     *  	int: row, column, team and maxDistance
     */
  
    private static void AddOrthogonal(List<Position> locs, int row, int col, int team, int maxDistance) {

        int dRow, dCol;
        for (int step = 0; step < 4; step++) {

            if (step == 0) {
                dRow = 1;
                dCol = 0;
            } else if (step == 1) {
                dRow = -1;
                dCol = 0;
            } else if (step == 2) {
                dRow = 0;
                dCol = 1;
            } else {
                dRow = 0;
                dCol = -1;
            }

            int i = 1;
            GameObject tile = chessboardManager.FindTile(row + dRow * i, col + dCol * i);
            while (tile != null && i <= maxDistance) {

                // Stop right before friendly piece
                TileProperties tProp = tile.GetComponent<TileProperties>();
                if (tProp.childPiece != null && tProp.childPiece.GetComponent<PieceProperties>().team == team) {
                    break;
                }

                locs.Add(new Position(row + dRow * i, col + dCol * i));
                i++;
                tile = chessboardManager.FindTile(row + dRow * i, col + dCol * i);

                // Stop on enemy piece
                if (tProp.childPiece != null && tProp.childPiece.GetComponent<PieceProperties>().team != team) {
                    break;
                }

            }

        }
        
    }

     /**
     * @description : Gets the pieces to move in a diagonal line 
     *                 	
     *                 
     *  @param :list :list of positions, 
     *  	int: row, column, team and maxDistance
     */
 
   private static void AddDiagonal(List<Position> locs, int row, int col, int team, int maxDistance) {

        int dRow, dCol;
        for (int step = 0; step < 4; step++) {

            if (step == 0) {
                dRow = 1;
                dCol = 1;
            } else if (step == 1) {
                dRow = 1;
                dCol = -1;
            } else if (step == 2) {
                dRow = -1;
                dCol = -1;
            } else {
                dRow = -1;
                dCol = 1;
            }

            int i = 1;
            GameObject tile = chessboardManager.FindTile(row + dRow * i, col + dCol * i);
            while (tile != null && i <= maxDistance) {

                // Stop right before friendly piece
                TileProperties tProp = tile.GetComponent<TileProperties>();
                if (tProp.childPiece != null && tProp.childPiece.GetComponent<PieceProperties>().team == team) {
                    break;
                }

                locs.Add(new Position(row + dRow * i, col + dCol * i));
                i++;
                tile = chessboardManager.FindTile(row + dRow * i, col + dCol * i);

                // Stop on enemy piece
                if (tProp.childPiece != null && tProp.childPiece.GetComponent<PieceProperties>().team != team) {
                    break;
                }

            }

        }

    }

     /**
     * @description : Gets the Knights moves
     *                 	
     *                 
     *  @param :list :list of positions, 
     *  	int: row, column, team
     */

    private static void AddL(List<Position> locs, int row, int col, int team) {

        GameObject tile, piece;
        int dRow, dCol;
        for (int step = 0; step < 8; step++) {

            if (step == 0) {
                dRow = 2;
                dCol = 1;
            } else if (step == 1) {
                dRow = 2;
                dCol = -1;
            } else if (step == 2) {
                dRow = -2;
                dCol = 1;
            } else if (step == 3) {
                dRow = -2;
                dCol = -1;
            } else if (step == 4) {
                dCol = 2;
                dRow = 1;
            } else if (step == 5) {
                dCol = 2;
                dRow = -1;
            } else if (step == 6) {
                dCol = -2;
                dRow = 1;
            } else {
                dCol = -2;
                dRow = -1;
            }

            tile = chessboardManager.FindTile(row + dRow, col + dCol);
            piece = chessboardManager.FindPiece(row + dRow, col + dCol);
            if (tile != null && (piece == null || piece.GetComponent<PieceProperties>().team != team)) {
                locs.Add(new Position(row + dRow, col + dCol));
            }

        }

    }

     /**
     * @description : Check if given team's king is in check
     *                 	
     *                 
     *  @param : int: team 
     *  	
     */
   
    private static bool IsInCheck(int team) {

        // Find team's king piece and get its row and column
        GameObject king = null;
        foreach (GameObject piece in chessboardManager.pieces) {
            PieceProperties pProp = piece.GetComponent<PieceProperties>();
            if (pProp.team == team && pProp.type == PieceProperties.Type.King) {
                king = piece;
                break;
            }
        }

        int kRow = king.GetComponent<PieceProperties>().row;
        int kCol = king.GetComponent<PieceProperties>().column;

        // Check if any enemy pieces are pressuring king
        foreach (GameObject p in chessboardManager.pieces) {
            if (p.GetComponent<PieceProperties>().team != team) {

                List<Position> availableMoves = GetAvailableMoves(p);
                foreach(Position pos in availableMoves) {
                    if (pos.row == kRow && pos.col == kCol) {
                        return true;
                    }
                }

            }
        }

        // If not, return false
        return false;

    }

}

