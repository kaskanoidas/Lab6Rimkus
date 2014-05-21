using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
namespace Lab6Rimkus
{
    public class Class1
    {
        [CommandMethod("Figura1")]
        public void Figura1()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.GetDocument(db);
            Editor ed = doc.Editor;

            PromptDoubleOptions GetIlgis = new PromptDoubleOptions("\nĮveskite mikroskemos ilgį");
            double Ilgis;
            GetIlgis.AllowNegative = false;
            GetIlgis.AllowZero = false;
            GetIlgis.AllowNone = false;
            PromptDoubleResult GetIlgisRezultatas = ed.GetDouble(GetIlgis);
            Ilgis = GetIlgisRezultatas.Value;

            PromptDoubleOptions GetPlotis = new PromptDoubleOptions("\nĮveskite mikroskemos plotį");
            double Plotis;
            GetPlotis.AllowNegative = false;
            GetPlotis.AllowZero = false;
            GetPlotis.AllowNone = false;
            PromptDoubleResult GetPlotisRezultatas = ed.GetDouble(GetPlotis);
            Plotis = GetPlotisRezultatas.Value;

            PromptPointOptions GetTaskas = new PromptPointOptions("\nPasirinkite tašką");
            Point3d Taskas;
            GetTaskas.AllowNone = false;
            PromptPointResult GetTaskasRezultatas = ed.GetPoint(GetTaskas);
            Taskas = GetTaskasRezultatas.Value;

            Transaction tr = db.TransactionManager.StartTransaction();
            using (tr)
            {
                BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead, false);
                BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite,false);
                Polyline3d polyline = new Polyline3d();
                btr.AppendEntity(polyline);
                tr.AddNewlyCreatedDBObject(polyline, true);
                PolylineVertex3d vex3d = new PolylineVertex3d();

                //Pradinis taskas
                vex3d = new PolylineVertex3d(Taskas);
                polyline.AppendVertex(vex3d);
                tr.AddNewlyCreatedDBObject(vex3d, true);

                //Atsakos tarp pradinio ir pirmo kampo
                double betaIlgis = Ilgis / 8;
                double AtsakosIlgis = Math.Min(Ilgis / 4, Plotis / 4);
                for (int i = 1; i < 8; i++)
                {
                    // Linija iki atsakos
                    vex3d = new PolylineVertex3d(new Point3d(Taskas.X, Taskas.Y + betaIlgis * i, 0));
                    polyline.AppendVertex(vex3d);
                    tr.AddNewlyCreatedDBObject(vex3d, true);

                    //Atsakos linija
                    vex3d = new PolylineVertex3d(new Point3d(Taskas.X - AtsakosIlgis, Taskas.Y + betaIlgis * i, 0));
                    polyline.AppendVertex(vex3d);
                    tr.AddNewlyCreatedDBObject(vex3d, true);

                    //Atsakos grizimas atgal i kontura
                    vex3d = new PolylineVertex3d(new Point3d(Taskas.X, Taskas.Y + betaIlgis * i, 0));
                    polyline.AppendVertex(vex3d);
                    tr.AddNewlyCreatedDBObject(vex3d, true);
                }

                // Pirmas kampas
                vex3d = new PolylineVertex3d(new Point3d(Taskas.X, Taskas.Y + Ilgis, 0));
                polyline.AppendVertex(vex3d);
                tr.AddNewlyCreatedDBObject(vex3d, true);

                //Atsakos tarp pirmo ir angtro kampo
                double betaPlotis = Plotis / 2;
                for (int i = 1; i < 2; i++)
                {
                    // Linija iki atsakos
                    vex3d = new PolylineVertex3d(new Point3d(Taskas.X + betaPlotis * i, Taskas.Y + Ilgis, 0));
                    polyline.AppendVertex(vex3d);
                    tr.AddNewlyCreatedDBObject(vex3d, true);

                    //Atsakos linija
                    vex3d = new PolylineVertex3d(new Point3d(Taskas.X + betaPlotis * i, Taskas.Y + Ilgis + AtsakosIlgis, 0));
                    polyline.AppendVertex(vex3d);
                    tr.AddNewlyCreatedDBObject(vex3d, true);

                    //Atsakos grizimas atgal i kontura
                    vex3d = new PolylineVertex3d(new Point3d(Taskas.X + betaPlotis * i, Taskas.Y + Ilgis, 0));
                    polyline.AppendVertex(vex3d);
                    tr.AddNewlyCreatedDBObject(vex3d, true);
                }

