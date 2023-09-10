using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace RandomNumberBackend.Database
{
    public class DatabaseLocal : IDatabase
    {
        private readonly ConcurrentDictionary<string, int> currentGames;
        private readonly ConcurrentDictionary<string, List<int>> userGames;

        public DatabaseLocal()
        {
            currentGames = new ConcurrentDictionary<string, int>();
            userGames = new ConcurrentDictionary<string, List<int>>();
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
            if (currentGames.TryRemove(nickname, out int hiddenNumber))
            {
                userGames.AddOrUpdate(nickname, new List<int> { hiddenNumber }, (key, oldValue) =>
                {
                    List<int> copy = new List<int>(oldValue);
                    copy.Add(hiddenNumber);
                    return copy;
                });
            }
        }
    }
}
