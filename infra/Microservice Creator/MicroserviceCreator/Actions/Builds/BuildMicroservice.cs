using Cli.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Cli.Actions.Builds
{
    public class BuildMicroservice : ActionChain
    {
        public override ProcessCommand Command => "build microservice";

        protected override async Task Execute(ProcessCommand command)
        {
            var microserviceNameParameter = command.FindParameter("name");

            if (microserviceNameParameter is not null)
            {//path to tempalte
                var microserviceName = microserviceNameParameter.Value;

                var pathToMainFolder = Environment.CurrentDirectory.Split(new String[] { "infra" }, StringSplitOptions.RemoveEmptyEntries)[0];
                var microservicesPath = Path.Combine(pathToMainFolder, "Microservices");
                var templatePath = Path.Combine(pathToMainFolder, "infra", "Microservice Creator", "Template");

                var templateDirectories = Directory.GetDirectories(templatePath, "*", SearchOption.AllDirectories);

                List<Task> changeFileContentTasks = new List<Task>();

                foreach (var currentFolder in templateDirectories)
                {
                    var changedMicroserviceFolderName = ChangeMicroserviceName(microserviceName, currentFolder);

                    var newFolder = changedMicroserviceFolderName.Replace(templatePath, microservicesPath);

                    if (!Directory.Exists(newFolder)) Directory.CreateDirectory(newFolder);

                    var files = Directory.GetFiles(currentFolder);

                    foreach (var currentFile in files)
                    {
                        var changedMicroserviceFileName = ChangeMicroserviceName(microserviceName, currentFile);

                        var newFileFolder = changedMicroserviceFileName.Replace(templatePath, microservicesPath);

                        if (!File.Exists(newFileFolder))
                            File.Create(newFileFolder).Close();

                        changeFileContentTasks.Add(ChangeFileContent(microserviceName, currentFile, newFileFolder));
                    }
                }

                Task.WaitAll(changeFileContentTasks.ToArray());
            }
        }

        private static async Task ChangeFileContent(string microserviceName, string templateFilePath, string newFilePath)
        {
            var fileContent = await File.ReadAllTextAsync(templateFilePath);

            var changedContent = ChangeMicroserviceName(microserviceName, fileContent);

            await File.WriteAllTextAsync(newFilePath, changedContent);
        }

        private static string ChangeMicroserviceName(string microserviceName, string currentFile)
        {
            return currentFile
                .Replace("Expense", microserviceName[0].ToString().ToUpper() + microserviceName[1..])
                .Replace("expense", microserviceName[0].ToString().ToLower() + microserviceName[1..]);
        }

        protected override Task Help(ProcessCommand command)
        {
            Console.WriteLine("Example: build microservice --name=Invoice");

            return Task.CompletedTask;
        }
    }
}
