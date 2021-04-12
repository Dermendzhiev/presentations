namespace Service.Contracts
{
    public interface ILogger
    {
        void Information(string messageTemplate, params object[] propertyValues);
    }
}
