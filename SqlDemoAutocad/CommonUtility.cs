using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;

namespace AutocadSQLPlugin
{
    public class CommonUtility
    {
        public static int GetColorIndex(string colorName)
        {
            switch (colorName.ToUpper())
            {
                case "RED":
                    break;
                case "YELLOW":
                    break;
                case "GREEN":
                    break;
                case "CYAN":
                    break;
                case "BLUE":
                    break;
                case "MAGENTA":
                    break;
                case "WHITE":
                    break;
                case "BYBLOCK":
                    break;
                case "BYLAYER":
                    break;
                default:
                    break;
            }
            return 7;
        }

        public static void AddXDataToEntity(string appName, Entity ent, int xdValue)
        {
            Document activeDocument = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument;
            Database database = activeDocument.Database;
            Transaction tr = database.TransactionManager.StartTransaction();
            using (tr)
            {
                RegAppTable regTable = (RegAppTable)tr.GetObject(database.RegAppTableId, OpenMode.ForRead);

                if (!regTable.Has(appName))
                {
                    regTable.UpgradeOpen();
                    RegAppTableRecord app = new()
                    {
                        Name = appName
                    };
                    regTable.Add(app);
                    tr.AddNewlyCreatedDBObject(app, true);
                }

                ResultBuffer rb = new(new TypedValue(1001, appName), new TypedValue((int)DxfCode.ExtendedDataInteger32, xdValue));
                ent.XData = rb;
                rb.Dispose();
                tr.Commit();
            }
        }
    }
}
