private void InitializeYearAndWeekSelection()
{
    ComboBoxYear.Items.Clear();
    ComboBoxWeeks.Items.Clear();
    ComboBoxWeeks.Enabled = false;
    ButtonReporting.Enabled = false;

    string baseArchivePath =
        SharedGlobals.MarntCLoudArchiveLocation + "\\" + SharedGlobals.ActiveCompany + "\\peppol\\archive";

    if (!Directory.Exists(baseArchivePath))
    {
        return;
    }

    // Collect all year folders that actually contain WeekXX subfolders
    var years = new List<int>();
    foreach (string yearFolder in Directory.GetDirectories(baseArchivePath))
    {
        string folderName = Path.GetFileName(yearFolder);

        int year;
        if (!int.TryParse(folderName, out year))
        {
            continue;
        }

        // Only keep years that have at least one WeekNN folder
        if (Directory.GetDirectories(yearFolder, "Week*").Length > 0)
        {
            years.Add(year);
        }
    }

    if (years.Count == 0)
    {
        return;
    }

    years.Sort(); // ascending
    // Option: take latest year
    int latestYear = years[years.Count - 1];

    foreach (int y in years)
    {
        ComboBoxYear.Items.Add(y.ToString());
    }

    // Selecting here will trigger ComboBoxYear_SelectedIndexChanged,
    // which will correctly fill ComboBoxWeeks for that year.
    ComboBoxYear.SelectedItem = latestYear.ToString();
}

private void ComboBoxYear_SelectedIndexChanged(object sender, EventArgs e)
{
    ComboBoxWeeks.Items.Clear();
    ComboBoxWeeks.Enabled = false;
    ButtonReporting.Enabled = false;

    if (ComboBoxYear.SelectedItem == null)
    {
        return;
    }

    string selectedYear = ComboBoxYear.SelectedItem.ToString();
    string basePath =
        SharedGlobals.MarntCLoudArchiveLocation + "\\" + SharedGlobals.ActiveCompany +
        "\\peppol\\archive\\" + selectedYear;

    if (!Directory.Exists(basePath))
    {
        return;
    }

    var weekItems = new List<string>();

    foreach (string weekFolder in Directory.GetDirectories(basePath, "Week*"))
    {
        // weekFolderName e.g. "Week01"
        string weekFolderName = Path.GetFileName(weekFolder);
        int xmlCount = Directory.GetFiles(weekFolder, "*.xml").Length;
        string itemText = weekFolderName + " (" + xmlCount.ToString() + ")";
        weekItems.Add(itemText);
    }

    // sort by week number descending
    weekItems.Sort((a, b) =>
    {
        int weekA = ExtractWeekNumber(a);
        int weekB = ExtractWeekNumber(b);
        return weekB.CompareTo(weekA);
    });

    ComboBoxWeeks.Items.AddRange(weekItems.ToArray());

    if (ComboBoxWeeks.Items.Count > 0)
    {
        ComboBoxWeeks.SelectedIndex = 0;
        ComboBoxWeeks.Enabled = true;
        ButtonReporting.Enabled = true;
    }
}

private static int ExtractWeekNumber(string itemText)
{
    if (string.IsNullOrEmpty(itemText) || itemText.Length < 6)
    {
        return 0;
    }

    // expected: "WeekNN ..."
    string numPart = itemText.Substring(4, 2);
    int week;
    return int.TryParse(numPart, out week) ? week : 0;
}