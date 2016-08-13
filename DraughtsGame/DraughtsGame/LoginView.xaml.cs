using DraughtsGame.GameService;
using DraughtsGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DraughtsGame
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        
        private ServerEventsCallback serverEventsCallback;
        private GameServiceClient gameServiceClient;

        public LoginView()
        {
            InitializeComponent();
            loginButton.IsEnabled = false;
            loginButton.Click += loginButton_Click;
            aboutButton.Click += aboutButton_Click;

            groupIdTextBox.TextChanged += groupIdTextBox_TextChanged;
            groupIdTextBox.PreviewMouseDown += groupIdTextBox_PreviewMouseDown;
            groupIdTextBox.Text = "Enter Game Code";
            groupIdTextBox.Tag = -1;

            serverEventsCallback = new ServerEventsCallback();
            gameServiceClient = new GameServiceClient(new System.ServiceModel.InstanceContext(serverEventsCallback));

        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            int groupId = Int32.Parse(groupIdTextBox.Text);

            GroupData groupData = gameServiceClient.LogIn(groupId);

            MainView mainView = new MainView(groupId, groupData, gameServiceClient, serverEventsCallback);
            mainView.Show();

            Application.Current.Windows[0].Close();
        }

        private void aboutButton_Click(object sender, RoutedEventArgs e)
        {

            //AboutBox box = new AboutBox();
            //box.ShowDialog();
        }

        private void groupIdTextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            
            loginButton.IsEnabled = (groupIdTextBox.Tag != null && !groupIdTextBox.Tag.Equals(-1)) && (groupIdTextBox.Text.Length > 0);

        }

        private void groupIdTextBox_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            if(groupIdTextBox.Tag.Equals(-1))
            {
                groupIdTextBox.Tag = 0;
                groupIdTextBox.Text = "";
            }
            
   
        }

    

    }
}
