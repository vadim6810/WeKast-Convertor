using System;
using System.IO;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;

namespace WeCastConvertor.Utils{

    public class SplitAnimation
    {


        public static string splitAnimation(string strFileName, Presentation pptPresentation)
        {
            string splitDir = Path.GetTempPath() + "\\animations";
            // Microsoft.Office.Interop.PowerPoint.Application pptApplication = new Microsoft.Office.Interop.PowerPoint.Application();
            //Presentation pptPresentation = pptApplication.Presentations.Open(strFileName, MsoTriState.msoFalse, MsoTriState.msoFalse, MsoTriState.msoFalse);
            if (Directory.Exists(splitDir))
                DeleteDirectory(splitDir);

            Directory.CreateDirectory(splitDir);
            foreach (Slide slide in pptPresentation.Slides)
            {
                string slidePath = splitDir + "\\" + slide.SlideIndex;
                if (Directory.Exists(slidePath))
                    emptyPresentationFolder(slidePath);
                foreach (Effect ef in slide.TimeLine.MainSequence)
                {
                    if (ef.Timing.TriggerType == MsoAnimTriggerType.msoAnimTriggerOnPageClick && ef.Shape.Type == MsoShapeType.msoPlaceholder)
                    {
                        //original
                        if (!Directory.Exists(slidePath))
                        {
                            Directory.CreateDirectory(slidePath);

                            slide.PublishSlides(slidePath, true, true);
                            string[] files = Directory.GetFiles(slidePath);
                            if (files.Length != 0)
                            {
                                splitPesentationToSlides(files[0], slidePath, true);
                                break;
                            }

                        }
                    }
                    else if (ef.Timing.TriggerType == MsoAnimTriggerType.msoAnimTriggerOnPageClick)
                    {
                        // MessageBox.Show("Slide : " + slide.SlideNumber + " trigger type : " + ef.Timing.TriggerType + " Effect type : " + ef.EffectType + " ShapeType " + ef.Shape.Type);
                        bool isVideo = false;
                        if ((ef.EffectType == MsoAnimEffect.msoAnimEffectCenterRevolve) || (ef.EffectType == MsoAnimEffect.msoAnimEffectFadedSwivel) || (ef.EffectType == MsoAnimEffect.msoAnimEffectFlip)
                            || (ef.EffectType == MsoAnimEffect.msoAnimEffectGrowShrink) || (ef.EffectType == MsoAnimEffect.msoAnimEffectSpin) || (ef.EffectType == MsoAnimEffect.msoAnimEffectStretch)
                            || (ef.EffectType == MsoAnimEffect.msoAnimEffectSwivel) || (ef.EffectType == MsoAnimEffect.msoAnimEffectWave))
                        {
                            isVideo = true;
                        }
                        if (!Directory.Exists(slidePath))
                        {
                            Directory.CreateDirectory(slidePath);
                            slide.PublishSlides(slidePath, true, true);
                            string[] files = Directory.GetFiles(slidePath);
                            if (files.Length != 0)
                            {
                                if (isVideo)
                                {
                                    saveSlideAsWmvFile(files[0], splitDir);
                                    //FileRemovingServer.deletePresentationFile(files[0]);
                                    break;
                                }
                                else
                                {
                                    splitPesentationToSlides(files[0], slidePath, false);
                                    break;
                                }
                            }

                        }

                        //MessageBox.Show("Slide : " + slide.SlideNumber + " trigger type : " + ef.Timing.TriggerType + " Effect type : " + ef.EffectType + " ShapeType " + ef.Shape.Type);
                    }
                }
            }
            //pptPresentation.Close();
            //pptApplication.Quit();
            //pptPresentation.Application.Activate();
            splitPesentationToSlides(openDummyApp(),Path.GetTempPath(),false);
            return splitDir;
        }
        private static string openDummyApp()
        {
            string dummyPath = Path.GetTempPath() + "\\dummy.pptx";
            if(!File.Exists(dummyPath)){
                string path = Path.GetTempPath() + "dummy.pptx";
                //File.WriteAllBytes(path, WeKastRibbon.Properties.Resources.dummy);
            }
            return dummyPath;
        }
        private static void DeleteDirectory(string path)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    //Delete all files from the Directory
                    foreach (string file in Directory.GetFiles(path))
                    {
                        File.Delete(file);
                    }
                    //Delete all child Directories
                    foreach (string directory in Directory.GetDirectories(path))
                    {
                        DeleteDirectory(directory);
                    }
                    //Delete a Directory
                    Directory.Delete(path);
                }
            }
            catch (Exception e)
            {

            }
        }
        private static bool emptyPresentationFolder(string presDir)
        {
            string dirParent = Path.GetDirectoryName(presDir);
            string[] filePaths = Directory.GetFiles(presDir);
            if (filePaths.Length != 0)
            {
                for (int i = 0; i < filePaths.Length; i++)
                {
                    try
                    {
                        File.Delete(filePaths[i]);
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
            }
            Directory.Delete(presDir);
            return true;
        }

        private static void convertToVideo(string presDir)
        {
            string fileName = String.Empty;
            string exportName = "video_of_presentation";
            string exportPath = presDir + @"\video\{0}.wmv";

            string[] filePaths = Directory.GetFiles(presDir);
            if (filePaths.Length != 0)
            {
                for (int i = 0; i < filePaths.Length; i++)
                    fileName = filePaths[i];
                Application ppApp = new Application();
                ppApp.Visible = MsoTriState.msoTrue;
                ppApp.WindowState = PpWindowState.ppWindowMinimized;
                Presentations oPresSet = ppApp.Presentations;
                _Presentation oPres = oPresSet.Open(fileName,
                            MsoTriState.msoFalse, MsoTriState.msoFalse,
                            MsoTriState.msoFalse);
                try
                {
                    oPres.CreateVideo(exportName);
                    oPres.SaveCopyAs(String.Format(exportPath, exportName),
                        PpSaveAsFileType.ppSaveAsWMV,
                        MsoTriState.msoCTrue);
                }
                finally
                {
                    ppApp.Quit();
                }
            }
        }

        private static void saveSlideAsWmvFile(string fileLocationDir, string destDirectory)
        {
            Presentation pptPresentation = null;
            string videosDir = destDirectory + "\\videos";
            string outputFile = String.Empty;
            if (!Directory.Exists(videosDir))
                Directory.CreateDirectory(videosDir);

            string[] paths = fileLocationDir.Split('\\');
            string dir = paths[paths.Length - 2];
            outputFile = videosDir + "\\v" + dir + ".wmv";

            try
            {
                Application pptApplication = new Application();
                pptPresentation = pptApplication.Presentations.Open(fileLocationDir, MsoTriState.msoFalse, MsoTriState.msoFalse, MsoTriState.msoFalse);
                pptPresentation.SaveAs(outputFile, PpSaveAsFileType.ppSaveAsWMV, MsoTriState.msoTriStateMixed);
                long len = 0;
                do
                {
                    System.Threading.Thread.Sleep(500);
                    try
                    {
                        FileInfo f = new FileInfo(outputFile);
                        len = f.Length;
                    }
                    catch
                    {
                        continue;
                    }
                } while (len == 0);
                pptPresentation.Close();
            }
            catch (Exception e)
            {
                pptPresentation.Close();
                //System.Console.WriteLine("Error: " + e.Message);
            }

            //releaseCOM(pptPresentation);

        }
        private static void splitPesentationToSlides(string presFileName, string presPath, bool isPlaceHolder)
        {
            Application pptApplication = new Application();
            Presentation pptPresentation = pptApplication.Presentations.Open(presFileName, MsoTriState.msoFalse, MsoTriState.msoFalse, MsoTriState.msoFalse);
            //original
            //SplitAnimationClass.AddElements(pptPresentation, presPath, isPlaceHolder);

            //pptPresentation.Save();
            //  pptPresentation.Close();
        }
         public static void releaseCOM(object o)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(o);
            }
            catch { }
            finally
            {
                o = null;
            }
        }

    
    }

}
