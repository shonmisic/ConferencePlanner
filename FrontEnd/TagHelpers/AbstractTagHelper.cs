using System;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace FrontEnd.TagHelpers
{
    [HtmlTargetElement("abstract", Attributes = "abstract-text")]
    public class AbstractTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var text = context.AllAttributes["abstract-text"].Value.ToString();
            foreach (var para in text.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
            {
                output.Content.AppendHtml($@"<p>{para}</p>");
            }
            output.Attributes.Clear();
        }
    }
}
