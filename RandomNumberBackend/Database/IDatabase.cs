using System;

namespace RandomNumberBackend.Database
{
    public interface IDatabase
    {
        public bool IsGameExist(string nickname);

        public bool CreateGame(string nickname, int hiddenNumber);
    }
}
