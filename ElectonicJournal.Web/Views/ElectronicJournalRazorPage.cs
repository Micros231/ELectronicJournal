using ElectronicJournal.Consts;
using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.Web.Views
{
    public abstract class ElectronicJournalRazorPage<TModel> : RazorPage<TModel>
    {
        public string ApplicationPath 
        {
            get
            {
                var appPath = Context.Request.PathBase.Value;
                if (appPath == null)
                {
                    return "/";
                }
                return EnsureEndsWith(appPath, '/');
            }
        }
        public string WebName => WebConsts.WebName;
        private string EnsureEndsWith(string value, char endChar)
        {
            if (string.IsNullOrEmpty(value))
            {
                return endChar.ToString();
            }
            var lastValue = value.Last();
            if (lastValue.Equals(endChar))
            {
                return value;
            }
            value += endChar;
            return value;
        }

    }
}
