using System;
using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace Whyvra.Blazor.Forms.Infrastructure
{
    public class Input : Field
    {
        public string DisplayName { get; set; }

        public PropertyInfo PropertyInfo { get; set; }

        public AsyncEventHandler<ChangeEventArgs> OnChangeAsyncHandler { get; set; }

        public EventHandler<ChangeEventArgs> OnChangeHandler { get; set; }
    }
}