#pragma checksum "D:\projects\working_copy\CWB\CWB.App\Views\Shared\_CustomerSuppliedRM.cshtml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "1a2670400eaafc9710dfb8c566dc515204969fd3d54a5be6aa366afaf729c08f"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared__CustomerSuppliedRM), @"mvc.1.0.view", @"/Views/Shared/_CustomerSuppliedRM.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA256", @"1a2670400eaafc9710dfb8c566dc515204969fd3d54a5be6aa366afaf729c08f", @"/Views/Shared/_CustomerSuppliedRM.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA256", @"d5ea8e6831a1ee80032874d8c95796e9e8392b4c92855acd058fadc59f67b90e", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Shared__CustomerSuppliedRM : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    #nullable disable
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("value", "1", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("value", "2", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"<div class=""modal fade"" id=""rm-select-cust"" data-bs-backdrop=""static"" data-bs-keyboard=""false"" tabindex=""-1"" aria-labelledby=""staticBackdropLabel"" aria-hidden=""true"">
    <div class=""modal-dialog modal-lg"">
        <div class=""modal-content"">
            <div class=""modal-header"">
                <h4 class=""modal-title"" id=""myLargeModalLabel"">Customer Supplied RM Selection : Customer - <span id=""RMSupplier""></span></h4>
                <button type=""button"" class=""btn-close"" id=""btn-close-CustRM"" data-bs-dismiss=""modal"" aria-label=""Close""></button>
            </div>
            <div class=""modal-body"">
                <!-- ===================== -->
                <div class=""row mb-1"">
                    <label class=""form-label col-md-2 mb-2""> Part Number </label>
                    <div class=""col-md-4 mb-2"" title=""Enter the Part No partially or fully to shortlist"" data-plugin=""tippy"" data-tippy-placement=""top"">
                        <input id=""CPartNo"" name=""CPartNo"" class=""form-control f");
            WriteLiteral(@"orm-control-sm"" type=""text"" placeholder=""enter here"">
                    </div>
                    <!-- =============== -->
                    <label class=""form-label col-md-2 mb-2"">Part Description </label>
                    <div class=""col-md-4 mb-2"" title=""Enter hte Part Description partially or fully to shortlist"" data-plugin=""tippy"" data-tippy-placement=""top"">
                        <textarea id=""CPartDescription"" name=""CPartDescription"" class=""form-control form-control-sm""></textarea>
                    </div>
                    <!-- =============== -->
                    <label class=""form-label col-md-2 mb-2""> RM Category</label>
                    <div class=""col-md-4 mb-2"" title=""Select the Raw Material Category  from the  list"" data-plugin=""tippy"" data-tippy-placement=""top"">
                        <select class=""form-select form-select-sm"" id=""CRawMaterialMadeSubType"" name=""RawMaterialMadeSubType"">
                            ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "1a2670400eaafc9710dfb8c566dc515204969fd3d54a5be6aa366afaf729c08f6141", async() => {
                WriteLiteral(" Made to Print ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            BeginWriteTagHelperAttribute();
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __tagHelperExecutionContext.AddHtmlAttribute("selected", Html.Raw(__tagHelperStringValueBuffer), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.Minimized);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                            ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "1a2670400eaafc9710dfb8c566dc515204969fd3d54a5be6aa366afaf729c08f7672", async() => {
                WriteLiteral(" Standard Purchased RM ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(@"
                        </select>
                    </div>
                    <!-- ------------------- -->
                    <label class=""form-label col-md-2 mb-2""> RM Type</label>
                    <div class=""col-md-4 mb-2"" title=""Select the Raw Material Type from the list (Casting, Forging, Plate, Bar ..."" data-plugin=""tippy"" data-tippy-placement=""top"">
                        <input class=""form-control form-control-sm"" type=""text"" id=""CRawMaterialTypeId"" name=""CRawMaterialTypeId"" placeholder=""enter here"">

                    </div>
                    <!-- ------------------- -->
                    <label class=""form-label col-md-2 mb-2""> Base Material</label>
                    <div class=""col-md-4 mb-2"" title=""Select the Base Raw Material from the list (Steel, Brass ....)"" data-plugin=""tippy"" data-tippy-placement=""top"">
                        <input class=""form-control form-control-sm"" type=""text"" id=""CBaseRawMaterialId"" name=""CBaseRawMaterialId"" placeholder=""enter here"">

   ");
            WriteLiteral(@"                 </div>
                    <!-- ------------------- -->
                    <label class=""form-label col-md-2 mb-1"">Keyword / Desc</label>
                    <div  class=""col-md-4 mb-2"" title=""Enter any word that is related to the Raw Material to shortlist"" data-plugin=""tippy"" data-tippy-placement=""top"">
                        <input id=""CKeyWord"" name=""CKeyWord"" class=""form-control form-control-sm"" type=""text"" placeholder=""enter here"">
                    </div>
                    <!-- =============== -->
                </div>
                <!-- ====================================== -->
                <div class=""row mt-3"">
                    <div class=""table-responsive mt-1 table-he-180"">
                        <table id=""CustRMTable"" class=""table table-sm table-bordered w-100 mb-3 tableFixHead"">
                            <thead class=""  table-info th-sti text-center"">
                                <tr class=""table-border-bottom"">
                               ");
            WriteLiteral(@"     <th width=""5%""></th>
                                    <th>Part No</th>
                                    <th>Description</th>
                                    <th>Notes</th>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            <div class=""modal-footer"">
                <button id=""CSelect"" type=""button"" class=""btn btn-primary btn-sm"" onclick=""copyCustData()"">Select</button>                
            </div>
        </div>
        <!-- /.modal-content -->
    </div>
    <!-- /.modal-dialog -->
</div>
<template id=""CustomerSuppliedRMTemplate"">
    <tr>
        <td>
            <input class=""form-check-input mt-1"" type=""radio"" name=""CSRM"" value=""{num}"">
        </td>
        <td data-key=""partNo"">{partNo}</td>
        <td data-key=""partDescription"">{partDescription}</");
            WriteLiteral(@"td>
        <td data-key=""rawMaterialNotes"">{rawMaterialNotes}</td>
        <td style=""visibility:collapse"" data-key=""rawMaterialDetailId"">{rawMaterialDetailId}</td>
        <td style=""visibility:collapse"" data-key=""rmMadeSubType"">{rawMaterialMadeSubType}</td>
        <td style=""visibility:collapse"" data-key=""rmType"">{rawMaterialType}</td>
        <td style=""visibility:collapse"" data-key=""baseRM"">{baseRawMaterial}</td>
    </tr>
</template>


");
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