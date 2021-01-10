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

        public FormBuilder<TModel> Checkbox<TProperty>(Expression<Func<TModel, TProperty>> propertyLambda)
        {
            var member = propertyLambda.Body as MemberExpression;
            var propertyInfo = member.Member as PropertyInfo;
            var name = propertyInfo.Name;

            var exprPath = propertyLambda.ToString();

            var checkbox = new Checkbox
            {
                Name = name,
                DisplayName = AddSpaces(name),
                Getter = propertyLambda.GetGetter(),
                Setter = propertyLambda.GetSetter(),
                ValidationPath = exprPath.Substring(exprPath.IndexOf('.') + 1)
            };

            _activeField = name;
            _fields.Add(name, checkbox);

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

        public FormBuilder<TModel> HideOnCheck(Func<Input, bool> fieldsToHideFunc)
        {
            var field = _fields[_activeField];
            if (field is Checkbox checkbox)
            {
                checkbox.FieldsToHideFunc = fieldsToHideFunc;
            }

            return this;
        }

        public FormBuilder<TModel> Input<TProperty>(Expression<Func<TModel, TProperty>> propertyLambda)
        {
            var member = propertyLambda.Body as MemberExpression;
            var propertyInfo = member.Member as PropertyInfo;
            var name = propertyInfo.Name;

            var exprPath = propertyLambda.ToString();
            var displayName = AddSpaces(name);

            var input = new Input
            {
                Name = name,
                DisplayName = displayName,
                Getter = propertyLambda.GetGetter(),
                Placeholder = displayName,
                Setter = propertyLambda.GetSetter(),
                ValidationPath = exprPath.Substring(exprPath.IndexOf('.') + 1)
            };

            _activeField = name;
            _fields.Add(name, input);

            return this;
        }

        public FormBuilder<TModel> Number<TProperty>(Expression<Func<TModel, TProperty>> propertyLambda)
        {
            var member = propertyLambda.Body as MemberExpression;
            var propertyInfo = member.Member as PropertyInfo;
            var name = propertyInfo.Name;

            var exprPath = propertyLambda.ToString();

            var number = new Number
            {
                Name = name,
                DisplayName = AddSpaces(name),
                Getter = propertyLambda.GetGetter(),
                Setter = propertyLambda.GetSetter(),
                ValidationPath = exprPath.Substring(exprPath.IndexOf('.') + 1)
            };

            _activeField = name;
            _fields.Add(name, number);

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

        public FormBuilder<TModel> TagsInput<TProperty>(Expression<Func<TModel, TProperty>> propertyLambda)
        {
            var member = propertyLambda.Body as MemberExpression;
            var propertyInfo = member.Member as PropertyInfo;
            var name = propertyInfo.Name;

            var exprPath = propertyLambda.ToString();
            var displayName = AddSpaces(name);

            var tags = new TagsInput
            {
                Name = name,
                DisplayName = displayName,
                Getter = propertyLambda.GetGetter(),
                Placeholder = displayName,
                Setter = propertyLambda.GetSetter(),
                ValidationPath = exprPath.Substring(exprPath.IndexOf('.') + 1)
            };

            _activeField = name;
            _fields.Add(name, tags);

            return this;
        }

        public FormBuilder<TModel> TextArea<TProperty>(Expression<Func<TModel, TProperty>> propertyLambda, int? columns = null, int? rows = null)
        {
            var member = propertyLambda.Body as MemberExpression;
            var propertyInfo = member.Member as PropertyInfo;
            var name = propertyInfo.Name;

            var exprPath = propertyLambda.ToString();
            var displayName = AddSpaces(name);

            var input = new TextArea
            {
                Name = name,
                Columns = columns,
                DisplayName = AddSpaces(name),
                Getter = propertyLambda.GetGetter(),
                Placeholder = displayName,
                Rows = rows,
                Setter = propertyLambda.GetSetter(),
                ValidationPath = exprPath.Substring(exprPath.IndexOf('.') + 1)
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

        public FormBuilder<TModel> WithDefaultEmptyValue(string empty)
        {
            var field = _fields[_activeField];
            if (field is TagsInput tag)
            {
                tag.EmptyValue = empty;
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

        public FormBuilder<TModel> WithPlaceholder(string placeholder)
        {
            var field = _fields[_activeField];
            if (field is Input input)
            {
                input.Placeholder = placeholder;
            }

            return this;
        }

        public FormBuilder<TModel> WithText(string text)
        {
            var field = _fields[_activeField];
            if (field is Input input)
            {
                input.DisplayName = text;
                input.Placeholder = text;
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