using System;

namespace RandomNumberBackend.Game
{
    public interface INumberValidator
    {
        public NumberValidatorStatus Validate(int myNumber, int hiddenNumber);
    }
}
