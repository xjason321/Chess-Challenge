using ChessChallenge.API;
using System;
using System.Collections.Generic;
public class MyBot : IChessBot
{
    public Move Think(Board board, Timer timer) {
        Move[] moves = board.GetLegalMoves();
        
        Move bestCalculatedMove = moves[0];
        
        string botColor = (board.IsWhiteToMove) ? "w" : "b";

        double bestEval = (botColor == "w") ? -99999999 : 99999999; 

        foreach(Move move in moves){
            board.MakeMove(move);

            double CalculatedEval = minimax(board, 3, double.NegativeInfinity, double.PositiveInfinity, board.IsWhiteToMove);

            if(botColor == "w"){
                if (CalculatedEval > bestEval){
                    bestCalculatedMove = move;
                    bestEval = CalculatedEval;
                } 
            } else {
                if (CalculatedEval < bestEval){
                    bestCalculatedMove = move;
                    bestEval = CalculatedEval;
                }
            }

            board.UndoMove(move);
        }
        return bestCalculatedMove;
    }

    private double evaluate(Board board){
        double value = 0;
        
        // Evaluation based on piece values
        foreach(PieceList pieces in board.GetAllPieceLists()){
            foreach(Piece piece in pieces){
                double v = 0;
                switch (piece.PieceType){
                    case PieceType.Pawn:
                        v += 100;
                        break;
                    case PieceType.Knight:
                        v += 300;
                        break;
                    case PieceType.Bishop:
                        v += 300;
                        break;
                    case PieceType.Rook:
                        v += 500;
                        break;
                    case PieceType.Queen:
                        v += 900;
                        break;
                    case PieceType.King:
                        v += 1000;
                        break;
                }
                
                v -= (Math.Abs(3.5 - piece.Square.Rank) + Math.Abs(3.5 - piece.Square.File))*5;
                
                value += piece.IsWhite ? v : -v;
            }
        }
        
        return value;
    }

    private double minimax(Board board, int depth, double alpha, double beta, bool isMaximizingPlayer){
    
        double bestEval = 0;

        if (depth == 0){
            return evaluate(board);
        }
        
        Move[] moves = board.GetLegalMoves();

        if (isMaximizingPlayer){
            bestEval = -99999999999;
            
            foreach(Move move in moves){
                board.MakeMove(move);
                double evaluate = minimax(board, depth-1, alpha, beta, false);   
                board.UndoMove(move);
                
                bestEval = Math.Max(bestEval, evaluate);
                alpha = Math.Max(alpha, bestEval);

                // Alpha-beta pruning
                if (beta <= alpha)
                    break;
            }
            return bestEval;
            
        } else {
            bestEval = 99999999999;

            foreach(Move move in moves){
                board.MakeMove(move);
                double evaluate = minimax(board, depth-1, alpha, beta, true);   
                board.UndoMove(move);

                bestEval = Math.Min(bestEval, evaluate);
                beta = Math.Min(beta, bestEval);

                // Alpha-beta pruning
                if (beta <= alpha)
                    break;
            }
            return bestEval;
        }    
    }    
}