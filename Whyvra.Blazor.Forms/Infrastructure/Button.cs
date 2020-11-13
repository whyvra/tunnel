using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Web;

namespace Whyvra.Blazor.Forms.Infrastructure
{
    public class Button : Field
    {
        public IList<string> Classes { get; } = new List<string>();

        public bool IsDisabledUntilValid { get; set; }

        public EventHandler<MouseEventArgs> OnClickHandler { get; set; }

        public AsyncEventHandler<MouseEventArgs> OnClickAsyncHandler { get; set; }
    }
}