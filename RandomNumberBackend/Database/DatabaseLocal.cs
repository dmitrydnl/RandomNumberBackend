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
            userGames = new ConcurrentDictionary<string, List<int>>()
            {
                ["cat"] = new List<int> { 37, 68 },
                ["dog"] = new List<int> { 2, 8, 99 }
            };
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
                    var copy = new List<int>(oldValue);
                    copy.Add(hiddenNumber);
                    return copy;
                });
            }
        }

        public KeyValuePair<string, List<int>>[] GetGlobalStatistics()
        {
            return userGames.ToArray();
        }

        public List<int> GetUserStatistics(string nickname)
        {
            if (userGames.TryGetValue(nickname, out List<int> games))
            {
                return games;
            }

            return new List<int>();
        }
    }
}
