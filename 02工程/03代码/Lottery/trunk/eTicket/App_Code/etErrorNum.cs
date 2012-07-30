using System;

/// <summary>
/// Error Code from eTickets into a System Code
/// </summary>
public class etErrorNum
{
    public const string Success = "0000";
    public const string TimedOut = "0001";      // Server timed out
    public const string AgentFailure = "0002";  // Account error, check with provider
    public const string GeneralError = "0003";  // General error, or unknown error
    
    #region Message body related
    public class Message
    {
        public const string FormatError = "1000";   // Request format error
        public const string SignatureErr = "1001";  // Signature error
    }
    #endregion

    #region Issue related
    public class Issue
    {
        public const string Error = "2000";     // General errors with issue id
        public const string NA = "2001";        // Issue not available
        public const string Expired = "2002";   // Issue is expired, past issue end time
    }
    #endregion

    #region Ticket related
    public class Ticket
    {
        public const string WrongAmount = "3000";   // Total amount of the ticket is incorrect
        public const string WrongFormat = "3001";   // Ticket format incorrect
    }
    #endregion

    public etErrorNum() {}
}