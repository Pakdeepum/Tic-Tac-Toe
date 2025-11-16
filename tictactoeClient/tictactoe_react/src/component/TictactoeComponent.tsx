/* eslint-disable no-debugger */
/* eslint-disable @typescript-eslint/no-explicit-any */
import React, { useState, useEffect } from 'react';
import Service from '../service/service';
import Swal from 'sweetalert2'

interface TictactoeProps {
    user: any;
}

type Player = 'X' | 'O';
type SquareValue = Player | null;

const PLAYER: Player = 'X';
const BOT: Player = 'O';

const calculateWinner = (squares: SquareValue[]): SquareValue | 'draw' | null => {
    const lines = [
        [0, 1, 2],
        [3, 4, 5],
        [6, 7, 8],
        [0, 3, 6],
        [1, 4, 7],
        [2, 5, 8],
        [0, 4, 8],
        [2, 4, 6],
    ];
    for (let i = 0; i < lines.length; i++) {
        const [a, b, c] = lines[i];
        if (squares[a] && squares[a] === squares[b] && squares[a] === squares[c]) {
            return squares[a];
        }
    }
    if (squares.every(square => square !== null)) {
        return 'draw';
    }
    return null;
};


const TictactoeComponent: React.FC<TictactoeProps> = ({ user }) => {
    const [board, setBoard] = useState<SquareValue[]>(Array(9).fill(null));
    const [isPlayerTurn, setIsPlayerTurn] = useState<boolean>(true);
    const [winner, setWinner] = useState<SquareValue | 'draw' | null>(null);
    const [userData, setUserData] = useState<any>();
    const [winCountData, setCountData] = useState<any>();

    const GetUserData = async (email: string) => {
        const _user = await Service.getUser(email);
        if (_user && _user.length > 0) {
            setUserData(_user[0]);
            if(userData){
                const _winCount = await Service.checkWinCount(userData.id);
                setCountData(_winCount);
            }
        }
        return _user;
    };

    const UpdateScore = async (id: string, score: number) => {
        await Service.UpdateScore(userData.id, score);

        const _user = await GetUserData(user.email);

        return _user;
    };

    const botMove = () => {
        const newBoard = [...board];

        // 1. Check if bot can win
        for (let i = 0; i < 9; i++) {
            if (newBoard[i] === null) {
                newBoard[i] = BOT;
                if (calculateWinner(newBoard) === BOT) {
                    setBoard(newBoard);
                    setWinner(BOT);
                    setIsPlayerTurn(true);
                    return;
                }
                newBoard[i] = null; // backtrack
            }
        }

        // 2. Check if player is about to win and block
        for (let i = 0; i < 9; i++) {
            if (newBoard[i] === null) {
                newBoard[i] = PLAYER;
                if (calculateWinner(newBoard) === PLAYER) {
                    newBoard[i] = BOT; //bot move
                    setBoard(newBoard);
                    setIsPlayerTurn(true);
                    return;
                }
                newBoard[i] = null; // backtrack
            }
        }

        // 3. Otherwise, make a random move
        const availableSpots = board.map((val, idx) => val === null ? idx : null).filter(val => val !== null);
        if (availableSpots.length > 0) {
            const randomMove = availableSpots[Math.floor(Math.random() * availableSpots.length)] as number;
            newBoard[randomMove] = BOT;
            setBoard(newBoard);

            const gameResult = calculateWinner(newBoard);
            if (gameResult) {
                setWinner(gameResult);
            }
        }

        setIsPlayerTurn(true);
    };

    useEffect(() => {
        if (!user?.email) return;
        const fetchUserData = async () => {
            await GetUserData(user.email);
        };

        fetchUserData();
    }, [user?.email]);

    useEffect(() => {   
        if (!isPlayerTurn && !winner) {
            const timeoutId = setTimeout(() => {
                botMove();
            }, 500);

            return () => clearTimeout(timeoutId);
        }
    }, [isPlayerTurn, winner, board]);

    const handleClick = (index: number) => {
        if (board[index] || winner || !isPlayerTurn) {
            return;
        }

        const newBoard = [...board];
        newBoard[index] = PLAYER;
        setBoard(newBoard);

        const gameResult = calculateWinner(newBoard);
        if (gameResult) {
            setWinner(gameResult);
        } else {
            setIsPlayerTurn(false);
        }
    };

    const restartGame = () => {
        setBoard(Array(9).fill(null));
        setWinner(null);
        setIsPlayerTurn(true);
        GetUserData(user.email);
    };

    const renderSquare = (index: number) => {
        return (
            <button className="square" onClick={() => handleClick(index)}>
                {board[index]}
            </button>
        );
    };

    let status;
    if (winner) {
        let isExtra = false;
        if(winner === 'X'){
            if(winCountData == 2){
                isExtra = true;
                UpdateScore(userData.id, 2);
            }else{
                UpdateScore(userData.id, 1);
            }
        }else if(winner === 'O'){
            UpdateScore(userData.id, -1);
        }
        Swal.fire({
            title: winner === 'draw' ? "It's a Draw!" : `Winner: ${winner}`,
            text: isExtra ? 'You Win 3 Times! Get 1 Point!' : '',
            icon: winner === 'draw' ? 'info' : winner === 'X' ? 'success' : 'error',
            confirmButtonText: 'Ok'
        })

        
        restartGame();
    } else {
        status = `Next player: ${isPlayerTurn ? 'You (X)' : 'Bot (O)'}`;
    }

    return (
        <div className="game">
            <div className='score_text'>Your Score: <span className='score'>{userData?.score}</span></div>
            <div className="status">{status}</div>
            <div className="board">
                <div className="board-row">
                    {renderSquare(0)}{renderSquare(1)}{renderSquare(2)}
                </div>
                <div className="board-row">
                    {renderSquare(3)}{renderSquare(4)}{renderSquare(5)}
                </div>
                <div className="board-row">
                    {renderSquare(6)}{renderSquare(7)}{renderSquare(8)}
                </div>
            </div>
            <button className="restart-button" onClick={restartGame}>Restart Game</button>
        </div>
    );
}

export default TictactoeComponent;