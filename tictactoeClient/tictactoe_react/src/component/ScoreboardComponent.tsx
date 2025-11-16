/* eslint-disable @typescript-eslint/no-explicit-any */
import React, { useState, useEffect, useCallback } from 'react';
import Service from '../service/service';

const ScoreboardComponent: React.FC = () => {
    const [scores, setScores] = useState<any[]>([]);
    // eslint-disable-next-line @typescript-eslint/no-unused-vars
    const [loading, setLoading] = useState<boolean>(true);

    const fetchLeaderboard = useCallback(async () => {
        setLoading(true);
        try {
            // Assuming a service method to get the leaderboard data
            const leaderboardData = await Service.getLeaderboard();
            if (leaderboardData) {
                // Sort by score in descending order
                setScores(leaderboardData);
            }
        } catch (error) {
            console.error("Failed to fetch leaderboard:", error);
        } finally {
            setLoading(false);
        }
    }, []);

    useEffect(() => {
        fetchLeaderboard();
        // Refresh leaderboard every 10 seconds
        const interval = setInterval(fetchLeaderboard, 10000);

        return () => clearInterval(interval);
    }, [fetchLeaderboard]);

    return (
        <div className="scoreboard-container">
            <div className="scoreboard-header">
                <h2>Scoreboard</h2>
                <a onClick={fetchLeaderboard} className="refresh-button"><i className='fa-solid fa-arrows-rotate'></i></a>
            </div>
            <table className="scoreboard-table">
                <thead>
                    <tr>
                        <th className='text-center' style={{ width: '30%' }}>No.</th>
                        <th style={{ width: '50%' }}>Email</th>
                        <th className='text-center' style={{ width: '20%' }}>Score</th>
                    </tr>
                </thead>
                <tbody>
                    {scores.map((user, index) => (
                        <tr key={index} className={index < 3 ? 'top-three' : ''}>
                            <td className='text-center'>{index < 3 ? '🏆' : ''} {index + 1}</td>
                            <td >{user.email}</td>
                            <td className='text-center'>{user.score}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div >
    );
};

export default ScoreboardComponent;
