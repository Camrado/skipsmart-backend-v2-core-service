namespace SkipSmart.Application.Exceptions;

public class DuplicateEmailException : Exception {
    public DuplicateEmailException(string message, Exception innerException) : base(message, innerException) {
        
    }
}