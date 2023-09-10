using System;
using System.Collections.Concurrent;

namespace RandomNumberBackend.Database
{
    public class DatabaseLocal : IDatabase
    {
        private readonly ConcurrentDictionary<string, int> currentGames;

        public DatabaseLocal()
        {
            currentGames = new ConcurrentDictionary<string, int>();
        }

        public bool CreateGame(string nickname, int hiddenNumber)
        {
            return currentGames.TryAdd(nickname, hiddenNumber);
        }

        public int? GetCurrentGame(string nickname)
        {
            if (currentGames.TryGetValue(nickname, out int hiddenNumber))
            {
                return hiddenNumber;
            }

            return null;
        }

        public void FinishGame(string nickname)
        {
            currentGames.TryRemove(nickname, out int hiddenNumber);
        }
    }
}
