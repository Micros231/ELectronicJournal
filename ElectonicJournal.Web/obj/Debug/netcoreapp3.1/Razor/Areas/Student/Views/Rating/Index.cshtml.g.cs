#pragma checksum "D:\Job\repos\ElectonicJournal\ElectonicJournal.Web\Areas\Student\Views\Rating\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "2fd014e33a63889e5f26cdf08c8ba5fcd78063f4"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_Student_Views_Rating_Index), @"mvc.1.0.view", @"/Areas/Student/Views/Rating/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "D:\Job\repos\ElectonicJournal\ElectonicJournal.Web\Areas\Student\Views\_ViewImports.cshtml"
using ElectronicJournal.Web.Areas.Startup;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "D:\Job\repos\ElectonicJournal\ElectonicJournal.Web\Areas\Student\Views\Rating\Index.cshtml"
using ElectronicJournal.Web.Areas.Student.Models.Rating;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"2fd014e33a63889e5f26cdf08c8ba5fcd78063f4", @"/Areas/Student/Views/Rating/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a553fea1d1ddb3dd00252d1b22e1f5fc26917ebb", @"/Areas/Student/Views/_ViewImports.cshtml")]
    public class Areas_Student_Views_Rating_Index : ElectronicJournal.Web.Views.ElectronicJournalRazorPage<GetRatingViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 4 "D:\Job\repos\ElectonicJournal\ElectonicJournal.Web\Areas\Student\Views\Rating\Index.cshtml"
  
    ViewBag.CurrentPageName = ElectronicJournalPageNames.PageNames.StudentRating;
    ViewBag.Title = ElectronicJournalPageNames.DisplayPageNames.StudentRating;

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
    <h1>
        Рейтинг
    </h1>
    <table class=""table table-bordered table-sm"">
        <thead>
            <tr>
                <th scope=""col"">Имя</th>
                <th scope=""col"">Средний балл</th>
            </tr>
        </thead>
        <tbody>
");
#nullable restore
#line 20 "D:\Job\repos\ElectonicJournal\ElectonicJournal.Web\Areas\Student\Views\Rating\Index.cshtml"
             foreach (var studentRating in Model.StudentRatings)
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                <tr>\r\n                    <th scope=\"row\">");
#nullable restore
#line 23 "D:\Job\repos\ElectonicJournal\ElectonicJournal.Web\Areas\Student\Views\Rating\Index.cshtml"
                               Write(studentRating.Student.FullName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</th>\r\n                    <th>\r\n                        ");
#nullable restore
#line 25 "D:\Job\repos\ElectonicJournal\ElectonicJournal.Web\Areas\Student\Views\Rating\Index.cshtml"
                   Write(studentRating.Score.ToString("0.00"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </th>\r\n                </tr>\r\n");
#nullable restore
#line 28 "D:\Job\repos\ElectonicJournal\ElectonicJournal.Web\Areas\Student\Views\Rating\Index.cshtml"
            }

#line default
#line hidden
#nullable disable
            WriteLiteral("        </tbody>\r\n    </table>\r\n\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<GetRatingViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
