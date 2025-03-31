using System.Net;

namespace CarRental.Common.Exceptions;

public abstract class CarRentalException : Exception
{
    public abstract HttpStatusCode HttpStatusCode { get; }

    protected CarRentalException(string message) : base(message)
    {
        
    }
}