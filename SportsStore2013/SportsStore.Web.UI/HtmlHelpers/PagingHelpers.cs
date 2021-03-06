﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Linq;
using SportsStore.Core.Contracts.Models;
using SportsStore.Web.UI.Models;

namespace SportsStore.Web.UI.HtmlHelpers
{
    public static class PagingHelpers
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html, PagingInfo pagingInfo, Func<int, string> pageUrl)
        {
            // Don't show navigation if there is only one page
            if (pagingInfo.TotalPages <= 1)
                return MvcHtmlString.Create(string.Empty);

            // If we get here, that means more than one page - so display nav
            StringBuilder li = new StringBuilder();
            TagBuilder ulTag = new TagBuilder("ul");
            for (int i = 1; i <= pagingInfo.TotalPages; i++)
            {
                TagBuilder liTag = new TagBuilder("li");

                TagBuilder anchorTag = new TagBuilder("a"); // Construct an <a> tag
                anchorTag.MergeAttribute("href", pageUrl(i));
                anchorTag.InnerHtml = i.ToString();
                if (i == pagingInfo.CurrentPage)
                    anchorTag.AddCssClass("selected");

                liTag.InnerHtml = anchorTag.ToString();

                li.Append(liTag.ToString());
            }

            ulTag.InnerHtml = li.ToString();

            return MvcHtmlString.Create(ulTag.ToString());
        }

        public static IEnumerable<SelectListItem> ToSelectListItem(this IEnumerable<int> quantities, int selectedQuantity)
        {
            return quantities.OrderBy(q => q)
                             .Select(q => new SelectListItem
                                              {
                                                  Selected = (q == selectedQuantity),
                                                  Text = q.ToString(),
                                                  Value = q.ToString()
                                              });
        }

        public static IEnumerable<SelectListItem> ToSelectListItem(this IEnumerable<Category> categories, int currentCategoryId)
        {
            return categories.OrderBy(c => c.Name)
                             .Select(c => new SelectListItem
                                              {
                                                  Selected = c.Id == currentCategoryId,
                                                  Text = c.Name,
                                                  Value = c.Id.ToString()
                                              });
        }
    }
}