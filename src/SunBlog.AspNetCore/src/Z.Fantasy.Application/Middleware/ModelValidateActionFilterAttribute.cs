using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Fantasy.Core.Exceptions;
using Z.Fantasy.Core.Extensions;
using Cuemon.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Http;

namespace Z.Fantasy.Application.Middleware
{
    /// <summary>
    /// 模型验证
    /// </summary>
    public class ModelValidateActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                //获取具体的错误消息
                // 获取失败的验证信息列表
                var errors = context.ModelState
                    .Where(s => s.Value != null && s.Value.ValidationState == ModelValidationState.Invalid)
                    .Select(e => new 
                    {
                       Key = e.Key,
                       Message = e.Value.Errors.Select(p=>p.ErrorMessage)

                    }).ToDictionary(p=>p.Key,c=>c.Message);

                context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

                context.Result= new BadRequestObjectResult(errors);
            }
        }
    }
}
