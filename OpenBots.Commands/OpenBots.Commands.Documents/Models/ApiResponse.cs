using System;
using System.Linq;

namespace OpenBots.Commands.Documents.Models
{
    /// <summary>
    /// This class is used to create standard responses for AJAX/remote requests.
    /// </summary>
    [Serializable]
    public class ApiResponse : ApiResponse<object>
    {
        /// <summary>
        /// Creates an <see cref="ApiResponse"/> object.
        /// <see cref="ApiResponse.Success"/> is set as true.
        /// </summary>
        public ApiResponse()
        {    
        }

        /// <summary>
        /// Creates an <see cref="ApiResponse"/> object with <see cref="ApiResponse.Success"/> specified.
        /// </summary>
        /// <param name="success">Indicates success status of the result</param>
        public ApiResponse(bool success) : base(success)
        {       
        }

        /// <summary>
        /// Creates an <see cref="ApiResponse"/> object with <see cref="ApiResponse{TResult}.Result"/> specified.
        /// <see cref="ApiResponse.Success"/> is set as true.
        /// </summary>
        /// <param name="result">The actual result object</param>
        public ApiResponse(object result) : base(result)
        {       
        }

        /// <summary>
        /// Creates an <see cref="ApiResponse"/> object with <see cref="ApiResponse.Error"/> specified.
        /// <see cref="ApiResponse.Success"/> is set as false.
        /// </summary>
        /// <param name="error">Error details</param>
        /// <param name="unAuthorizedRequest">Used to indicate that the current user has no privilege to perform this request</param>
        public ApiResponse(ErrorInfo error, bool unAuthorizedRequest = false) : base(error, unAuthorizedRequest)
        {
        }
    }

    public class ApiResponse<TResult>
    {
        /// <summary>
        /// The actual result object of AJAX request.
        /// It is set if <see cref="ApiResponse.Success"/> is true.
        /// </summary>
        public TResult Result { get; set; }
        //
        // Summary:
        //     This property can be used to redirect user to a specified URL.
        public string TargetUrl { get; set; }
        //
        // Summary:
        //     Indicates success status of the result. Set Abp.Web.Models.AjaxResponseBase.Error
        //     if this value is false.
        public bool Success { get; set; }
        //
        // Summary:
        //     Error details (Must and only set if Abp.Web.Models.AjaxResponseBase.Success is
        //     false).
        public ErrorInfo Error { get; set; }
        //
        // Summary:
        //     This property can be used to indicate that the current user has no privilege
        //     to perform this request.
        public bool UnAuthorizedRequest { get; set; }
        //
        // Summary:
        //     A special signature for AJAX responses. It's used in the client to detect if
        //     this is a response wrapped by ABP.
        public bool __abp { get; }


        /// <summary>
        /// Creates an <see cref="ApiResponse"/> object.
        /// <see cref="ApiResponse.Success"/> is set as true.
        /// </summary>
        public ApiResponse()
        { 
        }

        /// <summary>
        /// Creates an <see cref="ApiResponse"/> object with <see cref="ApiResponse.Success"/> specified.
        /// </summary>
        /// <param name="success">Indicates success status of the result</param>
        public ApiResponse(bool success) 
        {
            Success = success;
        }

        /// <summary>
        /// Creates an <see cref="ApiResponse"/> object with <see cref="ApiResponse{TResult}.Result"/> specified.
        /// <see cref="ApiResponse.Success"/> is set as true.
        /// </summary>
        /// <param name="result">The actual result object</param>
        public ApiResponse(TResult result) 
        {
            Result = result;
        }

        /// <summary>
        /// Creates an <see cref="ApiResponse"/> object with <see cref="ApiResponse.Error"/> specified.
        /// <see cref="ApiResponse.Success"/> is set as false.
        /// </summary>
        /// <param name="error">Error details</param>
        /// <param name="unAuthorizedRequest">Used to indicate that the current user has no privilege to perform this request</param>
        public ApiResponse(ErrorInfo error, bool unAuthorizedRequest = false)
        {
            Error = error;
            UnAuthorizedRequest = unAuthorizedRequest;
        }
    }

    /// <summary>
    /// Used to store information about a validation error.
    /// </summary>
    [Serializable]

    public class ValidationErrorInfo
    {
        /// <summary>
        /// Validation error message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Relate invalid members (fields/properties).
        /// </summary>
        public string[] Members { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="ValidationErrorInfo"/>.
        /// </summary>
        public ValidationErrorInfo()
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="ValidationErrorInfo"/>.
        /// </summary>
        /// <param name="message">Validation error message</param>
        public ValidationErrorInfo(string message)
        {
            Message = message;
        }

        /// <summary>
        /// Creates a new instance of <see cref="ValidationErrorInfo"/>.
        /// </summary>
        /// <param name="message">Validation error message</param>
        /// <param name="members">Related invalid members</param>
        public ValidationErrorInfo(string message, string[] members) : this(message)
        {
            Members = members;
        }

        /// <summary>
        /// Creates a new instance of <see cref="ValidationErrorInfo"/>.
        /// </summary>
        /// <param name="message">Validation error message</param>
        /// <param name="member">Related invalid member</param>
        public ValidationErrorInfo(string message, string member) : this(message, new[] { member })
        {
        }
    }

    /// <summary>
    /// Used to store information about an error.
    /// </summary>
    [Serializable]
    public class ErrorInfo
    {
        /// <summary>
        /// Error code.
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Error message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Error details.
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// Validation errors if exists.
        /// </summary>
        public ValidationErrorInfo[] ValidationErrors { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="ErrorInfo"/>.
        /// </summary>
        public ErrorInfo()
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="ErrorInfo"/>.
        /// </summary>
        /// <param name="message">Error message</param>
        public ErrorInfo(string message)
        {
            Message = message;
        }

        /// <summary>
        /// Creates a new instance of <see cref="ErrorInfo"/>.
        /// </summary>
        /// <param name="code">Error code</param>
        public ErrorInfo(int code)
        {
            Code = code;
        }

        /// <summary>
        /// Creates a new instance of <see cref="ErrorInfo"/>.
        /// </summary>
        /// <param name="code">Error code</param>
        /// <param name="message">Error message</param>
        public ErrorInfo(int code, string message) : this(message)
        {
            Code = code;
        }

        /// <summary>
        /// Creates a new instance of <see cref="ErrorInfo"/>.
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="details">Error details</param>
        public ErrorInfo(string message, string details) : this(message)
        {
            Details = details;
        }

        /// <summary>
        /// Creates a new instance of <see cref="ErrorInfo"/>.
        /// </summary>
        /// <param name="code">Error code</param>
        /// <param name="message">Error message</param>
        /// <param name="details">Error details</param>
        public ErrorInfo(int code, string message, string details) : this(message, details)
        {
            Code = code;
        }

        public string GetConsolidatedMessage()
        {
            string consolidatedMessage = Message;
            if (ValidationErrors != null && ValidationErrors.Any())
                consolidatedMessage += ValidationErrors.Select(e => e.Message); //.JoinAsString(Environment.NewLine + " * ");

            return consolidatedMessage;
        }
    }
}
