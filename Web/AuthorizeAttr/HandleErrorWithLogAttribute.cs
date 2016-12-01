using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TracySecretTool.Tools;

namespace Web.AuthorizeAttr
{
    public class HandleErrorWithLogAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            var e = filterContext.Exception;

            string url = filterContext.HttpContext.Request.Url.ToString();
            string queryString = filterContext.HttpContext.Request.Url.Query;
            NameValueCollection formData = filterContext.HttpContext.Request.Form;
            string strFormData = string.Empty;
            for (int i = 0; i < formData.AllKeys.Count(); i++)
            {
                strFormData += formData.AllKeys[i] + "=" + formData[formData.AllKeys[i]] + "&";
            }
            string trace = e.StackTrace;

            LogHelper.WriteMVCLog(url, queryString.Trim('?'), strFormData.Trim('&'), e.InnerException == null ? e.Message : e.InnerException.Message, trace);
        }
    }
}