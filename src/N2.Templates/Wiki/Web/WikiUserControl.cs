﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using N2.Engine;
using N2.Templates.Web.UI;

namespace N2.Templates.Wiki.Web
{
    public class WikiUserControl<T> : TemplateUserControl<T>, IWikiTemplate
        where T:Items.AbstractContentPage, IArticle
    {
        protected bool IsAuthorized
        {
            get
            {
                if (Engine.SecurityManager.IsEditor(Page.User))
                    return true;

                return Engine.SecurityManager.IsAuthorized(Page.User, CurrentPage.WikiRoot.ModifyRoles);
            }
        }

        ViewContext viewed = null;
        public ViewContext Viewed 
        {
            get { return viewed ?? (viewed = new ViewContext { Article = CurrentPage as IArticle, Fragment = null }); }
            set { viewed = value; }
        }
    }
}