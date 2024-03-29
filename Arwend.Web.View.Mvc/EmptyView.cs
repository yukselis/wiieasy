﻿using System;
using System.IO;
using System.Web.Mvc;

namespace Arwend.Web.View.Mvc
{
    public class EmptyView : IView
    {
        public void Render(ViewContext viewContext, TextWriter writer)
        {
            throw new InvalidOperationException();
        }
    }
}
