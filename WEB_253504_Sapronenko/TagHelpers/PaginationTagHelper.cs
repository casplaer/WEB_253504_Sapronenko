using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WEB_253504_Sapronenko.UI.TagHelpers
{
    [HtmlTargetElement("pager")]
    public class PaginationTagHelper : TagHelper
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string? Action { get; set; }
        public string? Controller { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "ul";
            output.Attributes.SetAttribute("class", "pagination");

            if (TotalPages <= 1) return;

            if (CurrentPage > 1)
            {
                output.Content.AppendHtml(CreatePageLink("« Предыдущая", CurrentPage - 1));
            }

            for (int i = 1; i <= TotalPages; i++)
            {
                var link = CreatePageLink(i.ToString(), i, i == CurrentPage);
                output.Content.AppendHtml(link);
            }

            if (CurrentPage < TotalPages)
            {
                output.Content.AppendHtml(CreatePageLink("Следующая »", CurrentPage + 1));
            }
        }

        private TagBuilder CreatePageLink(string text, int pageNo, bool isActive = false)
        {
            var li = new TagBuilder("li");
            li.AddCssClass("page-item");

            if (isActive)
            {
                li.AddCssClass("active");
                li.Attributes["aria-current"] = "page"; // Атрибут для текущей страницы
            }

            var button = new TagBuilder("button");
            button.AddCssClass("page-link btn btn-light"); // Классы Bootstrap
            button.Attributes["type"] = "button";
            button.Attributes["onclick"] = $"location.href='/{Controller}/{Action}?pageNo={pageNo}'";
            button.InnerHtml.Append(text);

            li.InnerHtml.AppendHtml(button);
            return li;
        }
    }
}
