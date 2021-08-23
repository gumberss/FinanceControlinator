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

            var microserviceType = command.FindParameter("type") ?? "nosql";

            if (microserviceNameParameter is not null)
            {   //path to tempalte
                var microserviceName = microserviceNameParameter.Value;
                var templateMicroserviceName = microserviceType.Value == "nosql" ? "Payment" : "Expense";

                var pathToMainFolder = Environment.CurrentDirectory.Split(new String[] { "infra" }, StringSplitOptions.RemoveEmptyEntries)[0];
                var microservicesPath = Path.Combine(pathToMainFolder, "Microservices", microserviceName + "s");
                var templatePath = Path.Combine(pathToMainFolder, "infra", "Microservice Creator", "Template", templateMicroserviceName + "s");

                var templateDirectories = Directory.GetDirectories(templatePath, "*", SearchOption.AllDirectories);

                List<Task> changeFileContentTasks = new List<Task>();

                foreach (var currentFolder in templateDirectories)
                {
                    var newFolder = ChangeMicroserviceName(microserviceName, templateMicroserviceName, currentFolder.Replace(templatePath, microservicesPath));

                    if (!Directory.Exists(newFolder)) Directory.CreateDirectory(newFolder);

                    var files = Directory.GetFiles(currentFolder);

                    foreach (var currentFile in files)
                    {
                        var newFileFolder = ChangeMicroserviceName(microserviceName, templateMicroserviceName, currentFile.Replace(templatePath, microservicesPath));

                        if (!File.Exists(newFileFolder))
                            File.Create(newFileFolder).Close();

                        changeFileContentTasks.Add(ChangeFileContent(microserviceName, templateMicroserviceName, currentFile, newFileFolder));
                    }
                }

                Task.WaitAll(changeFileContentTasks.ToArray());
            }
        }

        private static async Task ChangeFileContent(string microserviceName,string templateMicroserviceName, string templateFilePath, string newFilePath)
        {
            var fileContent = await File.ReadAllTextAsync(templateFilePath);

            var changedContent = ChangeMicroserviceName(microserviceName, templateMicroserviceName, fileContent);

            await File.WriteAllTextAsync(newFilePath, changedContent);
        }

        private static string ChangeMicroserviceName(string microserviceName, string templateMicroserviceName, string currentFile)
        {
            var captalizeTemplateName = templateMicroserviceName.ToUpper()[0] + templateMicroserviceName[1..].ToLower();

            return currentFile
                .Replace(captalizeTemplateName, microserviceName[0].ToString().ToUpper() + microserviceName[1..])
                .Replace(templateMicroserviceName.ToLower(), microserviceName[0].ToString().ToLower() + microserviceName[1..]);
        }

        protected override Task Help(ProcessCommand command)
        {
            Console.WriteLine("Example: build microservice --name=Invoice --nosql");

            return Task.CompletedTask;
        }
    }
}
