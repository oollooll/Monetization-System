using System;

namespace Services.ScoreDir
{
    public interface IScoreUpdater
    {
        public delegate void ScoreUpdated(int amount);
        public event ScoreUpdated OnScoreUpdate;
    }
}