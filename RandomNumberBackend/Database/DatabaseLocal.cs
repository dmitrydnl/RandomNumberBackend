using System;

namespace RandomNumberBackend.Database
{
    public class DatabaseLocal : IDatabase
    {
        public bool IsGameExist(string nickname)
        {
            throw new NotImplementedException();
        }

        public bool CreateGame(string nickname, int hiddenNumber, DateTime now)
        {
            throw new NotImplementedException();
        }
    }
}
