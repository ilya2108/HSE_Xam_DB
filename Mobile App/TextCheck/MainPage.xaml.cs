using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;

namespace TextCheck
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public string URL = "https://textcheck.azurewebsites.net/api/AddToDB";
        public MainPage()
        {
            InitializeComponent();
        }

        async void OnCheck(System.Object sender, System.EventArgs e)
        {
            var obj = new
            {
                name = StudentName.Text,
                surname = StudentSurname.Text,
                text = StudentText.Text
            };

            using(var client = new HttpClient())
            {
                try
                {
                    var payload = JsonConvert.SerializeObject(obj);
                    var content = new StringContent(payload, Encoding.UTF8, @"application/json");

                    var response = await client.PostAsync(URL, content);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        await DisplayAlert("Success", responseBody, "OK");
                        /*var phrases = createKeyPhrasesList(responseBody);
                        string result = string.Empty;
                        foreach (var phrase in phrases)
                        {
                            result += phrase + "\n";
                        }
                        await DisplayAlert("Ура",
                            "Список ключевых фраз\n" + result,
                            "OK");
                        */
                    }
                    else
                    {
                        throw new ArgumentException();
                    }
                }
                catch
                {
                    await DisplayAlert("Error!",
                        "Something went wrong. Please try again later (maybe no...)",
                        "OK");
                }
                
            }


        }

        /// <summary>
        /// Function that creates an array of key phrases
        /// </summary>
        /// <param name="response">Response string from API</param>
        /// <returns></returns>
        string[] createKeyPhrasesList(string response)
        {
            response = response.Replace("[", string.Empty)
                .Replace("]", string.Empty)
                .Replace("\"", string.Empty);

            return response.Split(',');
        }
    }
}
