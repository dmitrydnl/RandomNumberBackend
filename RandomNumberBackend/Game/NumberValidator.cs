using System;

namespace RandomNumberBackend.Game
{
    public class NumberValidator : INumberValidator
    {
        public NumberValidatorStatus Validate(int myNumber, int hiddenNumber)
        {
            if (myNumber > hiddenNumber)
            {
                return NumberValidatorStatus.Greater;
            }
            else if (myNumber < hiddenNumber)
            {
                return NumberValidatorStatus.Less;
            }
            else
            {
                return NumberValidatorStatus.Equal;
            }
        }
    }
}
