#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;
#endregion

namespace MultiRevitProject
{
    internal class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication a)
        {
#if REVIT2022
            TaskDialog.Show("Test", "This is a Revit 2022 build");
#elif REVIT2023
            TaskDialog.Show("Test", "This is a Revit 2023 build");
#else
            TaskDialog.Show("Test", "This is a different Revit version build.");
#endif
            string assemblyName = GetAssemblyName();

            // step 1: create ribbon tab (if needed)
            try
            {
                a.CreateRibbonTab("Revit Add-in Bootcamp");
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
                Debug.Print(ex.Message);
                return Result.Failed;
            }

            // step 2: create ribbon panel(s)
            RibbonPanel panel1 = a.CreateRibbonPanel("Revit Add-in Bootcamp", "Panel 1");
            RibbonPanel panel2 = a.CreateRibbonPanel("Revit Add-in Bootcamp", "Panel 2");
            RibbonPanel panel3 = a.CreateRibbonPanel("Panel 3");

            // step 3: create button data instances
            PushButtonData pData1 = new PushButtonData("button1", "This Is \rButton 1", assemblyName, "MultiRevitProject.Command");
            PushButtonData pData2 = new PushButtonData("button2", "Button 2", assemblyName, "MultiRevitProject.Command");
            PushButtonData pData3 = new PushButtonData("button3", "Button 3", assemblyName, "MultiRevitProject.Command");
            PushButtonData pData4 = new PushButtonData("button4", "Button 4", assemblyName, "MultiRevitProject.Command");
            PushButtonData pData5 = new PushButtonData("button5", "Button 5", assemblyName, "MultiRevitProject.Command");
            PushButtonData pData6 = new PushButtonData("button6", "This is Button 6", assemblyName, "MultiRevitProject.Command");

            PulldownButtonData pullDownData1 = new PulldownButtonData("pulldown1", "Pulldown Button");
            SplitButtonData splitData1 = new SplitButtonData("split1", "Split Button");

            // step 4: add images
            pData1.LargeImage = BitmapToImageSource(MultiRevitProject_Resources.Properties.Resources.Blue_32);
            pData1.Image = BitmapToImageSource(MultiRevitProject_Resources.Properties.Resources.Blue_16);
            pData2.LargeImage = BitmapToImageSource(MultiRevitProject_Resources.Properties.Resources.Red_32);
            pData2.Image = BitmapToImageSource(MultiRevitProject_Resources.Properties.Resources.Red_16);
            pData3.LargeImage = BitmapToImageSource(MultiRevitProject_Resources.Properties.Resources.Yellow_32);
            pData3.Image = BitmapToImageSource(MultiRevitProject_Resources.Properties.Resources.Yellow_16);
            pData4.LargeImage = BitmapToImageSource(MultiRevitProject_Resources.Properties.Resources.Green_32);
            pData4.Image = BitmapToImageSource(MultiRevitProject_Resources.Properties.Resources.Green_16);
            pData5.LargeImage = BitmapToImageSource(MultiRevitProject_Resources.Properties.Resources.Blue_32);
            pData5.Image = BitmapToImageSource(MultiRevitProject_Resources.Properties.Resources.Blue_16);
            pData6.LargeImage = BitmapToImageSource(MultiRevitProject_Resources.Properties.Resources.Red_32);
            pData6.Image = BitmapToImageSource(MultiRevitProject_Resources.Properties.Resources.Red_16);

            pullDownData1.LargeImage = BitmapToImageSource(MultiRevitProject_Resources.Properties.Resources.Blue_32);

            // step 5: add tool tips
            pData1.ToolTip = "Button 1 tool tip";
            pData2.ToolTip = "Button 2 tool tip";

            // step 6: create buttons
            panel1.AddItem(pData1);

            SplitButton split1 = panel1.AddItem(splitData1) as SplitButton;
            split1.AddPushButton(pData3);
            split1.AddPushButton(pData4);

            PulldownButton pull1 = panel2.AddItem(pullDownData1) as PulldownButton;
            pull1.AddPushButton(pData5);


            panel1.AddStackedItems(pData2, pData6);

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }

        private BitmapImage BitmapToImageSource(Bitmap bm)
        {
            using (MemoryStream mem = new MemoryStream())
            {
                bm.Save(mem, System.Drawing.Imaging.ImageFormat.Png);
                mem.Position = 0;
                BitmapImage bmi = new BitmapImage();
                bmi.BeginInit();
                bmi.StreamSource = mem;
                bmi.CacheOption = BitmapCacheOption.OnLoad;
                bmi.EndInit();

                return bmi;
            }
        }

        private string GetAssemblyName()
        {
            string assemblyName = Assembly.GetExecutingAssembly().Location;
            return assemblyName;
        }
    }
}
