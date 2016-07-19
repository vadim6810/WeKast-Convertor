using System;
using System.Runtime.InteropServices;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;

namespace WeCastConvertor.Utils
{
    internal class EventLogger
    {
        private static Application _oPpt;
        private static FrmMain _frmMain;

        public EventLogger(FrmMain frmMain, Application pw)
        {
            _frmMain = frmMain;
            //Create an instance of PowerPoint.
            _oPpt = pw;//new Application();
        }

        public void AttachEvents()
        {
            _oPpt.AfterNewPresentation += AfterNewPresentation;
            _oPpt.AfterPresentationOpen += AfterPresentationOpen;
            _oPpt.ColorSchemeChanged += ColorSchemeChanged;
            //_oPpt.NewPresentation += NewPresentation;
            _oPpt.PresentationBeforeClose += PresentationBeforeClose;
            _oPpt.PresentationBeforeSave += PresentationBeforeSave;
            _oPpt.PresentationClose += PresentationClose;
            _oPpt.PresentationCloseFinal += PresentationCloseFinal;
            _oPpt.PresentationNewSlide += PresentationNewSlide;
            _oPpt.PresentationOpen += PresentationOpen;
            _oPpt.PresentationPrint += PresentationPrint;
            _oPpt.PresentationSave += PresentationSave;
            _oPpt.PresentationSync += PresentationSync;
            _oPpt.ProtectedViewWindowActivate += ProtectedViewWindowActivate;
            _oPpt.ProtectedViewWindowBeforeClose += ProtectedViewWindowBeforeClose;
            _oPpt.ProtectedViewWindowBeforeEdit += ProtectedViewWindowBeforeEdit;
            _oPpt.ProtectedViewWindowDeactivate += ProtectedViewWindowDeactivate;
            _oPpt.ProtectedViewWindowOpen += ProtectedViewWindowOpen;
            _oPpt.SlideSelectionChanged += SlideSelectionChanged;
            _oPpt.SlideShowBegin += SlideShowBegin;
            _oPpt.SlideShowEnd += SlideShowEnd;
            _oPpt.SlideShowNextBuild += SlideShowNextBuild;
            _oPpt.SlideShowNextClick += SlideShowNextClick;
            _oPpt.SlideShowNextSlide += SlideShowNextSlide;
            _oPpt.SlideShowOnNext += SlideShowOnNext;
            _oPpt.SlideShowOnPrevious += SlideShowOnPrevious;
            _oPpt.WindowActivate += WindowActivate;
            _oPpt.WindowBeforeDoubleClick += WindowBeforeDoubleClick;
            _oPpt.WindowBeforeRightClick += WindowBeforeRightClick;
            _oPpt.WindowDeactivate += WindowDeactivate;
            _oPpt.WindowSelectionChange += WindowSelectionChange;
        }

        private void SlideShowOnPrevious(SlideShowWindow wn)
        {
            Log("Slide Show On Previous");
        }

        private void SlideShowOnNext(SlideShowWindow wn)
        {
            Log("Slide Show On Next");
        }

        private void ProtectedViewWindowOpen(ProtectedViewWindow protviewwindow)
        {
            Log("Protected View Window Open");
        }

        private void ProtectedViewWindowDeactivate(ProtectedViewWindow protviewwindow)
        {
            Log("Protected View Window Deactivate");
        }

        private void ProtectedViewWindowBeforeEdit(ProtectedViewWindow protviewwindow, ref bool cancel)
        {
            Log("Protected View Window Before Edit");
        }

        private void ProtectedViewWindowBeforeClose(ProtectedViewWindow protviewwindow,
            PpProtectedViewCloseReason protectedviewclosereason, ref bool cancel)
        {
            Log("Protected View Window Before Close");
        }

        private void ProtectedViewWindowActivate(ProtectedViewWindow protviewwindow)
        {
            Log("Protected View Window Activate");
        }

        private void PresentationSync(Presentation pres, MsoSyncEventType synceventtype)
        {
            Log("Presentation Sync");
        }

        private void PresentationCloseFinal(Presentation pres)
        {
            Log("Presentation close final");
        }

        private void PresentationBeforeClose(Presentation pres, ref bool cancel)
        {
            Log("Presentation before close");
        }

        private void AfterPresentationOpen(Presentation pres)
        {
            Log("Presentation open");
        }

        private void AfterNewPresentation(Presentation pres)
        {
            Log("New presentation created");
        }

        [DispId(2001)]
        public void WindowSelectionChange(Selection sel)
        {
            Log("Window Selection Change");
        }

        [DispId(2002)]
        public void WindowBeforeRightClick(Selection sel, ref bool b)
        {
            Log("Window Before Right Click");
        }

        [DispId(2003)]
        public void WindowBeforeDoubleClick(Selection sel, ref bool b)
        {
            Log("Window Before Double Click");
        }

        [DispId(2004)]
        public void PresentationClose(Presentation pres)
        {
            Log("Presentation Close");
        }

        [DispId(2005)]
        public void PresentationSave(Presentation pres)
        {
            Log("Presentation Save");
        }

        [DispId(2006)]
        public void PresentationOpen(Presentation pres)
        {
            Log("Presentation Open");
        }

        [DispId(2007)]
        public void NewPresentation(Presentation pres)
        {
            Log("New Presentation");
        }

        [DispId(2008)]
        public void PresentationNewSlide(Slide sld)
        {
            Log("Presentation New Slide");
        }

        [DispId(2009)]
        public void WindowActivate(Presentation pres, DocumentWindow wn)
        {
            Log("Window Activate");
        }

        [DispId(2010)]
        public void WindowDeactivate(Presentation pres, DocumentWindow wn)
        {
            Log("Window Deactivate");
        }

        [DispId(2011)]
        public void SlideShowBegin(SlideShowWindow wn)
        {
            Log("Slide Show Begin");
        }

        [DispId(2012)]
        public void SlideShowNextBuild(SlideShowWindow wn)
        {
            Log("Slide Show Next Build");
        }

        [DispId(2013)]
        public void SlideShowNextSlide(SlideShowWindow wn)
        {
            Log("Slide Show Next Slide");
        }

        [DispId(2014)]
        public void SlideShowEnd(Presentation pres)
        {
            Log("Slide Show End");
        }

        [DispId(2015)]
        public void PresentationPrint(Presentation pres)
        {
            Log("Presentation Print");
        }

        [DispId(2016)]
        public void SlideSelectionChanged(SlideRange sldRange)
        {
            Log("Slide Selection Changed");
        }

        [DispId(2017)]
        public void ColorSchemeChanged(SlideRange sldRange)
        {
            Log("Color Scheme Changed");
        }

        [DispId(2018)]
        public void PresentationBeforeSave(Presentation pres, ref bool b)
        {
            Log("Presentation Before Save");
        }

        [DispId(2019)]
        public void SlideShowNextClick(SlideShowWindow wn, Effect nEffect)
        {
            Log("Slide Show Next Click");
        }

        public void DetachEvents()
        {
            Marshal.ReleaseComObject(_oPpt);
            GC.Collect();
        }

        private static void Log(string s) => _frmMain.AppendLog(DateTime.Now.ToString("hh:mm:ss") + ": " + s);
    }
}