using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;

namespace DbAutocadDemoNemetschek
{
    public static class CommonUtil
    {
         public static int GetColorIndex(string colorName)
        {
            int color = 7;
            switch (colorName.ToUpper())
            {
                case "RED":
                    color = 1;
                    break;
                case "YELLOW":
                    color = 2;
                    break;
                case "GREEN":
                    color = 3;
                    break;
                case "CYAN":
                    color = 4;
                    break;
                case "BLUE":
                    color = 5;
                    break;
                case "MAGENTA":
                    color = 6;
                    break;
                case "WHITE":
                    color = 7;
                    break;
                case "BYBLOCK":
                    color = 0;
                    break;
                case "BYLAYER":
                    color = 256;
                    break;
                default:
                    color = 256;
                    break;                        
            }
            return color;
        }
        
        public static void AddXDataToEntity(string appName, Entity ent, int xdValue)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Transaction tr = db.TransactionManager.StartTransaction();
            using (tr)
            {
                // Get the registered application names table
                RegAppTable regTable = (RegAppTable)tr.GetObject(db.RegAppTableId, OpenMode.ForRead);

                if (!regTable.Has(appName))
                {
                    regTable.UpgradeOpen();

                    // Add the application name for Xdata
                    RegAppTableRecord app = new RegAppTableRecord();
                    app.Name = appName;
                    regTable.Add(app);
                    tr.AddNewlyCreatedDBObject(app, true);
                }

                // Append the Xdata to entity
                ResultBuffer rb = new ResultBuffer(new TypedValue(1001, appName), new TypedValue((int)DxfCode.ExtendedDataInteger32, xdValue));
                ent.XData = rb;
                rb.Dispose();
                tr.Commit();
            }
        }
    }
}
