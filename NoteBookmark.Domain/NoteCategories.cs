using System;

namespace NoteBookmark.Domain;

/// <summary>
/// Notes Categories
/// Legacy code from the original ReadingNotres project.
/// </summary>
public static class NoteCategories
{
    /// <summary>
    /// Get a dictionary to change the short version by the long version of category name.
    /// </summary>
    public static string GetCategory(string? category)
    {
        var categories = new Dictionary<string, string>
                                                    {
                                                        {"cloud", "Cloud"},
                                                        {"data", "Data"},
                                                        {"database", "Databases"},
                                                        {"dev", "Programming"},
                                                        {"devops", "DevOps"},
                                                        {"misc", "Miscellaneous"},
                                                        {"top", "Suggestion of the week"},
                                                        {"oss", "Open Source"},
                                                        {"del", "del"}
                                                    };
        if (!String.IsNullOrEmpty(category) && categories.ContainsKey(category))
        {
            return categories[category];
        }
        else
        {
            return categories["misc"];
        }
    }
}
