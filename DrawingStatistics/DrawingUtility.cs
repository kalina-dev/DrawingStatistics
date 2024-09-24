using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Geometry;

namespace DrawingStatistics
{
    internal class DrawingUtility
    {
        const string Mtext1 = "Rotating MText";
        const string Mtext2 = "Rotated MText";
        const short radius1 = 1;
        const short radius2 = 2;
        const short radius3 = 3;
        const short radius4 = 6;
        const short height = 5;
        const short red = 1;
        const short green = 3;
        const short blue = 5;
        const string error = "Error occured: ";

        [CommandMethod("CreateAndCopyCircle")]
        public static void CreateAndCopyCircle()
        {
            // Get the current document and database
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;

            // Start a transaction
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // Open the BlockTable for read
                    BlockTable bt;
                    bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;

                    // Open the Block Table record Modelspace for write
                    BlockTableRecord btr;
                    btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                    using (Circle circle1 = new Circle())
                    {
                        circle1.Center = new Point3d(0, 0, 0);
                        circle1.Radius = radius1;
                        circle1.ColorIndex = red;

                        // Add the new object to the BlockTable record
                        btr.AppendEntity(circle1);
                        trans.AddNewlyCreatedDBObject(circle1, true);

                        Circle circle2 = new Circle();
                        circle2.Center = new Point3d(10, 10, 0);
                        circle2.Radius = radius2;

                        // Add the new object to the BlockTable record
                        btr.AppendEntity(circle2);
                        trans.AddNewlyCreatedDBObject(circle2, true);

                        Circle circle3 = new Circle();
                        circle3.Center = new Point3d(30, 30, 0);
                        circle3.Radius = radius3;
                        circle3.ColorIndex = blue;

                        // Add the new object to the BlockTable record
                        btr.AppendEntity(circle3);
                        trans.AddNewlyCreatedDBObject(circle3, true);

                        // Create a collection and the 3 Circle objects
                        DBObjectCollection col = new DBObjectCollection();
                        col.Add(circle1);
                        col.Add(circle2);
                        col.Add(circle3);

                        foreach (Circle cir in col)
                        {
                            if (cir.Radius == radius2)
                            {
                                Circle c4 = cir.Clone() as Circle;
                                c4.ColorIndex = green; // Green
                                c4.Radius = radius4;

                                // Add the new object to the BlockTable record
                                btr.AppendEntity(c4);
                                trans.AddNewlyCreatedDBObject(c4, true);
                            }
                        }
                    }
                    trans.Commit();
                }
                catch (System.Exception ex)
                {
                    doc.Editor.WriteMessage(error + ex.Message);
                    trans.Abort();
                }
            }
        }


        [CommandMethod("CreateAndRotateMText")]
        public static void CreateAndRotateMText()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    BlockTable bt;
                    bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;

                    BlockTableRecord btr;
                    btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                    using (MText mtx = new MText())
                    {
                        Point3d insPt = new Point3d(10, 10, 0);
                        mtx.Location = insPt;
                        mtx.Height = height;
                        mtx.Contents = Mtext1;

                        btr.AppendEntity(mtx);
                        trans.AddNewlyCreatedDBObject(mtx, true);

                        MText mtx2 = mtx.Clone() as MText;
                        mtx2.ColorIndex = red; //red
                        mtx2.Contents = Mtext2;

                        btr.AppendEntity(mtx2);
                        trans.AddNewlyCreatedDBObject(mtx2, true);

                        Matrix3d curUCSMatrix = doc.Editor.CurrentUserCoordinateSystem;
                        CoordinateSystem3d curUCS = curUCSMatrix.CoordinateSystem3d;

                        mtx2.TransformBy(Matrix3d.Rotation(0.5235, curUCS.Zaxis, new Point3d(0, 0, 0)));
                    }

                    trans.Commit();
                    doc.SendStringToExecute("._zoom e ", false, false, false);
                }
                catch (System.Exception ex)
                {
                    doc.Editor.WriteMessage(error + ex.Message);
                    trans.Abort();
                }
            }
        }
    }
}
