using System;
using System.Collections.Generic;
using System.Text;
using Specify.Configuration;

namespace CQRSlite.Extensions.SQLStreamStore.Testing
{
    public class SpecBootstrapper : DefaultBootstrapper
    {
        public SpecBootstrapper()
        {

            LoggingEnabled = true;
            HtmlReport.ReportType = HtmlReportConfiguration.HtmlReportType.Metro;
        }
    }
}
