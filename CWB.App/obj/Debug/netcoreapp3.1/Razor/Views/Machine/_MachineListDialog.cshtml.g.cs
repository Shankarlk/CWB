#pragma checksum "D:\projects\working_copy\CWB\CWB.App\Views\Machine\_MachineListDialog.cshtml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "ac51dd07fda32be8d72203360bd5eaeb693321e9476ca20f32b6eb6a57e15287"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Machine__MachineListDialog), @"mvc.1.0.view", @"/Views/Machine/_MachineListDialog.cshtml")]
namespace AspNetCore
{
    #line hidden
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::System.Threading.Tasks;
    using global::Microsoft.AspNetCore.Mvc;
    using global::Microsoft.AspNetCore.Mvc.Rendering;
    using global::Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "D:\projects\working_copy\CWB\CWB.App\Views\_ViewImports.cshtml"
using CWB.App;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\projects\working_copy\CWB\CWB.App\Views\_ViewImports.cshtml"
using CWB.App.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "D:\projects\working_copy\CWB\CWB.App\Views\_ViewImports.cshtml"
using CWB.Constants.UserIdentity;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA256", @"ac51dd07fda32be8d72203360bd5eaeb693321e9476ca20f32b6eb6a57e15287", @"/Views/Machine/_MachineListDialog.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA256", @"d5ea8e6831a1ee80032874d8c95796e9e8392b4c92855acd058fadc59f67b90e", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Machine__MachineListDialog : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    #nullable disable
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("name", "_MachineModelHeader", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"<div id=""dialog-machine"" class=""modal fade"" data-bs-backdrop=""static"" data-bs-keyboard=""false"" tabindex=""-1"" aria-labelledby=""staticBackdropLabel"" aria-hidden=""true"">
    <div class=""modal-dialog modal-full-width"">
        <div class=""modal-content"">
            <div class=""modal-header"">
                <div class=""col-md-12"">
                    <div class=""table-responsive"">
                        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("partial", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "ac51dd07fda32be8d72203360bd5eaeb693321e9476ca20f32b6eb6a57e152874176", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper.Name = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(@"
                    </div>
                </div>
            </div>
            <div class=""modal-body"">
                <ul class=""nav nav-pills navtab-bg nav-justified machine-tabs"">
                    <li class=""nav-item"">
                        <a href=""#machine-general"" data-bs-toggle=""tab"" aria-expanded=""false"" class=""nav-link active"">
                            <span class=""d-inline-block d-sm-none"">General</span>
                            <span class=""d-none d-sm-inline-block"">General</span>
                        </a>
                    </li>
                    <li class=""nav-item"">
                        <a href=""#machine-process-doc"" data-bs-toggle=""tab"" aria-expanded=""true"" class=""nav-link machine-tab-withdata disabled"">
                            <span class=""d-inline-block d-sm-none"">Process Document List</span>
                            <span class=""d-none d-sm-inline-block"">Process Document List</span>
                        </a>
                    </li>
                    <li");
            WriteLiteral(@" class=""nav-item"">
                        <a href=""#machine-cost-details"" data-bs-toggle=""tab"" aria-expanded=""false"" class=""nav-link machine-tab-withdata disabled"">
                            <span class=""d-inline-block d-sm-none"">Cost Details</span>
                            <span class=""d-none d-sm-inline-block"">Cost Details</span>
                        </a>
                    </li>
                    <li class=""nav-item"">
                        <a href=""#machine-maintenance-info"" data-bs-toggle=""tab"" aria-expanded=""false"" class=""nav-link machine-tab-withdata disabled"">
                            <span class=""d-inline-block d-sm-none"">Maintenance Info</span>
                            <span class=""d-none d-sm-inline-block"">Maintenance Info</span>
                        </a>
                    </li>
                    <li class=""nav-item"">
                        <a href=""#machine-pm-schedule"" data-bs-toggle=""tab"" aria-expanded=""false"" class=""nav-link machine-tab-withdata disabled"">
           ");
            WriteLiteral(@"                 <span class=""d-inline-block d-sm-none"">PM Schedule</span>
                            <span class=""d-none d-sm-inline-block"">PM Schedule</span>
                        </a>
                    </li>
                </ul>
                <div class=""tab-content"">
                    ");
#nullable restore
#line 45 "D:\projects\working_copy\CWB\CWB.App\Views\Machine\_MachineListDialog.cshtml"
               Write(Html.Partial("_MachineGeneralDialog", new CWB.App.Models.Machine.MachineVM()));

#line default
#line hidden
#nullable disable
            WriteLiteral("\n                    <!-- ================= -->\n                    ");
#nullable restore
#line 47 "D:\projects\working_copy\CWB\CWB.App\Views\Machine\_MachineListDialog.cshtml"
               Write(Html.Partial("_MachineProcessDocDialog"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\n                    <!-- ================= -->\n                    ");
#nullable restore
#line 49 "D:\projects\working_copy\CWB\CWB.App\Views\Machine\_MachineListDialog.cshtml"
               Write(Html.Partial("_MachineCostDetailsDialog"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\n                    <!-- ================= -->\n                    ");
#nullable restore
#line 51 "D:\projects\working_copy\CWB\CWB.App\Views\Machine\_MachineListDialog.cshtml"
               Write(Html.Partial("_MachineMaintenanceInfoDialog"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\n                    <!-- ================= -->\n                    ");
#nullable restore
#line 53 "D:\projects\working_copy\CWB\CWB.App\Views\Machine\_MachineListDialog.cshtml"
               Write(Html.Partial("_MachinePMScheduleDialog"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\n                    <!-- ================= -->\n                </div>\n            </div>\n        </div>\n        <!-- /.modal-content -->\n    </div>\n    <!-- /.modal-dialog -->\n</div>");
        }
        #pragma warning restore 1998
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591