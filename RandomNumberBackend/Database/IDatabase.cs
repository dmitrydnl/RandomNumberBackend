using System;
using System.Collections.Generic;

namespace RandomNumberBackend.Database
{
    public interface IDatabase
    {
        public bool CreateGame(string nickname, int hiddenNumber);

        public int? GetCurrentGame(string nickname);

        public void FinishGame(string nickname);

        public KeyValuePair<string, List<int>>[] GetGlobalStatistics();
    }
}
