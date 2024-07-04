using System.Reflection;

namespace DAPManSWebReports.Entities.Constants
{
    public static class ReportMessageCodes
    {
        public static int ValidationOK                      = 0;
        public static ReturnMessage DataSourceEmpty         = new ReturnMessage(1,   "Response.Error.DataSourceEmpty"      );
        public static ReturnMessage DataSourceExists        = new ReturnMessage(2,   "Response.Error.DataSourceExists"     );
        public static ReturnMessage DataviewEmpty           = new ReturnMessage(3,   "Response.Error.DataviewEmpty"        );
        public static ReturnMessage DataviewExists          = new ReturnMessage(4,   "Response.Error.DataviewExists"       );
        public static ReturnMessage DocumentLinkEmpty       = new ReturnMessage(5,   "Response.Error.DocumentLinkEmpty"    );
        public static ReturnMessage DocumentLinkExists      = new ReturnMessage(6,   "Response.Error.DocumentLinkExists"   );
        public static ReturnMessage FolderEmpty             = new ReturnMessage(7,   "Response.Error.FolderEmpty"          );
        public static ReturnMessage FolderExists            = new ReturnMessage(8,   "Response.Error.FolderExists"         );
        public static ReturnMessage ReportLinkEmpty         = new ReturnMessage(9,   "Response.Error.ReportLinkEmpty"      );
        public static ReturnMessage ReportLinkExists        = new ReturnMessage(10,  "Response.Error.ReportLinkExists"     );
        public static ReturnMessage CheckQuery              = new ReturnMessage(11,  "Response.Error.CheckQuery"           );
        public static ReturnMessage notAvailable            = new ReturnMessage(12,  "Response.Error.NotAvailable"         );
        public static ReturnMessage ReportDifferentParams   = new ReturnMessage(13,  "Response.Error.ReportDifferentParams");
        public static ReturnMessage ReportFileAlreadyExists = new ReturnMessage(14,  "Response.Error.FileAlreadyExists"    );
        public static ReturnMessage SessionEnded            = new ReturnMessage(15,  "Response.Error.SessionEnded"         );
        public static ReturnMessage Unauthorized            = new ReturnMessage(16,  "Response.Error.Unauthorized"         );
        public static ReturnMessage IncorrectVersion        = new ReturnMessage(17,  "Response.Error.IncorrectVersion"     );
        public static ReturnMessage DataSourceDeleted       = new ReturnMessage(-1,  "Response.Info.DataSourceDeleted"     );
        public static ReturnMessage DataSourceSaved         = new ReturnMessage(-2,  "Response.Info.DataSourceSaved"       );
        public static ReturnMessage DataviewDeleted         = new ReturnMessage(-3,  "Response.Info.DataviewDeleted"       );
        public static ReturnMessage DataviewSaved           = new ReturnMessage(-4,  "Response.Info.DataviewSaved"         );
        public static ReturnMessage DocumentLinkDeleted     = new ReturnMessage(-5,  "Response.Info.DocumentLinkDeleted"   );
        public static ReturnMessage DocumentLinkSaved       = new ReturnMessage(-6,  "Response.Info.DocumentLinkSaved"     );
        public static ReturnMessage FolderDeleted           = new ReturnMessage(-7,  "Response.Info.FolderDeleted"         );
        public static ReturnMessage FolderSaved             = new ReturnMessage(-8,  "Response.Info.FolderSaved"           );
        public static ReturnMessage ReportLinkDeleted       = new ReturnMessage(-9,  "Response.Info.ReportLinkDeleted"     );
        public static ReturnMessage ReportLinkSaved         = new ReturnMessage(-10, "Response.Info.ReportLinkSaved"       );
        public static ReturnMessage QueryOk                 = new ReturnMessage(-11, "Response.Info.QueryOk"               );
        public static ReturnMessage SettingsSaved           = new ReturnMessage(-12, "Response.Info.SettingsSaved"         );
        public static ReturnMessage SettingsDeleted         = new ReturnMessage(-12, "Response.Info.SettingsDeleted"       );
        public static ReturnMessage PermissionsSaved        = new ReturnMessage(-13, "Response.Info.PermissionsSaved"      );
        public static ReturnMessage ConnectionValidated     = new ReturnMessage(-14, "Response.Info.ConnectionValidated"   );
        public static ReturnMessage ObjectMoved             = new ReturnMessage(-15, "Response.Info.ObjectMoved"           );
        public static string GetCodeFromID(int id)
        {
            foreach (FieldInfo field in typeof(ReportMessageCodes).GetFields())
            {
                if (field.Name != "ValidationOK")
                {
                    ReturnMessage returnMessage = (ReturnMessage)field.GetValue((object)typeof(ReportMessageCodes));
                    if (returnMessage.ID == id)
                        return returnMessage.Code;
                }
            }
            return "";
        }
    }
}
