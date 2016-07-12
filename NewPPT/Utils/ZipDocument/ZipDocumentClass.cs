using System;
using System.IO;
using System.Threading;
using Shell32;

namespace NewPPT.Utils.ZipDocument
{
    internal class ZipDocumentClass
    {
        public static string ZipDocumentStructure(string sourseFolder, string destinationFolder, string fileName)
        {
            //string sourseFolder = @"C:\Users\hp\Desktop\OutputFiles";
            //string destinationFolder = @"C:\Users\hp\Desktop\InputFiles\test.zip";

            var zipDir = destinationFolder + "..\\..\\zip";
            zipDir = Path.GetFullPath(zipDir);
            if (!Directory.Exists(zipDir))
                Directory.CreateDirectory(zipDir);
            destinationFolder = zipDir + "\\" + fileName + ".zip"; //".ezs";
            //Create an empty zip file
            byte[] emptyzip = {80, 75, 5, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};

            var fs = File.Create(destinationFolder);
            fs.Write(emptyzip, 0, emptyzip.Length);
            fs.Flush();
            fs.Close();

            //Copy a folder and its contents into the newly created zip file
            var sc = new Shell();

            var srcFlder = sc.NameSpace(sourseFolder);
            var destFlder = sc.NameSpace(destinationFolder);
            //Shell32.FolderItems items = SrcFlder.Items();

            //Add _DELETE_ME_ file to each empty folder
            AddNullFiles(sourseFolder);

            var destFlder3 = (Folder3) sc.NameSpace(sourseFolder);
            var items3 = (FolderItems3) destFlder3.Items();
            var shcontfIncludehidden = 128;
            var shcontfFolders = 32;
            var shcontfNonfolders = 64;
            //"*" == all files
            items3.Filter(shcontfIncludehidden | shcontfNonfolders | shcontfFolders, "*");

            //Count the number of FolderItems in the original source location
            var originalItemCount = RecurseCount3(items3);

            //Start the ziping
            destFlder.CopyHere(items3, 1024);

            //Timeout period... if the compression is not done within this time
            //limit then the zip.exe shuts down and the ziping is stoped
            var timeoutDeadline = DateTime.Now.AddMinutes(1);

            //Wait until the ziping is done.
            for (;;)
            {
                //Are we past the deadline?
                if (DateTime.Now > timeoutDeadline)
                {
                    break;
                }

                //Check the number of items in the new zip to see if it matches
                //the number of items in the original source location

                //Only check the item count every 5 seconds
                Thread.Sleep(5000);

                var zipFileItemCount = RecurseCount(destFlder.Items());

                if (originalItemCount == zipFileItemCount)
                {
                    break;
                }
            }


            //Remove all _DELETE_ME_ files from the source
            DeleteNullFiles(srcFlder.Items());

            //First create a zip_temp folder where the zip.exe is at so we can
            //cut paste from the zip folder into this zip_temp folder
            var moveToFolder = sc.NameSpace(AppDomain.CurrentDomain.BaseDirectory);
            moveToFolder.NewFolder("zip_temp", 0);

            FolderItem tempFolder = null;

            //Find the zip_temp folder
            foreach (FolderItem item in moveToFolder.Items())
            {
                if (item.Name == "zip_temp")
                {
                    tempFolder = item;
                }
            }

            DeleteNullFilesFromZip(destFlder.Items(), tempFolder);
            var changedPath = Path.ChangeExtension(destinationFolder, ".ezs");
            File.Move(destinationFolder, changedPath);
            return changedPath;
        }

        private static void DeleteNullFilesFromZip(FolderItems source, FolderItem tempFolder)
        {
            //for each file that we find with the name _DELETE_ME_ cut and
            //paste it into the TempFolder
            foreach (FolderItem item in source)
            {
                if (item.IsFolder)
                {
                    DeleteNullFilesFromZip(((Folder) item.GetFolder).Items(), tempFolder);
                }
                else
                {
                    if (item.Name == "_DELETE_ME_")
                    {
                        //If there is already a file there by that name then delete it
                        if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "zip_temp\\_DELETE_ME_"))
                        {
                            File.Delete(AppDomain.CurrentDomain.BaseDirectory + "zip_temp\\_DELETE_ME_");
                        }
                        //Move file out of zip
                        item.InvokeVerb("Cut");
                        tempFolder.InvokeVerb("Paste");
                    }
                }
            }

            //Once that is all done remove all files from the zip_temp folder
            //and then delete the zip_temp folder
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "zip_temp\\_DELETE_ME_"))
            {
                File.Delete(AppDomain.CurrentDomain.BaseDirectory + "zip_temp\\_DELETE_ME_");
            }

            //Delete the zip_temp folder
            if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "zip_temp"))
            {
                Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + "zip_temp", true);
            }
        }


        //Add _DELETE_ME_ files to each empty folder
        private static void AddNullFiles(string currentDir)
        {
            //add A NULL file to empty dirs
            var files = Directory.GetFiles(currentDir, "*.*");
            //Now recurse to sub dirs
            var subdirs = Directory.GetDirectories(currentDir);
            foreach (var dir in subdirs)
            {
                AddNullFiles(dir);
            }
            if ((files.Length == 0) && (subdirs.Length == 0))
            {
                var fs = File.Create(currentDir + "\\_DELETE_ME_");
                fs.Close();
            }
        }

        //Remove all _DELETE_ME_ files from all folders
        private static void DeleteNullFiles(FolderItems source)
        {
            foreach (FolderItem item in source)
            {
                if (item.IsFolder)
                {
                    DeleteNullFiles(((Folder) item.GetFolder).Items());
                }
                else
                {
                    if (item.Name.EndsWith("_DELETE_ME_"))
                    {
                        File.Delete(item.Path);
                    }
                }
            }
        }


        //Get the number of files and folders in the source location
        //including all subfolders
        private static int RecurseCount(FolderItems source)
        {
            var itemCount = 0;

            foreach (FolderItem item in source)
            {
                if (item.IsFolder)
                {
                    //Add one for this folder
                    itemCount++;
                    //Then continue walking down the folder tree
                    itemCount += RecurseCount(((Folder) item.GetFolder).Items());
                }
                else
                {
                    //Add one for this file
                    itemCount++;
                }
            }

            return itemCount;
        }

        //Get the number of files and folders in the source location
        //including all subfolders and hidden files
        private static int RecurseCount3(FolderItems3 source)
        {
            var itemCount = 0;

            foreach (FolderItem item in source)
            {
                if (item.IsFolder)
                {
                    //Add one for this folder
                    itemCount++;
                    var items3 = (FolderItems3) ((Folder3) item.GetFolder).Items();
                    var shcontfIncludehidden = 128;
                    var shcontfFolders = 32;
                    var shcontfNonfolders = 64;
                    items3.Filter(shcontfIncludehidden | shcontfNonfolders | shcontfFolders, "*");
                    //Then continue walking down the folder tree
                    itemCount += RecurseCount3(items3);
                }
                else
                {
                    //Add one for this file
                    itemCount++;
                }
            }

            return itemCount;
        }
    }
}