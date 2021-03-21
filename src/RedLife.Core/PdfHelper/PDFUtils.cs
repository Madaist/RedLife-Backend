using Abp.Dependency;
using System;
using System.IO;

namespace RedLife.Core.PdfHelper
{
    public class PDFUtils : ISingletonDependency
    {
        public static void ConvertToPdf(string content, string donationId)
        {
            byte[] bytes = Convert.FromBase64String(content.Substring(28));

            var enviroment = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(enviroment).Parent.FullName;
            var fileLocation = $"{projectDirectory}/MedicalTestResults/{donationId}.pdf";
            if (File.Exists(fileLocation))
            {
                File.Delete(fileLocation);
            }

            FileStream stream = new FileStream(fileLocation, FileMode.CreateNew);
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(bytes, 0, bytes.Length);
            writer.Close();
        }

        public static string ConvertToBase64String(string filePath)
        {
            Byte[] bytes = File.ReadAllBytes(filePath);
            String file = Convert.ToBase64String(bytes);
            return file;
        }

        public static string GetPdfOrNull(string donationId)
        {
            var enviroment = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(enviroment).Parent.FullName;
            var pdfPath = $"{projectDirectory}/MedicalTestResults/{donationId}.pdf";

            if (File.Exists(pdfPath))
            {
                return PDFUtils.ConvertToBase64String(pdfPath);
            }
            return null;
        }

    }
}
