using System;
using System.Web;
using Kraken.Core;
using Common.Logging;

namespace Kraken.Web
{
    /// <summary>
    /// This HttpModule handles the HttpApplication.Error event, publishes the Exception
    /// and optionally re-directs to a specified error page.
    /// </summary>
    class ExceptionManagementHttpModule : IHttpModule
    {
        static ILog log = LogManager.GetLogger< ExceptionManagementHttpModule>();
        #region IHttpModule Members

        public void Init(HttpApplication context)
        {
            context.Error += ContextError;
        }

        public void Dispose()
        {
        }

        #endregion

        private static void ContextError(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            HttpRequest request = application.Context.Request;
            HttpResponse response = application.Context.Response;
            Exception ex = application.Server.GetLastError();


            string hint = string.Format(
                "User='{0}' experienced an exception at '{1}'"
                , WebLogic.ClientIdentity
                , request.RawUrl);

            log.Fatal(hint, ex);

            string errorPageUrl = KrakenConfig.GetAppSettingAsString("ExceptionManagementHttpModule.ErrorPageUrl", false);

            // Only continue with the redirect behaviour if ExceptionManagement is turned on
            if (string.IsNullOrEmpty(errorPageUrl))
            {
                log.Warn(m => m("AppSetting 'ExceptionManagementHttpModule.ErrorPageUrl' is not present. {0} will see hard error.", WebLogic.ClientIdentity));
            }
            else
            {
                // Extract the page name from errorPageUrl setting (ignore everything up to final /)
                // This is to ensure that "http://myserver/myapp/Error.aspx" will match with errorPageUrl="~/Error.aspx"
                // in the infinite-redirect-prevention check that follows
                string errorPageName = errorPageUrl;
                int lastSlashPos = errorPageUrl.LastIndexOf("/");

                if (lastSlashPos > -1 && lastSlashPos < errorPageUrl.Length - 1)
                {
                    errorPageName = errorPageUrl.Substring(lastSlashPos + 1);
                }

                // Before re-directing to Error page, make sure current request is NOT the error page
                // (avoids infinite redirect loop)
                if (request.Url.PathAndQuery.ToLower().IndexOf(errorPageName.ToLower()) < 0)
                {
                    response.Redirect(errorPageUrl);
                }
            }
        }
    }
}
