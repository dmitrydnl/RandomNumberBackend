using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace RandomNumberBackend.Database
{
    public class DatabaseLocal : IDatabase
    {
        private readonly ConcurrentDictionary<string, int> currentGames;
        private readonly ConcurrentDictionary<string, List<int>> userGames;
        private readonly ConcurrentDictionary<string, string> userToPassword;

        public DatabaseLocal()
        {
            currentGames = new ConcurrentDictionary<string, int>();
            userGames = new ConcurrentDictionary<string, List<int>>()
            {
                ["cat"] = new List<int> { 37, 68 },
                ["dog"] = new List<int> { 2, 8, 99 }
            };
            userToPassword = new ConcurrentDictionary<string, string>()
            {
                ["cat"] = "123456",
                ["dog"] = "654321"
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

        public Dictionary<string, int> GetGlobalStatistics()
        {
            var result = new Dictionary<string, int>();
            foreach (var nicknameToGames in userGames.ToArray())
            {
                result.Add(nicknameToGames.Key, nicknameToGames.Value.Count);
            }

            return result;
        }

        public List<int> GetUserStatistics(string nickname)
        {
            if (userGames.TryGetValue(nickname, out List<int> games))
            {
                return games;
            }

            return new List<int>();
        }

        public bool Authorization(string nickname, string password)
        {
            if (userToPassword.TryGetValue(nickname, out string userPassword))
            {
                return userPassword.Equals(password);
            }

            return false;
        }

        public bool Registration(string nickname, string password)
        {
            if (userToPassword.TryAdd(nickname, password))
            {
                return true;
            }

            return false;
        }

        public bool IsUserExist(string nickname)
        {
            if (userToPassword.TryGetValue(nickname, out string userPassword))
            {
                return true;
            }

            return false;
        }
    }
}
