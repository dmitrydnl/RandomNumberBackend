using System;
using System.Collections.Generic;

namespace RandomNumberBackend.Database
{
    public interface IDatabase
    {
        public bool CreateGame(string nickname, int hiddenNumber);

        public int? GetCurrentGame(string nickname);

        public void FinishGame(string nickname);

        public Dictionary<string, int> GetGlobalStatistics();

        public List<int> GetUserStatistics(string nickname);

        public bool Authorization(string nickname, string password);
    }
}
