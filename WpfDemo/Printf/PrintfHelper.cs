using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Xps.Packaging;

namespace LKsoft.Noah.Common.Helper
{
    public static class PrintfHelper
    {
        public static ImageSource CreateImage(FrameworkElement element)
        {
            if (element == null)
            {
                return null;
            }
            var width = (int)element.ActualWidth;
            var height = (int)element.ActualHeight;
            var rtb = new RenderTargetBitmap(width, height, 0, 0, PixelFormats.Default);
            rtb.Render(element);
            return rtb;
        }

        /// <summary>
        /// 加载控件到文档
        /// </summary>
        /// <param name="documentViewer1"></param>
        /// <param name="controlToPrint"></param>
        public static void LoadDocumentByControl(this DocumentViewer documentViewer1, FrameworkElement controlToPrint)
        {
            var imageSource = CreateImage(controlToPrint);
            var imageControl = new Image { Source = imageSource };

            var fixedDoc = new FixedDocument();
            var pageContent = new PageContent();
            var fixedPage = new FixedPage();
            //  fixedPage.Children.Add(controlToPrint);
            fixedPage.Children.Add(imageControl);

            ((IAddChild)pageContent).AddChild(fixedPage);
            fixedDoc.Pages.Add(pageContent);
            documentViewer1.Document = fixedDoc;
        }

        /// <summary>
        /// 打印预览界面保存到XPS文档
        /// </summary>
        /// <param name="documentViewer1"></param>
        public static void SaveDocument(this DocumentViewer documentViewer1)
        {
            var dlg = new SaveFileDialog();
            dlg.FileName = "test.xps";
            dlg.Filter = "XPS Documents (.xps)|*.xps";
            var result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                var doc = (FixedDocument)documentViewer1.Document;
                using (var xpsDocument = new XpsDocument(filename, FileAccess.ReadWrite))
                {
                    var xpsWriter = XpsDocument.CreateXpsDocumentWriter(xpsDocument);
                    xpsWriter.Write(doc);
                    xpsDocument.Close();
                }
            }
        }

        /// <summary>
        /// 加载文件到文档
        /// </summary>
        /// <param name="documentViewer1"></param>
        public static void LoadDocument(this DocumentViewer documentViewer1)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "XPS 文档(*.xps)|*.xps";
            if (dialog.ShowDialog() == true)
            {
                XpsDocument doc = new XpsDocument(dialog.FileName, FileAccess.Read);
                documentViewer1.Document = doc.GetFixedDocumentSequence();
            }
        }

        /// <summary>
        /// 打印界面
        /// </summary>
        /// <param name="element"></param>
        public static void PrintVisual(FrameworkElement element)
        {
            var dialog = new PrintDialog();
            if (dialog.ShowDialog() == true)
            {
                dialog.PrintVisual(element, "打印");
            }
        }
    }
}
