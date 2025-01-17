﻿using OnePassword.Common;
using OnePassword.Documents;
using OnePassword.Vaults;

namespace OnePassword;

public sealed partial class OnePasswordManager
{
    /// <inheritdoc />
    public ImmutableList<DocumentDetails> GetDocuments(IVault vault)
    {
        if (vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        var command = $"document list --vault {vault.Id}";
        return Op<ImmutableList<DocumentDetails>>(command);
    }

    /// <inheritdoc />
    public ImmutableList<DocumentDetails> SearchForDocuments(IVault? vault = null, bool? includeArchive = null)
    {
        if (vault is not null && vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        var command = "document list";
        if (vault is not null)
            command += $" --vault {vault.Id}";
        if (includeArchive is not null && includeArchive.Value)
            command += " --include-archive";
        return Op<ImmutableList<DocumentDetails>>(command);
    }

    /// <inheritdoc />
    public void GetDocument(IDocument document, IVault vault, string filePath, string? fileMode = null)
    {
        if (document.Id is null || document.Id.Length == 0)
            throw new ArgumentException($"{nameof(document.Id)} cannot be empty.", nameof(document));
        if (vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));
        var trimmedFilePath = filePath.Trim();
        if (trimmedFilePath.Length == 0)
            throw new ArgumentException($"{nameof(trimmedFilePath)} cannot be empty.", nameof(filePath));

        var trimmedFileMode = fileMode?.Trim();

        // Not specifying --force will hang waiting for user input if the file exists.
        var command = $"document get {document.Id} --out-file \"{trimmedFilePath}\" --force --vault {vault.Id}";
        if (trimmedFileMode is not null)
            command += $" --file-mode {trimmedFileMode}";
        Op(command);
    }

    /// <inheritdoc />
    public void SearchForDocument(IDocument document, string filePath, IVault? vault = null, bool? includeArchive = null, string? fileMode = null)
    {
        if (document.Id is null || document.Id.Length == 0)
            throw new ArgumentException($"{nameof(document.Id)} cannot be empty.", nameof(document));
        var trimmedFilePath = filePath.Trim();
        if (trimmedFilePath.Length == 0)
            throw new ArgumentException($"{nameof(trimmedFilePath)} cannot be empty.", nameof(filePath));
        if (!File.Exists(trimmedFilePath))
            throw new ArgumentException($"File '{trimmedFilePath}' was not found or could not be accessed.", nameof(filePath));
        if (vault is not null && vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        var trimmedFileMode = fileMode?.Trim();

        // Not specifying --force will hang waiting for user input if the file exists.
        var command = $"document get {document.Id} --out-file \"{trimmedFilePath}\" --force";
        if (vault is not null)
            command += $" --vault {vault.Id}";
        if (includeArchive is not null && includeArchive.Value)
            command += " --include-archive";
        if (trimmedFileMode is not null)
            command += $" --file-mode {trimmedFileMode}";
        Op(command);
    }

    /// <inheritdoc />
    public Document CreateDocument(IVault vault, string filePath, string? fileName = null, string? title = null, IReadOnlyCollection<string>? tags = null)
    {
        if (vault is not null && vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));
        var trimmedFilePath = filePath.Trim();
        if (trimmedFilePath.Length == 0)
            throw new ArgumentException($"{nameof(trimmedFilePath)} cannot be empty.", nameof(filePath));
        if (!File.Exists(trimmedFilePath))
            throw new ArgumentException($"File '{trimmedFilePath}' was not found or could not be accessed.", nameof(filePath));

        var trimmedFileName = fileName?.Trim();
        var trimmedTitle = title?.Trim();

        var command = $"document create \"{trimmedFilePath}\"";
        if (vault is not null)
            command += $" --vault {vault.Id}";
        if (trimmedFileName is not null)
            command += $" --file-name \"{trimmedFileName}\"";
        if (trimmedTitle is not null)
            command += $" --title \"{trimmedTitle}\"";
        if (tags is not null && tags.Count > 0)
            command += $" --tags \"{tags.ToCommaSeparated()}\"";
        return Op<Document>(command);
    }

    /// <inheritdoc />
    public void ReplaceDocument(IDocument document, IVault vault, string filePath, string? fileName = null, string? title = null, IReadOnlyCollection<string>? tags = null)
    {
        if (document.Id is null || document.Id.Length == 0)
            throw new ArgumentException($"{nameof(document.Id)} cannot be empty.", nameof(document));
        if (vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));
        var trimmedFilePath = filePath.Trim();
        if (trimmedFilePath.Length == 0)
            throw new ArgumentException($"{nameof(trimmedFilePath)} cannot be empty.", nameof(filePath));
        if (!File.Exists(trimmedFilePath))
            throw new ArgumentException($"File '{trimmedFilePath}' was not found or could not be accessed.", nameof(filePath));

        var trimmedFileName = fileName?.Trim();
        var trimmedTitle = title?.Trim();

        var command = $"document edit {document.Id} \"{trimmedFilePath}\" --vault {vault.Id}";
        if (trimmedFileName is not null)
            command += $" --file-name \"{trimmedFileName}\"";
        if (trimmedTitle is not null)
            command += $" --title \"{trimmedTitle}\"";
        if (tags is not null && tags.Count > 0)
            command += $" --tags \"{tags.ToCommaSeparated()}\"";
        Op(command);
    }

    /// <inheritdoc />
    public void ArchiveDocument(IDocument document, IVault vault)
    {
        if (document.Id is null || document.Id.Length == 0)
            throw new ArgumentException($"{nameof(document.Id)} cannot be empty.", nameof(document));
        if (vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        var command = $"document delete {document.Id} --vault {vault.Id} --archive";
        Op(command);
    }

    /// <inheritdoc />
    public void DeleteDocument(IDocument document, IVault vault)
    {
        if (document.Id is null || document.Id.Length == 0)
            throw new ArgumentException($"{nameof(document.Id)} cannot be empty.", nameof(document));
        if (vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        var command = $"document delete {document.Id} --vault {vault.Id}";
        Op(command);
    }
}