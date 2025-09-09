using Microsoft.AspNetCore.Razor.TagHelpers;

[HtmlTargetElement("star-rating")]
public class StarRatingTagHelper : TagHelper
{
    public int Value { get; set; }
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        string stars = "";
        for (int i = 1; i <= 5; i++)
        {
            stars += i <= Value ? "★" : "☆";
        }
        output.Content.SetHtmlContent($"<span style='font-size:20px;color:gold'>{stars}</span>");
    }
}
