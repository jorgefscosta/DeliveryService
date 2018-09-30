namespace DeliveryService.DL.Infrastructure
{
    public interface IErrorHandler
    {
        string GetErrorMessage(ErrorMessagesEnum errorNum);
    }
    public enum ErrorMessagesEnum
    {
        ModelValidation = 1,
        EntityNull = 2,
        EntityNotFound = 3,
        EntityDuplicate = 4,
        InvalidOption = 5
    }
}
