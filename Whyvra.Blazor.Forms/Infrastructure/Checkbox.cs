using System;

namespace Whyvra.Blazor.Forms.Infrastructure
{
    public class Checkbox : Input
    {
        public Func<Input, bool> FieldsToHideFunc { get; set; }
    }
}