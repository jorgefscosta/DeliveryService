using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryService.DL.Infrastructure
{
    public class ErrorHandler : IErrorHandler
    {
        public string GetErrorMessage(ErrorMessagesEnum errorNum)
        {
            switch (errorNum)
            {
                case ErrorMessagesEnum.ModelValidation:
                    return "The request data is not valid. For instance {0}";
                case ErrorMessagesEnum.EntityNull:
                    return "The entity passed cannot be null.";
                case ErrorMessagesEnum.EntityNotFound:
                    return "The entity passed does not exists.";
                case ErrorMessagesEnum.EntityDuplicate:
                    return "The entity passed already exists.";

                default:
                    throw new ArgumentOutOfRangeException(nameof(errorNum), errorNum, null);
            }
        }
    }
}
