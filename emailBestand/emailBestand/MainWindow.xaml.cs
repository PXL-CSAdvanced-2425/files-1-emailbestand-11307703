using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace emailBestand
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<string, string> _dictGeg = new Dictionary<string, string>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void readButton_Click(object sender, RoutedEventArgs e)
        {
            InlezenBestand("Email.txt");
            
        }




        public StringBuilder InlezenBestand(string bestandsnaam)
        {
            StringBuilder sb = new StringBuilder();
            using (StreamReader sr = new StreamReader(bestandsnaam))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] fixedLine = line.Replace("\"", "").Split(',');
                    string formattedLine = $"{fixedLine[0],-35} : {fixedLine[1]}";            
                    sb.AppendLine(formattedLine);
                }
              
            }
            resultTextBox.Text = sb.ToString();
            return sb;
        }

        private void readAndDialogButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog()
                {
                    Filter = "Textbestand|*txt",
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    FileName = "Email.txt"
                };
                if (ofd.ShowDialog() == true)
                {
                    using (FileStream fs = File.OpenRead(ofd.FileName))
                    {
                        InlezenBestand(ofd.FileName);
                    }
                }
            }
            catch (IOException io)
            {

                MessageBox.Show("Fout bij het lezen van bestand");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Onverwachte fout opgetreden");
            }
           
        }

        private void readAndDictionaryButton_Click(object sender, RoutedEventArgs e)
        {

            using (StreamReader sr = new StreamReader("Email.txt"))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] fixedLine = line.Replace("\"", "").Split(',');
                    string formattedLine = $"{fixedLine[0],-35} : {fixedLine[1]}";
                    _dictGeg[fixedLine[0]] = fixedLine[1];
                }

                StringBuilder sb = new StringBuilder();

                foreach (var line in _dictGeg)
                {
                    sb.AppendLine($"{line.Key} : {line.Value}");
                }
                resultTextBox.Text = sb.ToString();

            }
        }

        private void deleteAndDictionaryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("Adressen.txt", false, Encoding.UTF8))
                {
                    foreach (var line in _dictGeg)
                    {
                        sw.WriteLine($"{line.Key},{line.Value}"); // CSV format
                    }
                }

                MessageBox.Show("Bestand succesvol opgeslagen!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fout bij het wegschrijven van het bestand: {ex.Message}");
            }
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                FileName = "Email.txt",
                Filter = "Textbestand|*txt",
                Title = "Kies een bestand om gegevens toe te voegen"
            };
            if (sfd.ShowDialog() == true)
            {
                using (StreamWriter sw = new StreamWriter(sfd.FileName, true))
                {
                    
                    sw.WriteLine($"\"{nameTextBox.Text}\",\"{emailTextBox.Text}\""); 
                }

                MessageBox.Show("Gegevens succesvol toegevoegd!");
            }
        }
    }
}