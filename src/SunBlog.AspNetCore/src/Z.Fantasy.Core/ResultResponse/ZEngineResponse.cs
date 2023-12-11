using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.Fantasy.Core.ResultResponse
{
    [Serializable]
    public class ZEngineResponse<TResult> : ZResponseBase
    {
        public TResult? Result { get; set; }

        public ZEngineResponse(TResult result)
        {
            Result = result;
            Success = true;
        }

        public ZEngineResponse(int code, bool isSuccess)
        {
            StatusCode = code;
            Success = isSuccess;
        }

        public ZEngineResponse()
        {
            Success = true;
        }

        public ZEngineResponse(ErrorInfo error, bool unAuthorizedRequest = false)
        {
            Error = error;
            UnAuthorizedRequest = unAuthorizedRequest;
            Success = false;
        }
    }

    [Serializable]
    public class ZEngineResponse : ZEngineResponse<object>
    {
        public ZEngineResponse(object result) : base(result)
        {
        }

        public ZEngineResponse(int code, bool isSuccess) : base(code, isSuccess)
        {
        }
        public ZEngineResponse(ErrorInfo error, bool unAuthorizedRequest) : base(error, unAuthorizedRequest)
        {

        }

        public ZEngineResponse(object result, bool _unAuthorizedRequest) : base(result)
        {
            UnAuthorizedRequest = _unAuthorizedRequest;
        }


        public ZEngineResponse() : base() { }
    }
}
