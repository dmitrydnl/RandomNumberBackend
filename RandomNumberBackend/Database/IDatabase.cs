using System;

namespace RandomNumberBackend.Database
{
    public interface IDatabase
    {
        public bool CreateGame(string nickname, int hiddenNumber);
    }
}
