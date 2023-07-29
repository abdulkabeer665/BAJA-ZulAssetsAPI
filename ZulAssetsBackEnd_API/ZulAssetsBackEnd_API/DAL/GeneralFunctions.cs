namespace ZulAssetsBackEnd_API.DAL
{
    public class GeneralFunctions
    {

        #region Declaration

        public static string Message = "ControllerName were Operation by UserName";

        #endregion

        #region Write or Append Text in Text File

        public static string CreateAndWriteToFile(string ControllerName, string Operation, string UserName)
        {
            try
            {
                string text = Message;
                Dictionary<string, string> replacements = new Dictionary<string, string>()
                {
                    { "ControllerName", ControllerName },
                    { "Operation", Operation },
                    { "UserName", UserName }
                };

                using (StreamWriter writer = File.AppendText(CreateTransactionsFolder()))
                {
                    writer.WriteLine(ReplaceMultipleWords(text, replacements));
                    writer.WriteLine("-----------------**************************------------------");
                }
                return "Text appended to the file successfully.";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        public static string ReplaceMultipleWords(string text, Dictionary<string, string> replacements)
        {
            foreach (var kvp in replacements)
            {
                text = text.Replace(kvp.Key, kvp.Value);
            }

            return text;
        }

        public string ReplaceMultipleWordsDB(string text, Dictionary<string, string> replacements)
        {
            foreach (var kvp in replacements)
            {
                text = text.Replace(kvp.Key, kvp.Value);
            }

            return text;
        }

        #endregion

        #region Transaction Folder and Text File Creation

        public static string CreateTransactionsFolder()
        {
            var folderDir = GenerateFilePath();
            // If directory does not exist, create it
            if (!Directory.Exists(folderDir + "\\TransactionLogs"))
            {
                Directory.CreateDirectory(folderDir + "\\TransactionLogs");
            }
            return folderDir + "\\TransactionLogs\\TransactionLogs.txt";
        }

        #endregion

        #region Get File Path

        public static string GenerateFilePath()
        {
            string folderDir = Directory.GetCurrentDirectory();
            return folderDir;
        }

        #endregion

    }
}
