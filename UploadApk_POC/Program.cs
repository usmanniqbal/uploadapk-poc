using System;
using System.IO;

namespace UploadApk_POC
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");

			FileStream credentialsStream = new FileStream(@"../../../api-6446943526994748600-986635-339ce77700d1.json", FileMode.Open);
			var credentials = Google.Apis.Auth.OAuth2.ServiceAccountCredential.FromServiceAccountData(credentialsStream);

			Google.Apis.AndroidPublisher.v3.AndroidPublisherService androidPublisherService = new Google.Apis.AndroidPublisher.v3.AndroidPublisherService(
				new Google.Apis.AndroidPublisher.v3.AndroidPublisherService.Initializer
				{
					ApplicationName = "EventMobi-POC/1.1",
					HttpClientInitializer = credentials,
					ApiKey = "Enter your API Key Here",
				});
			const string id = "123123123";
			const string packageName = "com.em.flutter_bundle_upload_app";
			var appEdit = new Google.Apis.AndroidPublisher.v3.Data.AppEdit
			{
				Id = id,
			};

			//var validate = androidPublisherService.Edits.Validate(packageName, id).Execute();
			//var edit = androidPublisherService.Edits.Insert(appEdit, null).Execute();

			FileStream fs = new FileStream(@"C:\Users\Usman\AndroidStudioProjects\flutter_bundle_upload_app\build\app\outputs\bundle\release\app-release.aab", FileMode.Open);
			//fs.Position = 0;
			var upload = androidPublisherService.Edits.Bundles.Upload(packageName, id, fs, "application/octet-stream");
			upload.ProgressChanged += Upload_ProgressChanged;
			upload.ResponseReceived += Upload_ResponseReceived;
			upload.Upload();
			Console.ReadLine();
		}

		private static void Upload_ResponseReceived(Google.Apis.AndroidPublisher.v3.Data.Bundle obj)
		{
			Console.WriteLine("Received Response: {0}", obj.ETag);
		}

		private static void Upload_ProgressChanged(Google.Apis.Upload.IUploadProgress obj)
		{
			Console.WriteLine("Status: {0}", obj.Status.ToString());
			if (obj.Exception != null)
			{
				Console.WriteLine("Details: {0}", obj.Exception.Message);
			}
			Console.WriteLine();
		}
	}
}