using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Whyvra.Blazor.Forms.Infrastructure;

namespace Whyvra.Blazor.Forms
{
    public class FormBuilder<TModel>
    {
        private readonly IDictionary<string, Field> _fields = new Dictionary<string, Field>();
        private string _activeField;
        private TModel _model;

        public FormModel<TModel> Build()
        {
            if (_model == null)
            {
                return new FormModel<TModel>
                {
                    Fields = _fields.Values.ToList()
                };
            }

            return new FormModel<TModel>(_model)
            {
                Fields = _fields.Values.ToList()
            };
        }

        public FormBuilder<TModel> AddButton(string text)
        {
            var button = new Button
            {
                Name = text
            };

            _activeField = text;
            _fields.Add(_activeField, button);

            return this;
        }

        public FormBuilder<TModel> DisableUntilValid()
        {
            var field = _fields[_activeField];
            if (field is Button button)
            {
                button.IsDisabledUntilValid = true;
            }

            return this;
        }

        public FormBuilder<TModel> Input<TProperty>(Expression<Func<TModel, TProperty>> propertyLambda)
        {
            var member = propertyLambda.Body as MemberExpression;
            var propertyInfo = member.Member as PropertyInfo;
            var name = propertyInfo.Name;

            var input = new Input
            {
                Name = name,
                DisplayName = AddSpaces(name),
                PropertyInfo = propertyInfo
            };

            _activeField = name;
            _fields.Add(name, input);

            return this;
        }

        public FormBuilder<TModel> OnChange(AsyncEventHandler<ChangeEventArgs> onChangeAsyncHandler)
        {
            var field = _fields[_activeField];
            if (field is Input input)
            {
                input.OnChangeAsyncHandler += onChangeAsyncHandler;
            }
            return this;
        }

        public FormBuilder<TModel> OnChange(EventHandler<ChangeEventArgs> onChangeHandler)
        {
            var field = _fields[_activeField];
            if (field is Input input)
            {
                input.OnChangeHandler += onChangeHandler;
            }
            return this;
        }

        public FormBuilder<TModel> OnClick(AsyncEventHandler<MouseEventArgs> onClickAsyncHandler)
        {
            var field = _fields[_activeField];
            if (field is Button button)
            {
                button.OnClickAsyncHandler += onClickAsyncHandler;
            }

            return this;
        }

        public FormBuilder<TModel> OnClick(EventHandler<MouseEventArgs> onClickHandler)
        {
            var field = _fields[_activeField];
            if (field is Button button)
            {
                button.OnClickHandler += onClickHandler;
            }

            return this;
        }

        public FormBuilder<TModel> TextArea<TProperty>(Expression<Func<TModel, TProperty>> propertyLambda, int? columns = null, int? rows = null)
        {
            var member = propertyLambda.Body as MemberExpression;
            var propertyInfo = member.Member as PropertyInfo;
            var name = propertyInfo.Name;

            var input = new TextArea
            {
                Name = name,
                Columns = columns,
                DisplayName = AddSpaces(name),
                PropertyInfo = propertyInfo,
                Rows = rows
            };

            _activeField = name;
            _fields.Add(name, input);

            return this;
        }

        public FormBuilder<TModel> WithClass(string css)
        {
            var field = _fields[_activeField];
            if (field is Button btn)
            {
                btn.Classes.Add(css);
            }

            return this;
        }

        public FormBuilder<TModel> WithIcon(string icon)
        {
            var field = _fields[_activeField];
            field.Icon = icon;

            return this;
        }

        public FormBuilder<TModel> WithModel(TModel model)
        {
            _model = model;
            return this;
        }

        public FormBuilder<TModel> WithText(string text)
        {
            var field = _fields[_activeField];
            if (field is Input input)
            {
                input.DisplayName = text;
            }

            return this;
        }

        private string AddSpaces(string name)
        {
            var builder = new StringBuilder();
            builder.Append(name[0]);

            for (var i = 1; i < name.Length; i++)
            {
                var c = name[i];
                builder.Append(char.IsUpper(c) ? $" {c}" : $"{c}");
            }

            return builder.ToString();
        }
    }
}