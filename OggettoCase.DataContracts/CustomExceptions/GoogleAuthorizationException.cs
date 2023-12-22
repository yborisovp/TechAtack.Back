namespace OggettoCase.DataContracts.CustomExceptions;

public class GoogleAuthorizationException(string exceptionMessage) : Exception(exceptionMessage);