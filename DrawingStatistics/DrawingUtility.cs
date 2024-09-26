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
                    BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                    // Open the Block Table record Modelspace for write
                    BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                    Circle circleOne = new Circle() { Center = new Point3d(0, 0, 0), Radius = radius1, ColorIndex = red };
                    btr.AppendEntity(circleOne);
                    trans.AddNewlyCreatedDBObject(circleOne, true);

                    Circle circleTwo = new Circle() { Center = new Point3d(0, 0, 0), Radius = radius2 };
                    btr.AppendEntity(circleTwo);
                    trans.AddNewlyCreatedDBObject(circleTwo, true);

                    Circle circleThree = new Circle() { Center = new Point3d(30, 30, 0), Radius = radius3, ColorIndex = blue };
                    btr.AppendEntity(circleThree);
                    trans.AddNewlyCreatedDBObject(circleThree, true);

                    // Create a collection and the 3 Circle objects
                    DBObjectCollection collection = new DBObjectCollection();
                    collection.Add(circleOne);
                    collection.Add(circleTwo);
                    collection.Add(circleThree);

                    foreach (Circle circle in collection)
                    {
                        if (circle.Radius == radius2)
                        {
                            Circle circleFour = circle.Clone() as Circle;
                            circleFour.ColorIndex = green; // Green
                            circleFour.Radius = radius4;
                            btr.AppendEntity(circleFour);
                            trans.AddNewlyCreatedDBObject(circleFour, true);
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
                    BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                    BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                
                    Point3d insPt = new Point3d(10, 10, 0);
                    MText mtx = new MText() { Location = insPt, Height = height, Contents = Mtext1 };
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
