namespace MasterMind.Services
{
    public interface IGameService
    {
        char[] GenerateSecretNumber(int size, int minDigit, int maxDigit);
        char[] ValidateSecretAttempt(char[] secret, char[] attempt);
    }
}
