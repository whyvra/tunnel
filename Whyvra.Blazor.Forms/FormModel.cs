using System;
using System.Collections.Generic;
using Whyvra.Blazor.Forms.Infrastructure;

namespace Whyvra.Blazor.Forms
{
    public class FormModel<TModel>
    {
        public TModel DataModel { get; set; }

        public IEnumerable<Field> Fields { get; set; }

        public FormModel()
        {
            DataModel = Activator.CreateInstance<TModel>();
        }

        public FormModel(TModel model)
        {
            DataModel = model;
        }
    }
}