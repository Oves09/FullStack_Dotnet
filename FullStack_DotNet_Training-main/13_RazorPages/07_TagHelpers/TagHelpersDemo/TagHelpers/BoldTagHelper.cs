using Microsoft.AspNetCore.Razor.TagHelpers;

namespace TagHelpersDemo.TagHelpers
{
    [HtmlTargetElement("bold-text")]
    public class BoldTagHelper : TagHelper
    {
        public string Text { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "strong"; // Replace <bold-text> with <strong>
            output.Content.SetContent(Text);
        }
    }
}