                //Antras kampas
                vex3d = new PolylineVertex3d(new Point3d(Taskas.X + Plotis, Taskas.Y + Ilgis, 0));
                polyline.AppendVertex(vex3d);
                tr.AddNewlyCreatedDBObject(vex3d, true);

                //Atsakos tarp antro ir trecio kampo
                for (int i = 1; i < 8; i++)
                {
                    // Linija iki atsakos
                    vex3d = new PolylineVertex3d(new Point3d(Taskas.X + Plotis, Taskas.Y + Ilgis - betaIlgis * i, 0));
                    polyline.AppendVertex(vex3d);
                    tr.AddNewlyCreatedDBObject(vex3d, true);

                    //Atsakos linija
                    vex3d = new PolylineVertex3d(new Point3d(Taskas.X + Plotis + AtsakosIlgis, Taskas.Y + Ilgis - betaIlgis * i, 0));
                    polyline.AppendVertex(vex3d);
                    tr.AddNewlyCreatedDBObject(vex3d, true);

                    //Atsakos grizimas atgal i kontura
                    vex3d = new PolylineVertex3d(new Point3d(Taskas.X + Plotis, Taskas.Y + Ilgis - betaIlgis * i, 0));
                    polyline.AppendVertex(vex3d);
                    tr.AddNewlyCreatedDBObject(vex3d, true);
                }

                //Trecias kampas
                vex3d = new PolylineVertex3d(new Point3d(Taskas.X + Plotis, Taskas.Y, 0));
                polyline.AppendVertex(vex3d);
                tr.AddNewlyCreatedDBObject(vex3d, true);

                //Atsakos tarp trecio ir pradinio kampo
                for (int i = 1; i < 2; i++)
                {
                    // Linija iki atsakos
                    vex3d = new PolylineVertex3d(new Point3d(Taskas.X + Plotis - betaPlotis * i, Taskas.Y, 0));
                    polyline.AppendVertex(vex3d);
                    tr.AddNewlyCreatedDBObject(vex3d, true);

                    //Atsakos linija
                    vex3d = new PolylineVertex3d(new Point3d(Taskas.X + Plotis - betaPlotis * i, Taskas.Y - AtsakosIlgis, 0));
                    polyline.AppendVertex(vex3d);
                    tr.AddNewlyCreatedDBObject(vex3d, true);

                    //Atsakos grizimas atgal i kontura
                    vex3d = new PolylineVertex3d(new Point3d(Taskas.X + Plotis - betaPlotis * i, Taskas.Y, 0));
                    polyline.AppendVertex(vex3d);
                    tr.AddNewlyCreatedDBObject(vex3d, true);
                }

