﻿using OnePassword.Common;
using OnePassword.Items;
using OnePassword.Templates;

namespace OnePassword;

public sealed partial class OnePasswordManager
{
    /// <inheritdoc />
    public ImmutableList<TemplateInfo> GetTemplates()
    {
        const string command = "item template list";
        return Op<ImmutableList<TemplateInfo>>(command);
    }

    /// <inheritdoc />
    public Template GetTemplate(ITemplate template)
    {
        if (template.Name.Length == 0)
            throw new ArgumentException($"{nameof(template.Name)} cannot be empty.", nameof(template));

        var command = $"item template get \"{template.Name}\"";
        var result = Op<Template>(command);

        result.Name = template.Name;

        return result;
    }

    /// <inheritdoc />
    public Template GetTemplate(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException($"{nameof(name)} cannot be empty.", nameof(name));

        var command = $"item template get \"{name}\"";
        var result = Op<Template>(command);

        result.Name = name;

        return result;
    }

    /// <inheritdoc />
    public Template GetTemplate(Category category)
    {
        if (category is Category.Unknown or Category.Custom)
            throw new ArgumentException($"{nameof(category)} cannot be {nameof(Category.Unknown)} or {nameof(Category.Custom)}.", nameof(category));

        var templateName = category.ToEnumString();

        var command = $"item template get \"{templateName}\"";
        var result = Op<Template>(command);

        result.Name = templateName;

        return result;
    }
}