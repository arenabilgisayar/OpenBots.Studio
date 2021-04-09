using System;

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
}