                //Ketvirtas kampas / Pradinis taskas
                vex3d = new PolylineVertex3d(Taskas);
                polyline.AppendVertex(vex3d);
                tr.AddNewlyCreatedDBObject(vex3d, true);
                tr.Commit();
            }
        }

        [CommandMethod("Figura2")]
        public void Figura2()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.GetDocument(db);
            Editor ed = doc.Editor;

            PromptDoubleOptions GetSpindulys = new PromptDoubleOptions("\nĮveskite detalės spindulį");
            double Spindulys;
            GetSpindulys.AllowNegative = false;
            GetSpindulys.AllowZero = false;
            GetSpindulys.AllowNone = false;
            PromptDoubleResult GetSpindulysRezultatas = ed.GetDouble(GetSpindulys);
            Spindulys = GetSpindulysRezultatas.Value;

            PromptPointOptions GetTaskas = new PromptPointOptions("\nPasirinkite centro tašką");
            Point3d Taskas;
            GetTaskas.AllowNone = false;
            PromptPointResult GetTaskasRezultatas = ed.GetPoint(GetTaskas);
            Taskas = GetTaskasRezultatas.Value;

            Transaction tr = db.TransactionManager.StartTransaction();
            using (tr)
            {
                BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead, false);
                BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite, false);

                //Apskritimas
                Circle Apskritimas = new Circle(Taskas, new Vector3d(0, 0, 1), Spindulys);
                btr.AppendEntity(Apskritimas);
                tr.AddNewlyCreatedDBObject(Apskritimas, true);

                //Linijos
                Polyline3d polyline = new Polyline3d();
                btr.AppendEntity(polyline);
                tr.AddNewlyCreatedDBObject(polyline, true);
                PolylineVertex3d vex3d = new PolylineVertex3d();

                // Linija NR1 (centrine)
                vex3d = new PolylineVertex3d(new Point3d(Taskas.X - Spindulys / 4, Taskas.Y + Spindulys / 6, 0));
                polyline.AppendVertex(vex3d);
                tr.AddNewlyCreatedDBObject(vex3d, true);

                vex3d = new PolylineVertex3d(new Point3d(Taskas.X - Spindulys / 4, Taskas.Y - Spindulys / 6, 0));
                polyline.AppendVertex(vex3d);
                tr.AddNewlyCreatedDBObject(vex3d, true);

                vex3d = new PolylineVertex3d(new Point3d(Taskas.X - Spindulys / 4, Taskas.Y, 0));
                polyline.AppendVertex(vex3d);
                tr.AddNewlyCreatedDBObject(vex3d, true);

                vex3d = new PolylineVertex3d(new Point3d(Taskas.X + Spindulys / 2, Taskas.Y, 0));
                polyline.AppendVertex(vex3d);
                tr.AddNewlyCreatedDBObject(vex3d, true);

                vex3d = new PolylineVertex3d(new Point3d(Taskas.X + Spindulys / 2, Taskas.Y - Spindulys / 2, 0));
                polyline.AppendVertex(vex3d);
                tr.AddNewlyCreatedDBObject(vex3d, true);

                vex3d = new PolylineVertex3d(new Point3d(Taskas.X - Spindulys / 4, Taskas.Y - Spindulys / 2, 0));
                polyline.AppendVertex(vex3d);
                tr.AddNewlyCreatedDBObject(vex3d, true);

                vex3d = new PolylineVertex3d(new Point3d(Taskas.X - Spindulys / 4, Taskas.Y - Spindulys / 2 + Spindulys / 6, 0));
                polyline.AppendVertex(vex3d);
                tr.AddNewlyCreatedDBObject(vex3d, true);

                vex3d = new PolylineVertex3d(new Point3d(Taskas.X - Spindulys / 4, Taskas.Y - Spindulys / 2 - Spindulys / 6, 0));
                polyline.AppendVertex(vex3d);
                tr.AddNewlyCreatedDBObject(vex3d, true);

                vex3d = new PolylineVertex3d(new Point3d(Taskas.X - Spindulys / 4, Taskas.Y - Spindulys / 2, 0));
                polyline.AppendVertex(vex3d);
                tr.AddNewlyCreatedDBObject(vex3d, true);

                vex3d = new PolylineVertex3d(new Point3d(Taskas.X + Spindulys / 2, Taskas.Y - Spindulys / 2, 0));
                polyline.AppendVertex(vex3d);
                tr.AddNewlyCreatedDBObject(vex3d, true);

                vex3d = new PolylineVertex3d(new Point3d(Taskas.X + Spindulys / 2, Taskas.Y - Spindulys / 2 - Spindulys / 1.5, 0));
                polyline.AppendVertex(vex3d);
                tr.AddNewlyCreatedDBObject(vex3d, true);

                polyline = new Polyline3d();
                btr.AppendEntity(polyline);
                tr.AddNewlyCreatedDBObject(polyline, true);

                // Linija NR2 (virsutine)
                vex3d = new PolylineVertex3d(new Point3d(Taskas.X - Spindulys / 4, Taskas.Y + Spindulys / 2 + Spindulys / 5, 0));
                polyline.AppendVertex(vex3d);
                tr.AddNewlyCreatedDBObject(vex3d, true);

                vex3d = new PolylineVertex3d(new Point3d(Taskas.X - Spindulys / 4, Taskas.Y + Spindulys / 2 - Spindulys / 5, 0));
                polyline.AppendVertex(vex3d);
                tr.AddNewlyCreatedDBObject(vex3d, true);

                vex3d = new PolylineVertex3d(new Point3d(Taskas.X - Spindulys / 4, Taskas.Y + Spindulys / 2, 0));
                polyline.AppendVertex(vex3d);
                tr.AddNewlyCreatedDBObject(vex3d, true);

                vex3d = new PolylineVertex3d(new Point3d(Taskas.X + Spindulys / 2, Taskas.Y + Spindulys / 2, 0));
                polyline.AppendVertex(vex3d);
                tr.AddNewlyCreatedDBObject(vex3d, true);

                vex3d = new PolylineVertex3d(new Point3d(Taskas.X + Spindulys / 2, Taskas.Y + Spindulys / 2 + Spindulys / 1.5, 0));
                polyline.AppendVertex(vex3d);
                tr.AddNewlyCreatedDBObject(vex3d, true);

                polyline = new Polyline3d();
                btr.AppendEntity(polyline);
                tr.AddNewlyCreatedDBObject(polyline, true);

                // Linija NR3 (kaire)
                vex3d = new PolylineVertex3d(new Point3d(Taskas.X - Spindulys / 2, Taskas.Y + Spindulys / 2, 0));
                polyline.AppendVertex(vex3d);
                tr.AddNewlyCreatedDBObject(vex3d, true);

                vex3d = new PolylineVertex3d(new Point3d(Taskas.X - Spindulys / 2, Taskas.Y - Spindulys / 2, 0));
                polyline.AppendVertex(vex3d);
                tr.AddNewlyCreatedDBObject(vex3d, true);

                vex3d = new PolylineVertex3d(new Point3d(Taskas.X - Spindulys / 2 - Spindulys / 2 - Spindulys / 4, Taskas.Y - Spindulys / 2, 0));
                polyline.AppendVertex(vex3d);
                tr.AddNewlyCreatedDBObject(vex3d, true);

                //Apskritimas FIll Viduryje
                Circle ApskritimasFillMIni = new Circle(new Point3d(Taskas.X + Spindulys / 2, Taskas.Y - Spindulys / 2, 0), new Vector3d(0, 0, 1), Spindulys / 8);
                btr.AppendEntity(ApskritimasFillMIni);
                tr.AddNewlyCreatedDBObject(ApskritimasFillMIni, true);

                Hatch hatch = new Hatch();
                btr.AppendEntity(hatch);
                tr.AddNewlyCreatedDBObject(hatch, true);

                ObjectIdCollection ID = new ObjectIdCollection();
                ID.Add(ApskritimasFillMIni.ObjectId);
                hatch.HatchStyle = HatchStyle.Ignore;
                hatch.SetHatchPattern(HatchPatternType.PreDefined, "SOLID");
                hatch.Associative = true;
                hatch.AppendLoop(HatchLoopTypes.Outermost, ID);
                hatch.EvaluateHatch(true);


                //Trikampis
                Solid trikampis = new Solid(new Point3d(Taskas.X - Spindulys / 4, Taskas.Y, 0), new Point3d(Taskas.X, Taskas.Y + Spindulys / 6, 0), new Point3d(Taskas.X, Taskas.Y - Spindulys / 6, 0));
                btr.AppendEntity(trikampis);
                tr.AddNewlyCreatedDBObject(trikampis, true);

                tr.Commit();
            }
        }

        [CommandMethod("Figura3")]
        public void Figura3()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.GetDocument(db);
            Editor ed = doc.Editor;

            PromptDoubleOptions GetSpindulys = new PromptDoubleOptions("\nĮveskite apskritimų spindulį");
            double Spindulys;
            GetSpindulys.AllowNegative = false;
            GetSpindulys.AllowZero = false;
            GetSpindulys.AllowNone = false;
            PromptDoubleResult GetSpindulysRezultatas = ed.GetDouble(GetSpindulys);
            Spindulys = GetSpindulysRezultatas.Value;

            PromptIntegerOptions GetKiekis = new PromptIntegerOptions("\nĮveskite apskritimų skaičių");
            int Kiekis;
            GetKiekis.AllowNegative = false;
            GetKiekis.AllowZero = false;
            GetSpindulys.AllowNone = false;
            PromptIntegerResult GetKiekisRezultatas = ed.GetInteger(GetKiekis);
            Kiekis = GetKiekisRezultatas.Value;

            PromptPointOptions GetTaskas = new PromptPointOptions("\nPasirinkite pradžios tašką");
            Point3d Taskas;
            GetTaskas.AllowNone = false;
            PromptPointResult GetTaskasRezultatas = ed.GetPoint(GetTaskas);
            Taskas = GetTaskasRezultatas.Value;
            Transaction tr = db.TransactionManager.StartTransaction();

            double kampas = 360 / Kiekis;
            double dabartiniskampas = 0;
            using (tr)
            {
                BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead, false);
                BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite, false);
                for (int j = 0; j < Kiekis; j++)
                {
                    dabartiniskampas += kampas;
                    Point3d beta = new Point3d(Taskas.X + Math.Sin(dabartiniskampas) * Spindulys, Taskas.Y + Math.Cos(dabartiniskampas) * Spindulys, 0);
                    Taskas = beta;
                    Circle Apskritimas = new Circle(Taskas , new Vector3d(0, 0, 1), Spindulys);
                    btr.AppendEntity(Apskritimas);
                    tr.AddNewlyCreatedDBObject(Apskritimas, true);
                }
                tr.Commit();
            }
        }

        [CommandMethod("Figura4")]
        public void Figura4()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.GetDocument(db);
            Editor ed = doc.Editor;

            PromptDoubleOptions GetSpindulys = new PromptDoubleOptions("\nĮveskite didžiausio apskritimo spindulį");
            double Spindulys;
            GetSpindulys.AllowNegative = false;
            GetSpindulys.AllowZero = false;
            GetSpindulys.AllowZero = false;
            PromptDoubleResult GetSpindulysRezultatas = ed.GetDouble(GetSpindulys);
            Spindulys = GetSpindulysRezultatas.Value;

            PromptDoubleOptions GetPlotis = new PromptDoubleOptions("\nĮveskite apskirtimo spindulio mažėjimą");
            double Plotis;
            GetPlotis.AllowNegative = false;
            GetPlotis.AllowZero = false;
            GetPlotis.AllowZero = false;
            PromptDoubleResult GetPlotisRezultatas = ed.GetDouble(GetPlotis);
            Plotis = GetPlotisRezultatas.Value;

            PromptPointOptions GetTaskas = new PromptPointOptions("\nPasirinkite centro tašką");
            Point3d Taskas;
            GetTaskas.AllowNone = false;
            PromptPointResult GetTaskasRezultatas = ed.GetPoint(GetTaskas);
            Taskas = GetTaskasRezultatas.Value;

            Transaction tr = db.TransactionManager.StartTransaction();
            using (tr)
            {
                BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead, false);
                BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite, false);
                int i = 1;
                while(Spindulys > 0)
                {
                    Circle Apskritimas = new Circle(Taskas, new Vector3d(0, 0, 1), Spindulys);
                    btr.AppendEntity(Apskritimas);
                    tr.AddNewlyCreatedDBObject(Apskritimas, true);

                    Hatch hatch = new Hatch();
                    btr.AppendEntity(hatch);
                    tr.AddNewlyCreatedDBObject(hatch, true);

                    ObjectIdCollection ID = new ObjectIdCollection();
                    ID.Add(Apskritimas.ObjectId);
                    hatch.HatchStyle = HatchStyle.Ignore;
                    hatch.SetHatchPattern(HatchPatternType.PreDefined, "SOLID");
                    hatch.Associative = true;
                    hatch.AppendLoop(HatchLoopTypes.Outermost, ID);
                    hatch.EvaluateHatch(true);
                    hatch.ColorIndex = i++;

                    Spindulys -= Plotis;
                }
                tr.Commit();
            }
        }
    }
}
