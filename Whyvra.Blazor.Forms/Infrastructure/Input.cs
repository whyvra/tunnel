using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Whyvra.Blazor.Forms.Infrastructure
{
    public class Input : Field
    {
        public string DisplayName { get; set; }

        public Func<object, object> Getter { get; set; }

        public Action<object, object> Setter { get; set; }

        public AsyncEventHandler<ChangeEventArgs> OnChangeAsyncHandler { get; set; }

        public EventHandler<ChangeEventArgs> OnChangeHandler { get; set; }

        public IEnumerable<string> ValidationMessages { get; set; } = new List<string>();

        public string ValidationPath { get; set; }
    }
}