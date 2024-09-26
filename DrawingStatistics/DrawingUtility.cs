using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Geometry;

namespace DrawingStatistics
{
    internal class DrawingUtility
    {
        const string MtextOne = @"Rotating MText";
        const string MtextTwo = @"Rotated MText";
        const string error = @"Error occured: ";
        const short radiusCircleOne = 1;
        const short radiusCircleTwo = 2;
        const short radiusCircleThree = 3;
        const short radiusCircleFour = 6;
        const short height = 5;
        const short red = 1;
        const short green = 3;
        const short blue = 5;
        const double angleDegree = 0.5235;

        [CommandMethod("CreateAndCopyCircle")]
        public static void CreateAndCopyCircle()
        {
            Document activeDocument = Application.DocumentManager.MdiActiveDocument;
            Database database = activeDocument.Database;

            using (Transaction transaction = database.TransactionManager.StartTransaction())
            {
                try
                {
                    BlockTable blockTable = transaction.GetObject(database.BlockTableId, OpenMode.ForRead) as BlockTable;
                    BlockTableRecord record = transaction.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                    Circle circleOne = new Circle() { Center = new Point3d(0, 0, 0), Radius = radiusCircleOne, ColorIndex = red };
                    record.AppendEntity(circleOne);
                    transaction.AddNewlyCreatedDBObject(circleOne, true);

                    Circle circleTwo = new Circle() { Center = new Point3d(0, 0, 0), Radius = radiusCircleTwo };
                    record.AppendEntity(circleTwo);
                    transaction.AddNewlyCreatedDBObject(circleTwo, true);

                    Circle circleThree = new Circle() { Center = new Point3d(30, 30, 0), Radius = radiusCircleThree, ColorIndex = blue };
                    record.AppendEntity(circleThree);
                    transaction.AddNewlyCreatedDBObject(circleThree, true);

                    DBObjectCollection collection = new DBObjectCollection
                    {
                        circleOne,
                        circleTwo,
                        circleThree
                    };

                    foreach (Circle circle in collection)
                    {
                        if (circle.Radius == radiusCircleTwo)
                        {
                            Circle circleFour = circle.Clone() as Circle;
                            circleFour.ColorIndex = green; 
                            circleFour.Radius = radiusCircleFour;
                            record.AppendEntity(circleFour);
                            transaction.AddNewlyCreatedDBObject(circleFour, true);
                        }
                    }

                    transaction.Commit();
                }
                catch (System.Exception ex)
                {
                    activeDocument.Editor.WriteMessage(error + ex.Message);
                    transaction.Abort();
                }
            }
        }


        [CommandMethod("CreateAndRotateMText")]
        public static void CreateAndRotateMText()
        {
            Document activeDocument = Application.DocumentManager.MdiActiveDocument;
            Database database = activeDocument.Database;

            using (Transaction transaction = database.TransactionManager.StartTransaction())
            {
                try
                {
                    BlockTable blockTable = transaction.GetObject(database.BlockTableId, OpenMode.ForRead) as BlockTable;
                    BlockTableRecord record = transaction.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                
                    Point3d insertionPoint = new Point3d(10, 10, 0);
                    MText firstText = new MText() { Location = insertionPoint, Height = height, Contents = MtextOne };
                    record.AppendEntity(firstText);
                    transaction.AddNewlyCreatedDBObject(firstText, true);

                    MText secondText = firstText.Clone() as MText;
                    secondText.ColorIndex = red;
                    secondText.Contents = MtextTwo;

                    record.AppendEntity(secondText);
                    transaction.AddNewlyCreatedDBObject(secondText, true);

                    Matrix3d curUCSMatrix = activeDocument.Editor.CurrentUserCoordinateSystem;
                    CoordinateSystem3d curUCS = curUCSMatrix.CoordinateSystem3d;
                    secondText.TransformBy(Matrix3d.Rotation(angleDegree, curUCS.Zaxis, new Point3d(0, 0, 0)));

                    transaction.Commit();
                    activeDocument.SendStringToExecute("._zoom e ", false, false, false);
                }
                catch (System.Exception ex)
                {
                    activeDocument.Editor.WriteMessage(error + ex.Message);
                    transaction.Abort();
                }
            }
        }
    }
}
